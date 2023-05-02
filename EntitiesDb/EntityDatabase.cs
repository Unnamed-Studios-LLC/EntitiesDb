using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using EntitiesDb.Components;
using EntitiesDb.Data;
using EntitiesDb.Events;
using EntitiesDb.Mapping;
using EntitiesDb.Queries;

namespace EntitiesDb
{
    public sealed partial class EntityDatabase : IDisposable
    {
        private const int DefaultParallelIndicesSize = 32;

        private static readonly ParallelOptions s_defaultParallelOptions = new();

        private readonly List<EntityGroup> _groups = new();
        private readonly Dictionary<Archetype, EntityGroup> _archetypeMap = new();
        private readonly Dictionary<long, List<EntityGroup>> _archetypeIndexBuckets = new();
        private readonly Dictionary<uint, EntityReference> _entityMap = new();
        private readonly List<Stack<Archetype>> _archetypeCache = new();
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
        private QueryFilter _queryFilter;

        public int Count { get; private set; }
        public IReadOnlyList<Exception> EventExceptions => _eventExceptions;
        public bool ParallelEnabled { get; set; } = true;
        public ParallelOptions ParallelOptions { get; set; } = new();

        public EntityDatabase()
        {
            ResetQueryFilter();
            PrePopulateCaches();
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
            var destinationArchetype = RentArchetype(depthResult);
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
                        ref var componentType = ref ComponentRegistry.GetType(id);
                        if (!componentType.ZeroSize)
                        {
                            while (reference.Group.ComponentIds[i] < id) i++;
                            if (reference.Group.ComponentIds[i] > id) break; // this type is not contained in the current archetype

                            var size = ComponentRegistry.GetType(id).Size;
                            destinationPtr = currentChunk.GetComponent(reference.Group.ListOffsets[i], size, reference.Index.List);
                        }
                        else destinationPtr = null;

                        try
                        {
                            componentType.OnRemove(_eventDispatcher, entityId, destinationPtr);
                        }
                        catch (Exception e)
                        {
                            _eventExceptions.Add(e);
                        }
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
                    var hasDataArchetype = new Archetype(layout.HasDataMask);
                    foreach (var id in layout.AddArchetype.GetIds())
                    {
                        ref var componentType = ref ComponentRegistry.GetType(id);
                        if (!componentType.ZeroSize)
                        {
                            while (reference.Group.ComponentIds[i] < id) i++;
                            var size = ComponentRegistry.GetType(id).Size;
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

                    ref var componentType = ref ComponentRegistry.GetType(id);
                    if (!componentType.ZeroSize)
                    {
                        while (reference.Group.ComponentIds[i] < id) i++;
                        var size = ComponentRegistry.GetType(id).Size;
                        destinationPtr = destinationChunk.GetComponent(reference.Group.ListOffsets[i], size, reference.Index.List);
                    }
                    else destinationPtr = null;

                    try
                    {
                        componentType.OnAdd(_eventDispatcher, entityId, destinationPtr);
                    }
                    catch (Exception e)
                    {
                        _eventExceptions.Add(e);
                    }
                }
            }

            // cleanup
            ReturnArchetype(destinationArchetype);
        }

        public unsafe uint Clone(uint entityId)
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
                    ref var componentType = ref ComponentRegistry.GetType(id);
                    if (!componentType.ZeroSize)
                    {
                        while (destination.Group.ComponentIds[i] < id) i++;
                        var size = ComponentRegistry.GetType(id).Size;
                        destinationPtr = destinationChunk.GetComponent(destination.Group.ListOffsets[i], size, destination.Index.List);
                    }
                    else destinationPtr = null;

                    try
                    {
                        componentType.OnAdd(_eventDispatcher, entityId, destinationPtr);
                    }
                    catch (Exception e)
                    {
                        _eventExceptions.Add(e);
                    }
                }
            }
            return newEntityId;
        }

        public uint CreateEntity()
        {
            ThrowIfStructuralChangeBlocked();
            var entityId = GenerateEntityId();
            _entityMap.Add(entityId, default);
            Count++;
            return entityId;
        }

        public uint CreateEntity(EntityLayout layout)
        {
            if (layout is null) throw new ArgumentNullException(nameof(layout));
            var entityId = CreateEntity();
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
                ref var componentType = ref ComponentRegistry.GetType(id);
                if (!componentType.ZeroSize)
                {
                    var size = ComponentRegistry.GetType(id).Size;
                    destinationPtr = currentChunk.GetComponent(reference.Group.ListOffsets[i++], size, reference.Index.List);
                }
                else destinationPtr = null;

                try
                {
                    componentType.OnRemove(_eventDispatcher, entityId, destinationPtr);
                }
                catch (Exception e)
                {
                    _eventExceptions.Add(e);
                }
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

        public void Dispose()
        {
            ThrowIfStructuralChangeBlocked();
            foreach (var group in _groups) group.Dispose();
            _groups.Clear();
            _archetypeIndexBuckets.Clear();
            _archetypeMap.Clear();
            _eventDispatcher.Clear();
        }

        public bool EntityExists(uint entityId) => _entityMap.ContainsKey(entityId);

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

        public void RemoveAllEventHandlers()
        {
            ThrowIfStructuralChangeBlocked();
            _eventDispatcher.Clear();
        }

        public ref T TryGetComponent<T>(uint entityId, out bool found) where T : unmanaged
        {
            if (!_entityMap.TryGetValue(entityId, out var reference)) ThrowEntityNotFound();
            return ref reference.TryGetComponent<T>(out found);
        }

        internal void PublishAddEvent<T>(uint entityId, ref T component) where T : unmanaged
        {
            _inEvent = true;
            _eventDispatcher.PublishAdd(entityId, ref component);
            _inEvent = false;
        }

        internal void PublishRemoveEvent<T>(uint entityId, ref T component) where T : unmanaged
        {
            _inEvent = true;
            _eventDispatcher.PublishRemove(entityId, ref component);
            _inEvent = false;
        }

        private void AddtoFilter(ref Archetype archetype, int typeId)
        {
            var depth = typeId / 64;
            if (depth + 1 > archetype.Depth)
            {
                var newArchetype = RentArchetype(depth + 1);
                archetype.CopyTo(newArchetype);
                ReturnArchetype(archetype);
                archetype = newArchetype;
            }

            archetype[depth] |= 1ul << (typeId % 64);
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

        private EntityGroup? GetOrCreateGroup(Archetype destinationArchetype)
        {
            if (destinationArchetype.Depth == 0) return null;
            ref var value = ref CollectionsMarshal.GetValueRefOrAddDefault(_archetypeMap, destinationArchetype, out var exists);
            if (!exists || value == null)
            {
                var groupArchetype = RentArchetype(destinationArchetype.Depth);
                destinationArchetype.CopyTo(groupArchetype);

                value = new EntityGroup(groupArchetype);
                foreach (var index in value.Archetype.GetIndices())
                {
                    ref var indexBucket = ref CollectionsMarshal.GetValueRefOrAddDefault(_archetypeIndexBuckets, index, out exists);
                    if (!exists || indexBucket == null) indexBucket = new();
                    indexBucket.Add(value);
                }
                _groups.Add(value);
            }
            return value;
        }

        private void MapParallelIndices(EntityGroup group)
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
        }

        private void PrePopulateCaches()
        {
            for (int i = 0; i < 8; i++) _jobListCache.Push(new List<EnumerationJob>());
            for (int i = 1; i <= 4; i++) _archetypeCache.Add(new Stack<Archetype>());
        }

        private unsafe void Query<T>(T query, bool parallel) where T : IQuery
        {
            Interlocked.Increment(ref _iterators);
            parallel = parallel && ParallelEnabled && Interlocked.CompareExchange(ref _inParallel, 1, 0) == 0;
            try
            {
                foreach (var id in query.GetRequiredIds())
                {
                    // add required ids to with filter
                    AddtoFilter(ref _queryFilter.With, id);
                }

                // determine jobs
                var queryFilter = TakeQueryFilter();
                var index = queryFilter.With.GetQueryIndex();
                var groupList = _archetypeIndexBuckets.TryGetValue(index, out var indexGroup) ? indexGroup : _groups;
                var jobList = RentJobList();
                foreach (var group in groupList)
                {
                    // check against filter
                    if (!queryFilter.Contains(in group.Archetype)) continue;
                    if (parallel) MapParallelIndices(group);

                    for (int i = 0; i < group.ChunkCount; i++)
                    {
                        jobList.Add(new(group, i));
                    }
                }

                // recycle query filter
                RecycleQueryFilter(queryFilter);

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

        private void RecycleQueryFilter(QueryFilter queryFilter)
        {
            ReturnArchetype(queryFilter.Any);
            ReturnArchetype(queryFilter.No);
            ReturnArchetype(queryFilter.With);
        }

        private void RemoveFromFilter(ref Archetype archetype, int typeId)
        {
            var depth = typeId / 64;
            if (depth + 1 > archetype.Depth) return;
            archetype[depth] &= ~(1ul << (typeId % 64));

            if (depth + 1 == archetype.Depth &&
                archetype[depth] == 0)
            {
                var newArchetype = RentArchetype(depth);
                archetype.CopyTo(newArchetype);
                ReturnArchetype(archetype);
                archetype = newArchetype;
            }
        }

        private Archetype RentArchetype(int depth)
        {
            if (depth == 0) return default;
            while (depth >= _archetypeCache.Count) _archetypeCache.Add(new Stack<Archetype>());
            var cache = _archetypeCache[depth];
            if (!cache.TryPop(out var result)) result = new(new ulong[depth]);
            return result;
        }

        private List<EnumerationJob> RentJobList()
        {
            lock (_jobListCache)
            {
                return _jobListCache.TryPop(out var jobList) ? jobList : new List<EnumerationJob>();
            }
        }

        private void ResetQueryFilter()
        {
            _queryFilter = default;
            AddtoFilter(ref _queryFilter.No, ComponentRegistry.Type<Disabled>.Id);
        }

        private Archetype ReturnArchetype(Archetype archetype)
        {
            if (archetype.Depth != 0)
            {
                while (archetype.Depth >= _archetypeCache.Count) _archetypeCache.Add(new Stack<Archetype>());
                var cache = _archetypeCache[archetype.Depth];
                archetype.Clear();
                cache.Push(archetype);
            }
            return default;
        }

        private void ReturnJobList(List<EnumerationJob> jobList)
        {
            jobList.Clear();
            lock (_jobListCache)
            {
                _jobListCache.Push(jobList);
            }
        }

        private QueryFilter TakeQueryFilter()
        {
            var queryFilter = _queryFilter;
            ResetQueryFilter();
            return queryFilter;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private void ThrowIfStructuralChangeBlocked()
        {
            if (_iterators != 0) throw new Exception("Entity structural change not allowed during ForEach.");
            if (_inEvent) throw new Exception("Entity structural change not allowed during event handler.");
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static void ThrowEntityNotFound() => throw new Exception("No entity found at the given id.");
    }
}
