using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using EntitiesDb.Cache;
using EntitiesDb.Components;
using EntitiesDb.Data;
using EntitiesDb.Events;
using EntitiesDb.Queries;

namespace EntitiesDb
{
    public sealed partial class EntityDatabase : IDisposable
    {
        private const int DefaultParallelIndicesSize = 32;

        private static readonly ParallelOptions s_defaultParallelOptions = new();

        private readonly List<EntityGroup> _groups = new();
        private readonly Dictionary<EntityArchetype, EntityGroup> _archetypeMap = new();
        private readonly Dictionary<long, List<EntityGroup>> _archetypeIndexBuckets = new();
        private readonly Dictionary<uint, EntityReference> _entityMap = new();
        private readonly Dictionary<EntityGroup, int> _parallelIndicesMap = new();
        private readonly Stack<List<EnumerationJob>> _jobListCache = new();
        private readonly EventDispatcher _eventDispatcher = new();

        private readonly List<Exception> _eventExceptions = new();

        private int[] _parallelIndices = new int[DefaultParallelIndicesSize * 6];
        private int _nextParallelIndex = 0;
        private uint _nextEntityId = 1;
        private int _iterators = 0;
        private int _inParallel = 0;
        private bool _inEvent = false;

        public int Count { get; private set; }
        public IReadOnlyList<Exception> EventExceptions => _eventExceptions;
        public bool ParallelEnabled { get; set; } = true;
        public ParallelOptions ParallelOptions { get; set; } = new();

        public EntityDatabase()
        {
            PrePopulateCaches();
        }

        public unsafe ref T AddComponent<T>(uint entityId, T data = default) where T : unmanaged
        {
            ref var component = ref ComponentRegistry.Get<T>();
            component.AddLayout.Set(data);
            ApplyLayout(entityId, component.AddLayout);
            ref var reference = ref CollectionsMarshal.GetValueRefOrNullRef(_entityMap, entityId);
            return ref reference.GetComponent<T>();
        }

        public unsafe void ApplyLayout(uint entityId, EntityLayout layout)
        {
            if (layout is null) throw new ArgumentNullException(nameof(layout));
            ThrowIfStructuralChangeBlocked();
            ref var reference = ref CollectionsMarshal.GetValueRefOrNullRef(_entityMap, entityId);
            if (Unsafe.IsNullRef(ref reference)) ThrowEntityNotFound();
            ClearEventExceptions();

            // determine destination archetype
            var currentArchetype = reference.Group?.Archetype ?? default;
            var depthResult = layout.GetDepthResult(currentArchetype);
            var destinationArchetype = ArchetypeCache.Rent(depthResult);
            layout.Apply(currentArchetype, destinationArchetype);

            // establish destination group
            var destinationGroup = GetOrCreateGroup(destinationArchetype);
            if (destinationGroup != reference.Group)
            {
                var destination = destinationGroup != null ? new EntityReference(destinationGroup, destinationGroup.Add(entityId)) : default;

                // publish remove events
                if (reference.Group != null)
                {
                    var currentChunk = reference.Group.GetChunk(reference.Index.Chunk);
                    int i = 0;
                    byte* destinationPtr = null;
                    foreach (var id in layout.RemoveArchetype.GetIds())
                    {
                        ref var componentType = ref ComponentRegistry.Get(id);
                        if (!componentType.ZeroSize)
                        {
                            while (reference.Group.ComponentIds[i] < id) i++;
                            if (reference.Group.ComponentIds[i] > id) break; // this type is not contained in the current archetype

                            var size = ComponentRegistry.Get(id).Size;
                            destinationPtr = currentChunk.GetComponent(reference.Group.ListOffsets[i], size, reference.Index.List);
                        }
                        else destinationPtr = null;

                        componentType.OnRemove(this, entityId, destinationPtr);
                    }
                }

                // copy shared components
                reference.CopyComponentsInto(destination);

                // remove from current group
                if (reference.Group != null)
                {
                    var remappedEntityId = reference.Group.Remove(reference.Index);
                    if (remappedEntityId != 0)
                    {
                        // remap a moved entity
                        ref var remappedReference = ref CollectionsMarshal.GetValueRefOrAddDefault(_entityMap, remappedEntityId, out var exists);
                        if (exists) remappedReference = new EntityReference(remappedReference.Group, reference.Index);
                    }
                }

                // remap
                reference = destination;
            }

            // apply layout data
            if (reference.Group != null)
            {
                var destinationChunk = reference.Group.GetChunk(reference.Index.Chunk);
                int i = 0;
                byte* destinationPtr = null;
                fixed (byte* data = layout.ComponentData)
                {
                    int offset = 0;
                    var hasDataArchetype = new EntityArchetype(layout.HasDataMask);
                    foreach (var id in layout.AddArchetype.GetIds())
                    {
                        ref var componentType = ref ComponentRegistry.Get(id);
                        if (!componentType.ZeroSize)
                        {
                            while (reference.Group.ComponentIds[i] < id) i++;
                            var size = ComponentRegistry.Get(id).Size;
                            destinationPtr = destinationChunk.GetComponent(reference.Group.ListOffsets[i], size, reference.Index.List);
                            if (hasDataArchetype.Contains(id))
                            {
                                var sourcePtr = data + offset;
                                Buffer.MemoryCopy(sourcePtr, destinationPtr, size, size);
                            }
                            offset += size;
                        }
                        else destinationPtr = null;
                    }
                }

                // publish add events
                i = 0;
                foreach (var id in layout.AddArchetype.GetIds())
                {
                    if (currentArchetype.Contains(id)) continue; // already contains this type

                    ref var componentType = ref ComponentRegistry.Get(id);
                    if (!componentType.ZeroSize)
                    {
                        while (reference.Group.ComponentIds[i] < id) i++;
                        var size = ComponentRegistry.Get(id).Size;
                        destinationPtr = destinationChunk.GetComponent(reference.Group.ListOffsets[i], size, reference.Index.List);
                    }
                    else destinationPtr = null;

                    componentType.OnAdd(this, entityId, destinationPtr);
                }
            }

            // cleanup
            ArchetypeCache.Return(destinationArchetype);
        }

        public unsafe uint CloneEntity(uint entityId)
        {
            if (!_entityMap.ContainsKey(entityId)) ThrowEntityNotFound();

            var newEntityId = CreateEntity();
            ref var source = ref CollectionsMarshal.GetValueRefOrNullRef(_entityMap, entityId);
            ref var destination = ref CollectionsMarshal.GetValueRefOrNullRef(_entityMap, newEntityId);
            ClearEventExceptions();

            if (source.Group != null)
            {
                destination = new EntityReference(source.Group, source.Group.Add(newEntityId));

                // copy shared components
                source.CopyComponentsInto(destination);

                // publish add events
                var destinationChunk = destination.Group.GetChunk(destination.Index.Chunk);
                int i = 0;
                byte* destinationPtr = null;
                foreach (var id in destination.Group.Archetype.GetIds())
                {
                    ref var componentType = ref ComponentRegistry.Get(id);
                    if (!componentType.ZeroSize)
                    {
                        while (destination.Group.ComponentIds[i] < id) i++;
                        var size = ComponentRegistry.Get(id).Size;
                        destinationPtr = destinationChunk.GetComponent(destination.Group.ListOffsets[i], size, destination.Index.List);
                    }
                    else destinationPtr = null;

                    componentType.OnAdd(this, entityId, destinationPtr);
                }
            }
            return newEntityId;
        }

        public uint CreateEntity() => CreateEntity(0);
        public uint CreateEntity(uint entityId)
        {
            ThrowIfStructuralChangeBlocked();
            if (entityId == 0) entityId = GenerateEntityId();
            else if (_entityMap.ContainsKey(entityId)) throw new Exception($"Cannot create entity, entity id is already in use!");
            _entityMap.Add(entityId, default);
            Count++;
            return entityId;
        }

        public uint CreateEntity(EntityLayout layout) => CreateEntity(0, layout);
        public uint CreateEntity(uint entityId, EntityLayout layout)
        {
            if (layout is null) throw new ArgumentNullException(nameof(layout));
            entityId = CreateEntity(entityId);
            ApplyLayout(entityId, layout);
            return entityId;
        }

        public void DestroyAllEntities()
        {
            ThrowIfStructuralChangeBlocked();

            for (int i = 0; i < _groups.Count; i++)
            {
                var group = _groups[i];

                // destroy entities backwards to avoid component patching
                while (group.TryGetLastEntityId(out var entityId))
                {
                    DestroyEntity(entityId);
                }
            }
        }

        public unsafe bool DestroyEntity(uint entityId)
        {
            ThrowIfStructuralChangeBlocked();
            if (!_entityMap.Remove(entityId, out var reference)) return false;
            Count--;
            if (reference.Group == null) return true;
            ClearEventExceptions();

            // publish remove events
            var currentChunk = reference.Group.GetChunk(reference.Index.Chunk);
            int i = 0;
            byte* destinationPtr;
            foreach (var id in reference.Group.Archetype.GetIds())
            {
                ref var componentType = ref ComponentRegistry.Get(id);
                if (!componentType.ZeroSize)
                {
                    var size = ComponentRegistry.Get(id).Size;
                    destinationPtr = currentChunk.GetComponent(reference.Group.ListOffsets[i++], size, reference.Index.List);
                }
                else destinationPtr = null;

                componentType.OnRemove(this, entityId, destinationPtr);
            }

            // remove from group
            var remappedEntityId = reference.Group.Remove(reference.Index);
            if (remappedEntityId != 0)
            {
                // remap a moved entity
                ref var remappedReference = ref CollectionsMarshal.GetValueRefOrAddDefault(_entityMap, remappedEntityId, out var exists);
                if (exists) remappedReference = new EntityReference(remappedReference.Group, reference.Index);
            }
            return true;
        }

        public void DisableEntity(uint entityId) => AddComponent<Disabled>(entityId);

        public void Dispose()
        {
            ThrowIfStructuralChangeBlocked();
            foreach (var group in _groups) group.Dispose();
            _groups.Clear();
            _archetypeIndexBuckets.Clear();
            _archetypeMap.Clear();
            _eventDispatcher.Clear();
        }

        public void EnableEntity(uint entityId) => RemoveComponent<Disabled>(entityId);

        public bool EntityExists(uint entityId) => _entityMap.ContainsKey(entityId);

        public EntityArchetype GetArchetype(uint entityId)
        {
            if (!_entityMap.TryGetValue(entityId, out var reference)) ThrowEntityNotFound();
            return reference.Group?.Archetype ?? default;
        }

        public ComponentType GetComponentType(int typeId) => ComponentRegistry.Get(typeId);
        public ComponentType GetComponentType<T>() where T : unmanaged => ComponentRegistry.Get<T>();

        public object GetComponent(uint entityId, int typeId)
        {
            if (!_entityMap.TryGetValue(entityId, out var reference)) ThrowEntityNotFound();
            ref var componentType = ref ComponentRegistry.Get(typeId);
            return componentType.Getter.Invoke(this, entityId);
        }

        public ref T GetComponent<T>(uint entityId) where T : unmanaged
        {
            if (!_entityMap.TryGetValue(entityId, out var reference)) ThrowEntityNotFound();
            return ref reference.GetComponent<T>();
        }

        public Entity GetEntity(uint entityId)
        {
            if (!_entityMap.TryGetValue(entityId, out var reference)) ThrowEntityNotFound();
            return reference.GetEntity();
        }

        public bool HasComponent<T>(uint entityId) where T : unmanaged
        {
            if (!_entityMap.TryGetValue(entityId, out var reference)) ThrowEntityNotFound();
            return reference.Group != null && reference.Group.Archetype.Contains(ComponentRegistry.Type<T>.Id);
        }

        public void RemoveAllEventHandlers()
        {
            ThrowIfStructuralChangeBlocked();
            _eventDispatcher.Clear();
        }

        public unsafe bool RemoveComponent<T>(uint entityId) where T : unmanaged
        {
            ref var component = ref ComponentRegistry.Get<T>();
            var archetype = GetArchetype(entityId);
            if (!archetype.Contains(component.Id)) return false;
            ApplyLayout(entityId, component.RemoveLayout);
            return true;
        }

        public void SetComponent(uint entityId, int typeId, object value)
        {
            if (!_entityMap.TryGetValue(entityId, out var reference)) ThrowEntityNotFound();
            ref var componentType = ref ComponentRegistry.Get(typeId);
            componentType.Setter.Invoke(this, entityId, value);
        }

        public ref T TryGetComponent<T>(uint entityId, out bool found) where T : unmanaged
        {
            if (!_entityMap.TryGetValue(entityId, out var reference)) ThrowEntityNotFound();
            return ref reference.TryGetComponent<T>(out found);
        }

        internal unsafe void Query<T>(T query, bool parallel, QueryFilter queryFilter) where T : IQuery
        {
            Interlocked.Increment(ref _iterators);
            parallel = parallel && ParallelEnabled && Interlocked.CompareExchange(ref _inParallel, 1, 0) == 0;
            try
            {
                foreach (var id in query.GetRequiredIds())
                {
                    // add required ids to with filter
                    QueryFilter.AddtoFilter(ref queryFilter.WithFilter, id);
                }

                // determine jobs
                var index = queryFilter.WithFilter.GetQueryIndex();
                var groupList = _archetypeIndexBuckets.TryGetValue(index, out var indexGroup) ? indexGroup : _groups;
                var jobList = RentJobList();
                foreach (var group in groupList)
                {
                    // check against filter
                    if (!queryFilter.Contains(in group.Archetype)) continue;
                    if (parallel)
                    {
                        var indicesIndex = MapParallelIndices(group);
                        var indices = _parallelIndices.AsSpan(indicesIndex * 6, 6);
                        query.CopyIndices(group, indices);
                    }

                    for (int i = 0; i < group.ChunkCount; i++)
                    {
                        jobList.Add(new(group, i));
                    }
                }

                // recycle query filter
                queryFilter.Return();

                // enumerate chunks
                if (!parallel)
                {
                    Span<int> indices = stackalloc int[6];
                    EntityGroup? indicesForGroup = null;
                    var jobSpan = CollectionsMarshal.AsSpan(jobList);
                    foreach (ref var job in jobSpan)
                    {
                        if (indicesForGroup != job.Group)
                        {
                            query.CopyIndices(job.Group, indices);
                            indicesForGroup = job.Group;
                        }
                        query.EnumerateChunk(in job, indices);
                    }
                }
                else
                {
                    Parallel.ForEach(jobList, ParallelOptions ?? s_defaultParallelOptions, (job) =>
                    {
                        var indicesIndex = _parallelIndicesMap[job.Group];
                        var indices = _parallelIndices.AsSpan(indicesIndex * 6, 6);
                        query.EnumerateChunk(in job, indices);
                    });
                    ClearParallel();
                }

                // cleanup
                ReturnJobList(jobList);
            }
            finally
            {
                Interlocked.Decrement(ref _iterators);
                if (parallel) _inParallel = 0;
            }
        }

        private void ClearEventExceptions()
        {
            _eventExceptions.Clear();
        }

        private void ClearParallel()
        {
            _parallelIndicesMap.Clear();
            _nextParallelIndex = 0;
        }

        private uint GenerateEntityId()
        {
            if (_entityMap.Count >= int.MaxValue) throw new Exception("Maximum entities reached");
            uint id;
            do
            {
                id = _nextEntityId++;
            }
            while (_entityMap.ContainsKey(id));
            return id;
        }

        private EntityGroup? GetOrCreateGroup(EntityArchetype destinationArchetype)
        {
            if (destinationArchetype.Depth == 0) return null;
            if (!_archetypeMap.TryGetValue(destinationArchetype, out var value))
            {
                var groupArchetype = ArchetypeCache.Rent(destinationArchetype.Depth);
                destinationArchetype.CopyTo(groupArchetype);

                value = new EntityGroup(groupArchetype);
                _archetypeMap.Add(groupArchetype, value);
                foreach (var index in value.Archetype.GetIndices())
                {
                    ref var indexBucket = ref CollectionsMarshal.GetValueRefOrAddDefault(_archetypeIndexBuckets, index, out var exists);
                    if (!exists || indexBucket == null) indexBucket = new();
                    indexBucket.Add(value);
                }
                _groups.Add(value);
            }
            return value;
        }

        private int MapParallelIndices(EntityGroup group)
        {
            // get next indices index
            var indicesIndex = _nextParallelIndex++;

            // expand indices array if needed
            if (indicesIndex * 6 >= _parallelIndices.Length)
            {
                var newIndices = new int[_parallelIndices.Length * 2];
                Array.Copy(_parallelIndices, newIndices, _parallelIndices.Length);
                _parallelIndices = newIndices;
            }

            _parallelIndicesMap[group] = indicesIndex;
            return indicesIndex;
        }

        private void PrePopulateCaches()
        {
            for (int i = 0; i < 8; i++) _jobListCache.Push(new List<EnumerationJob>());
        }

        private List<EnumerationJob> RentJobList()
        {
            lock (_jobListCache)
            {
                return _jobListCache.TryPop(out var jobList) ? jobList : new List<EnumerationJob>();
            }
        }

        private void ReturnJobList(List<EnumerationJob> jobList)
        {
            jobList.Clear();
            lock (_jobListCache)
            {
                _jobListCache.Push(jobList);
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private void ThrowIfStructuralChangeBlocked()
        {
            if (_iterators != 0) throw new Exception("Structural change not allowed during ForEach.");
            if (_inEvent) throw new Exception("Structural change not allowed during event handler.");
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static void ThrowEntityNotFound() => throw new Exception("No entity found at the given id.");
    }
}
