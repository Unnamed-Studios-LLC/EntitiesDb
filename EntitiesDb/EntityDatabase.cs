using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace EntitiesDb
{
    public sealed partial class EntityDatabase
    {
        /// <summary>
        /// The maximum amount of entities that can be stored in one <see cref="EntityDatabase"/>
        /// </summary>
        public const int MaxEntities = int.MaxValue;

        private readonly Dictionary<long, List<Archetype>> _indexedArchetypes = new();
        private readonly Dictionary<int, List<Archetype>> _mappedArchetypes = new();
        private readonly Dictionary<uint, EntityReference> _entityReferences = new();
        private readonly List<Archetype> _archetypes = new();
        private readonly List<Type> _componentTypes = new();
        private readonly Dictionary<Type, int> _componentTypeMap = new();
        private readonly Stack<List<EnumerationJob>> _jobListCache = new();
        private readonly Stack<HashSet<Type>> _queryFilterCache = new();
        private readonly EventDispatcher _eventDispatcher = new();

        private int[] _workingIds = new int[256];
        private ulong[] _workingMasks = new ulong[1];
        private long[] _workingIndices = new long[256];
        private Type[] _workingTypes = new Type[256];
        private uint _entityIds = 0;
        private int _enumerators = 0;
        private int _inParallel = 0;
        private bool _inEvent = false;

        /// <summary>
        /// The amount of entities currently stored
        /// </summary>
        public int Count => _entityReferences.Count;

        /// <summary>
        /// If entities and components can be added or removed
        /// </summary>
        public bool ReadOnly => _enumerators != 0 || _inEvent;

        /// <summary>
        /// Adds or replaces a component
        /// </summary>
        /// <typeparam name="T">The component type</typeparam>
        /// <param name="entityId">Id of the entity altered</param>
        /// <param name="component">Component data</param>
        /// <exception cref="ReadOnlyException"></exception>
        /// <exception cref="EntityNotFoundException"></exception>
        /// <exception cref="EventException"></exception>
        public void AddComponent<T>(uint entityId, T component = default) where T : unmanaged
        {
            if (ReadOnly)
            {
                throw new ReadOnlyException();
            }

            if (!_entityReferences.TryGetValue(entityId, out var entityReference))
            {
                throw new EntityNotFoundException(entityId);
            }

            // retrieve metadata first to ensure static contructors are run for this type
            var metaData = ComponentMetaData<T>.Instance;

            // resolve archetype
            var added = !entityReference.Archetype.ContainsType(typeof(T));
            if (added)
            {
                // archetype changed
                // move entity to new archetype
                var destinationMask = GetDestinationMaskAdd(entityReference.Archetype, typeof(T));
                var newArchetype = GetArchetype(destinationMask);
                entityReference = MoveEntity(entityId, entityReference, newArchetype);
            }

            // set component data
            ref var addedComponent = ref ComponentMetaData<T>.Empty;
            if (metaData.Size != 0)
            {
                // only set if non-zero
                var chunk = entityReference.Archetype.GetChunk(entityReference.Indices.ChunkIndex);
                var listOffset = entityReference.Archetype.GetListOffset(typeof(T));
                addedComponent = ref chunk.GetComponent<T>(listOffset, entityReference.Indices.ListIndex);
                addedComponent = component;
            }

            // add event
            if (added)
            {
                PublishAddEvent(entityId, ref addedComponent);
            }
        }

        /// <summary>
        /// Adds and removes components specified in the given <see cref="EntityLayout"/>
        /// </summary>
        /// <param name="entityId">Id of the entity altered</param>
        /// <param name="entityLayout"></param>
        /// <exception cref="ReadOnlyException"></exception>
        /// <exception cref="EntityNotFoundException"></exception>
        /// <exception cref="EventException"></exception>
        public void ApplyLayout(uint entityId, EntityLayout entityLayout)
        {
            if (ReadOnly)
            {
                throw new ReadOnlyException();
            }

            if (!_entityReferences.TryGetValue(entityId, out var entityReference))
            {
                throw new EntityNotFoundException(entityId);
            }

            // resolve archetype
            var destinationMask = GetDestinationMask(entityReference.Archetype, entityLayout);
            var newArchetype = GetArchetype(destinationMask);
            var archetypeChanged = newArchetype != entityReference.Archetype;
            var newEntityReference = entityReference;
            if (archetypeChanged)
            {
                // archetype changed
                // remove events
                PublishRemoveEvents(entityId, entityReference, newArchetype);

                // move entity to new archetype
                newEntityReference = MoveEntity(entityId, entityReference, newArchetype);
            }

            // set component data
            SetComponentData(newEntityReference, entityLayout);

            // add events
            if (archetypeChanged)
            {
                PublishAddEvents(entityId, newEntityReference, entityReference.Archetype);
            }
        }

        /// <summary>
        /// Resets <see cref="EntityDatabase"/> to its initial state and may be reused
        /// </summary>
        /// <exception cref="ReadOnlyException"></exception>
        public void Clear()
        {
            if (ReadOnly)
            {
                throw new ReadOnlyException();
            }

            _indexedArchetypes.Clear();
            _mappedArchetypes.Clear();
            _entityReferences.Clear();
            _componentTypes.Clear();
            _componentTypeMap.Clear();
            _jobListCache.Clear();
            _queryFilterCache.Clear();
            _queryFilterCache.Clear();
            _eventDispatcher.Clear();

            _workingIds = new int[256];
            _workingIndices = new long[256];
            _workingMasks = new ulong[1];
            _workingTypes = new Type[256];
            _entityIds = 0;

            foreach (var archetype in _archetypes)
            {
                archetype.Dispose();
            }
            _archetypes.Clear();
        }

        /// <summary>
        /// Clones an existing entity and its components
        /// </summary>
        /// <param name="entityId">Id of the entity to clone</param>
        /// <returns>Id of the cloned entity</returns>
        /// <exception cref="ReadOnlyException"></exception>
        /// <exception cref="EntityNotFoundException"></exception>
        /// <exception cref="EntityMaxException"></exception>
        /// <exception cref="EventException"></exception>
        public uint CloneEntity(uint entityId)
        {
            if (ReadOnly)
            {
                throw new ReadOnlyException();
            }

            if (!_entityReferences.TryGetValue(entityId, out var entityReference))
            {
                throw new EntityNotFoundException(entityId);
            }

            if (_entityReferences.Count >= MaxEntities)
            {
                throw new EntityMaxException(MaxEntities);
            }

            var clonedEntityId = GetAvailableEntityId();
            var clonedEntityIndex = entityReference.Archetype.Add(clonedEntityId);
            var clonedEntityReference = new EntityReference(entityReference.Archetype, clonedEntityIndex);
            CopyComponents(in entityReference, in clonedEntityReference);
            _entityReferences.Add(clonedEntityId, clonedEntityReference);

            // add entity event
            PublishAddEntity(clonedEntityId);

            // add component events
            PublishAddEvents(clonedEntityId, clonedEntityReference, null);

            return clonedEntityId;
        }

        /// <summary>
        /// Creates a new empty entity
        /// </summary>
        /// <returns>Id of the created entity</returns>
        /// <exception cref="ReadOnlyException"></exception>
        /// <exception cref="EntityMaxException"></exception>
        /// <exception cref="EventException"></exception>
        public uint CreateEntity()
        {
            if (ReadOnly)
            {
                throw new ReadOnlyException();
            }

            if (_entityReferences.Count >= MaxEntities)
            {
                throw new EntityMaxException(MaxEntities);
            }

            var entityId = GetAvailableEntityId();
            CreateEntityInternal(entityId, Span<ulong>.Empty);

            // add entity event
            PublishAddEntity(entityId);

            return entityId;
        }

        /// <summary>
        /// Creates a new entity with a given <see cref="EntityLayout"/>
        /// </summary>
        /// <param name="entityLayout">The layout to apply to the new entity</param>
        /// <returns>Id of the created entity</returns>
        /// <exception cref="ReadOnlyException"></exception>
        /// <exception cref="EntityMaxException"></exception>
        /// <exception cref="EventException"></exception>
        public uint CreateEntity(EntityLayout entityLayout)
        {
            if (ReadOnly)
            {
                throw new ReadOnlyException();
            }

            if (_entityReferences.Count >= MaxEntities)
            {
                throw new EntityMaxException(MaxEntities);
            }

            // create entity
            var entityId = GetAvailableEntityId();
            var destinationMasks = GetDestinationMask(entityLayout);
            var entityReference = CreateEntityInternal(entityId, destinationMasks);

            // set component data
            SetComponentData(entityReference, entityLayout);

            // add entity event
            PublishAddEntity(entityId);

            // add component events
            PublishAddEvents(entityId, entityReference, null);

            return entityId;
        }

        /// <summary>
        /// Creates a new empty entity with a given id
        /// </summary>
        /// <param name="entityId">Id to assign to the new entity</param>
        /// <returns>Id of the created entity</returns>
        /// <exception cref="ReadOnlyException"></exception>
        /// <exception cref="EntityConflictException"></exception>
        /// <exception cref="EventException"></exception>
        public uint CreateEntity(uint entityId)
        {
            if (ReadOnly)
            {
                throw new ReadOnlyException();
            }

            if (_entityReferences.ContainsKey(entityId))
            {
                throw new EntityConflictException(entityId);
            }

            CreateEntityInternal(entityId, Span<ulong>.Empty);

            // add entity event
            PublishAddEntity(entityId);

            return entityId;
        }

        /// <summary>
        /// Creates a new entity with a given <see cref="EntityLayout"/> and a given id
        /// </summary>
        /// <param name="entityId">Id to assign to the new entity</param>
        /// <param name="entityLayout">The layout to apply to the new entity</param>
        /// <returns>Id of the created entity</returns>
        /// <exception cref="ReadOnlyException"></exception>
        /// <exception cref="EntityConflictException"></exception>
        /// <exception cref="EventException"></exception>
        public uint CreateEntity(uint entityId, EntityLayout entityLayout)
        {
            if (ReadOnly)
            {
                throw new ReadOnlyException();
            }

            if (_entityReferences.ContainsKey(entityId))
            {
                throw new EntityConflictException(entityId);
            }

            var destinationMasks = GetDestinationMask(entityLayout);
            var entityReference = CreateEntityInternal(entityId, destinationMasks);

            // set component data
            SetComponentData(entityReference, entityLayout);

            // add entity event
            PublishAddEntity(entityId);

            // add component events
            PublishAddEvents(entityId, entityReference, null);

            return entityId;
        }

        /// <summary>
        /// Destroys all entities
        /// </summary>
        /// <exception cref="ReadOnlyException"></exception>
        /// <exception cref="EventException"></exception>
        public void DestroyAllEntities()
        {
            if (ReadOnly)
            {
                throw new ReadOnlyException();
            }

            // destroy all entities from end to start of chunks (to avoid patching)
            foreach (var archetype in _archetypes)
            {
                while (archetype.ChunkCount > 0)
                {
                    DestroyEntity(archetype.GetLastEntityId());
                }
            }
        }

        /// <summary>
        /// Destroys a given entity
        /// </summary>
        /// <param name="entityId">Id of the entity to destroy</param>
        /// <exception cref="ReadOnlyException"></exception>
        /// <exception cref="EntityNotFoundException"></exception>
        /// <exception cref="EventException"></exception>
        public void DestroyEntity(uint entityId)
        {
            if (ReadOnly)
            {
                throw new ReadOnlyException();
            }

            if (!_entityReferences.TryGetValue(entityId, out var entityReference))
            {
                throw new EntityNotFoundException(entityId);
            }

            // remove entity event
            PublishRemoveEntity(entityId);

            // remove component events
            PublishRemoveEvents(entityId, entityReference, null);

            var remappedEntityId = entityReference.Archetype.Remove(entityReference.Indices);
            if (remappedEntityId != 0) _entityReferences[remappedEntityId] = entityReference;
            _entityReferences.Remove(entityId);
        }

        /// <summary>
        /// Returns if an entity exists at a given id
        /// </summary>
        /// <param name="entityId">Id to check</param>
        /// <returns>If an entity exists at the given id</returns>
        public bool EntityExists(uint entityId) => _entityReferences.ContainsKey(entityId);

        /// <summary>
        /// Returns a reference to a component for an entity.
        /// Ref values may be invalid after structural changes and should not be stored.
        /// </summary>
        /// <typeparam name="T">Component type</typeparam>
        /// <param name="entityId">Id of the entity</param>
        /// <returns>Reference to the component for the given entity</returns>
        /// <exception cref="EntityNotFoundException"></exception>
        /// <exception cref="ComponentNotFoundException"></exception>
        public ref T GetComponent<T>(uint entityId) where T : unmanaged
        {
            if (!_entityReferences.TryGetValue(entityId, out var entityReference))
            {
                throw new EntityNotFoundException(entityId);
            }

            if (!entityReference.Archetype.TryGetListOffset(typeof(T), out var listOffset))
            {
                throw new ComponentNotFoundException(entityId, typeof(T));
            }

            var chunk = entityReference.Archetype.GetChunk(entityReference.Indices.ChunkIndex);
            return ref chunk.GetComponent<T>(listOffset, entityReference.Indices.ListIndex);
        }

        /// <summary>
        /// Gets the types for components on an entity
        /// </summary>
        /// <param name="entityId">Id of the entity</param>
        /// <returns>Types for components on the given entity</returns>
        /// <exception cref="EntityNotFoundException"></exception>
        public IEnumerable<Type> GetComponentTypes(uint entityId)
        {
            if (!_entityReferences.TryGetValue(entityId, out var entityReference))
            {
                throw new EntityNotFoundException(entityId);
            }

            return entityReference.Archetype.Types;
        }

        /// <summary>
        /// Returns if an entity has a given component type
        /// </summary>
        /// <typeparam name="T">Component type</typeparam>
        /// <param name="entityId">Id of the entity</param>
        /// <returns>If the entity has the given component type</returns>
        /// <exception cref="EntityNotFoundException"></exception>
        public bool HasComponent<T>(uint entityId) where T : unmanaged
        {
            if (!_entityReferences.TryGetValue(entityId, out var entityReference))
            {
                throw new EntityNotFoundException(entityId);
            }

            return entityReference.Archetype.ContainsType(typeof(T));
        }

        /// <summary>
        /// Returns if an entity has a given component type
        /// </summary>
        /// <typeparam name="T">Component type</typeparam>
        /// <param name="entityId">Id of the entity</param>
        /// <param name="type">The type to check</param>
        /// <returns>If the entity has the given component type</returns>
        /// <exception cref="EntityNotFoundException"></exception>
        public bool HasComponent(uint entityId, Type type)
        {
            if (!_entityReferences.TryGetValue(entityId, out var entityReference))
            {
                throw new EntityNotFoundException(entityId);
            }

            return entityReference.Archetype.ContainsType(type);
        }

        /// <summary>
        /// Removes a component for a given entity if the entity contains the component
        /// </summary>
        /// <typeparam name="T">Component type</typeparam>
        /// <param name="entityId">Id of the entity</param>
        /// <returns>If the component was found and removed</returns>
        /// <exception cref="ReadOnlyException"></exception>
        /// <exception cref="EntityNotFoundException"></exception>
        public bool RemoveComponent<T>(uint entityId) where T : unmanaged
        {
            if (ReadOnly)
            {
                throw new ReadOnlyException();
            }

            if (!_entityReferences.TryGetValue(entityId, out var entityReference))
            {
                throw new EntityNotFoundException(entityId);
            }

            if (!entityReference.Archetype.TryGetListOffset(typeof(T), out var listOffset))
            {
                return false;
            }

            // remove event
            ref var removedComponent = ref ComponentMetaData<T>.Empty;
            if (listOffset >= 0)
            {
                removedComponent = ref entityReference.GetChunk().GetComponent<T>(listOffset, entityReference.Indices.ListIndex);
            }
            PublishRemoveEvent(entityId, ref removedComponent);

            // move entity to new archetype
            var destinationMask = GetDestinationMaskRemove(entityReference.Archetype, typeof(T));
            var newArchetype = GetArchetype(destinationMask);
            MoveEntity(entityId, entityReference, newArchetype);
            return true;
        }

        /// <summary>
        /// Trys to get a component for a given entity.
        /// If the component is not found, the returned ref is invalid and should not be used.
        /// </summary>
        /// <typeparam name="T">Component type</typeparam>
        /// <param name="entityId">Id of the entity</param>
        /// <param name="found">If the component was found</param>
        /// <returns>Reference to the component</returns>
        /// <exception cref="EntityNotFoundException"></exception>
        public ref T TryGetComponent<T>(uint entityId, out bool found) where T : unmanaged
        {
            if (!_entityReferences.TryGetValue(entityId, out var entityReference))
            {
                throw new EntityNotFoundException(entityId);
            }

            if (!entityReference.Archetype.TryGetListOffset(typeof(T), out var listOffset))
            {
                found = false;
                return ref ComponentMetaData<T>.Empty;
            }

            found = true;
            var chunk = entityReference.Archetype.GetChunk(entityReference.Indices.ChunkIndex);
            return ref chunk.GetComponent<T>(listOffset, entityReference.Indices.ListIndex);
        }

        internal void Query<TQuery>(TQuery query, bool parallel, in QueryFilter filter) where TQuery : IQuery
        {
            Interlocked.Increment(ref _enumerators);
            parallel = parallel && Interlocked.CompareExchange(ref _inParallel, 1, 0) == 0;
            try
            {
                foreach (var metaData in query.GetDelegateMetaData())
                {
                    // add required ids to with filter
                    if (metaData.Size == 0) throw new ForEachTagException(metaData.Type);
                    filter.WithTypes.Add(metaData.Type);
                }

                // determine jobs
                Span<ulong> with = stackalloc ulong[_workingMasks.Length];
                Span<ulong> no = stackalloc ulong[_workingMasks.Length];
                Span<ulong> any = stackalloc ulong[_workingMasks.Length];

                with = GetDestinationMask(filter.WithTypes, with);
                no = GetDestinationMask(filter.NoTypes, no);
                any = GetDestinationMask(filter.AnyTypes, any);

                // return filters
                ReturnQueryFilter(in filter);

                var index = Mask.GetQueryIndex(with, _workingIds);
                var archetypeList = _indexedArchetypes.TryGetValue(index, out var indexGroup) ? indexGroup : _archetypes;
                var jobList = RentJobList();
                foreach (var archetype in archetypeList)
                {
                    // check against filter
                    if (!Mask.Match(archetype.Mask, with, no, any)) continue;
                    for (int i = 0; i < archetype.ChunkCount; i++)
                    {
                        jobList.Add(new(archetype, i));
                    }
                }

                // enumerate chunks
                if (!parallel)
                {
                    foreach (var job in jobList)
                    {
                        query.EnumerateChunk(in job);
                    }
                }
                else
                {
                    Parallel.ForEach(jobList, job =>
                    {
                        query.EnumerateChunk(in job);
                    });
                }

                // return job list
                ReturnJobList(jobList);
            }
            finally
            {
                Interlocked.Decrement(ref _enumerators);
                if (parallel) _inParallel = 0;
            }
        }

        private void CopyComponents(in EntityReference source, in EntityReference destination)
        {
            var sourceChunk = source.Archetype.GetChunk(source.Indices.ChunkIndex);
            var destinationChunk = destination.Archetype.GetChunk(destination.Indices.ChunkIndex);

            foreach (var metaData in source.Archetype.MetaData)
            {
                if (metaData.Size == 0 || // ignore zero-size
                    !source.Archetype.TryGetListOffset(metaData.Type, out var sourceOffset) ||
                    !destination.Archetype.TryGetListOffset(metaData.Type, out var destinationOffset))
                {
                    continue;
                }

                Chunk.Copy(
                    sourceChunk, sourceOffset,
                    destinationChunk, destinationOffset,
                    metaData.Size
                );
            }
        }

        private EntityReference CreateEntityInternal(uint entityId, Span<ulong> masks)
        {
            var archetype = GetArchetype(masks);
            var index = archetype.Add(entityId);
            var reference = new EntityReference(archetype, index);
            _entityReferences.Add(entityId, reference);
            return reference;
        }

        private Archetype GetArchetype(Span<ulong> masks)
        {
            var hashCode = Mask.GetHashCode(masks);
            if (!_mappedArchetypes.TryGetValue(hashCode, out var bucket))
            {
                bucket = new();
                _mappedArchetypes.Add(hashCode, bucket);
            }

            Archetype archetype = null;
            foreach (var item in bucket)
            {
                if (!masks.SequenceEqual(item.Mask)) continue;
                archetype = item;
                break;
            }

            if (archetype == null)
            {
                var typeCount = GetTypes(masks, ref _workingTypes);
                archetype = new Archetype(masks.ToArray(), _workingTypes.AsSpan(0, typeCount));
                bucket.Add(archetype); // add to mapped archetypes
                _archetypes.Add(archetype);

                // add to indexed archetypes
                var indexCount = Mask.GetIndices(masks, _workingIds, ref _workingIndices);
                var indices = _workingIndices.AsSpan(0, indexCount);
                foreach (ref var index in indices)
                {
                    if (!_indexedArchetypes.TryGetValue(index, out var indexBucket))
                    {
                        indexBucket = new();
                        _indexedArchetypes.Add(index, indexBucket);
                    }
                    indexBucket.Add(archetype);
                }
            }
            return archetype;
        }

        private uint GetAvailableEntityId()
        {
            while (++_entityIds == 0 || _entityReferences.ContainsKey(_entityIds)) { }
            return _entityIds;
        }

        private Span<ulong> GetDestinationMask(IEnumerable<Type> types, Span<ulong> masks)
        {
            masks.Clear();
            foreach (var type in types)
            {
                var typeId = GetTypeId(type);
                Mask.IdAdd(masks, typeId, masks);
            }
            return Mask.Trim(masks);
        }

        private Span<ulong> GetDestinationMask(EntityLayout entityLayout)
        {
            Array.Clear(_workingMasks, 0, _workingMasks.Length);
            foreach (var pair in entityLayout.AddedComponents)
            {
                var typeId = GetTypeId(pair.Key);
                Mask.IdAdd(_workingMasks, typeId, _workingMasks);
            }
            return Mask.Trim(_workingMasks);
        }

        private Span<ulong> GetDestinationMask(Archetype currentArchetype, EntityLayout entityLayout)
        {
            Array.Clear(_workingMasks, 0, _workingMasks.Length);
            currentArchetype.Mask.CopyTo(_workingMasks.AsSpan());

            foreach (var pair in entityLayout.AddedComponents)
            {
                var typeId = GetTypeId(pair.Key);
                Mask.IdAdd(_workingMasks, typeId, _workingMasks);
            }

            foreach (var type in entityLayout.RemovedComponents)
            {
                var typeId = GetTypeId(type);
                Mask.IdRemove(_workingMasks, typeId, _workingMasks);
            }

            return Mask.Trim(_workingMasks);
        }

        private Span<ulong> GetDestinationMaskAdd(Archetype currentArchetype, Type type)
        {
            var typeId = GetTypeId(type);
            Array.Clear(_workingMasks, 0, _workingMasks.Length);
            var masks = _workingMasks.AsSpan();
            currentArchetype.Mask.CopyTo(masks);
            Mask.IdAdd(masks, typeId, masks);
            return Mask.Trim(masks);
        }

        private Span<ulong> GetDestinationMaskRemove(Archetype currentArchetype, Type type)
        {
            var typeId = GetTypeId(type);
            Array.Clear(_workingMasks, 0, _workingMasks.Length);
            var masks = _workingMasks.AsSpan();
            currentArchetype.Mask.CopyTo(masks);
            Mask.IdRemove(masks, typeId, masks);
            return Mask.Trim(masks);
        }

        private int GetTypeId(Type type)
        {
            if (_componentTypeMap.TryGetValue(type, out var id)) return id;
            id = _componentTypes.Count;
            _componentTypes.Add(type);
            _componentTypeMap.Add(type, id);
            while (id > _workingIds.Length)
            {
                Array.Resize(ref _workingIds, _workingIds.Length * 2);
            }

            while (id / 64 > _workingMasks.Length)
            {
                Array.Resize(ref _workingMasks, _workingMasks.Length + 1);
            }
            return id;
        }

        private int GetTypes(Span<ulong> masks, ref Type[] results)
        {
            var count = Mask.GetIds(masks, _workingIds);
            while (results.Length < count)
            {
                Array.Resize(ref results, results.Length * 2);
            }

            for (int i = 0; i < count; i++)
            {
                results[i] = _componentTypes[_workingIds[i]];
            }
            return count;
        }

        private EntityReference MoveEntity(uint entityId, EntityReference entityReference, Archetype newArchetype)
        {
            // copy to new archetype
            var newEntityIndex = newArchetype.Add(entityId);
            var newEntityReference = new EntityReference(newArchetype, newEntityIndex);
            CopyComponents(in entityReference, in newEntityReference);
            _entityReferences[entityId] = newEntityReference;

            // remove from old archetype
            var remappedEntityId = entityReference.Archetype.Remove(entityReference.Indices);
            if (remappedEntityId != 0) _entityReferences[remappedEntityId] = entityReference;
            return newEntityReference;
        }

        private void PublishAddEntity(uint entityId)
        {
            try
            {
                _inEvent = true;
                _eventDispatcher.OnAddEntity(entityId);
            }
            catch (Exception e)
            {
                throw new EventException(e);
            }
            finally
            {
                _inEvent = false;
            }
        }

        private void PublishAddEvent<T>(uint entityId, ref T component) where T : unmanaged
        {
            try
            {
                _inEvent = true;
                _eventDispatcher.OnAddComponent(entityId, ref component);
            }
            catch (Exception e)
            {
                throw new EventException(e);
            }
            finally
            {
                _inEvent = false;
            }
        }

        private void PublishAddEvents(uint entityId, EntityReference entityReference, Archetype previousArchetype)
        {
            var chunk = entityReference.GetChunk();
            foreach (var metaData in entityReference.Archetype.MetaData)
            {
                if (previousArchetype?.ContainsType(metaData.Type) ?? false) continue; // this type was not added
                var listOffset = entityReference.Archetype.GetListOffset(metaData.Type);
                try
                {
                    _inEvent = true;
                    metaData.OnAddComponent(_eventDispatcher, entityId, chunk, listOffset, entityReference.Indices.ListIndex);
                }
                catch (Exception e)
                {
                    throw new EventException(e);
                }
                finally
                {
                    _inEvent = false;
                }
            }
        }

        private void PublishRemoveEntity(uint entityId)
        {
            try
            {
                _inEvent = true;
                _eventDispatcher.OnRemoveEntity(entityId);
            }
            catch (Exception e)
            {
                throw new EventException(e);
            }
            finally
            {
                _inEvent = false;
            }
        }

        private void PublishRemoveEvent<T>(uint entityId, ref T component) where T : unmanaged
        {
            try
            {
                _inEvent = true;
                _eventDispatcher.OnRemoveComponent(entityId, ref component);
            }
            catch (Exception e)
            {
                throw new EventException(e);
            }
            finally
            {
                _inEvent = false;
            }
        }

        private void PublishRemoveEvents(uint entityId, EntityReference entityReference, Archetype newArchetype)
        {
            var chunk = entityReference.GetChunk();
            foreach (var metaData in entityReference.Archetype.MetaData)
            {
                if (newArchetype?.ContainsType(metaData.Type) ?? false) continue; // this type was not removed
                var listOffset = entityReference.Archetype.GetListOffset(metaData.Type);
                try
                {
                    _inEvent = true;
                    metaData.OnRemoveComponent(_eventDispatcher, entityId, chunk, listOffset, entityReference.Indices.ListIndex);
                }
                catch (Exception e)
                {
                    throw new EventException(e);
                }
                finally
                {
                    _inEvent = false;
                }
            }
        }

        private List<EnumerationJob> RentJobList()
        {
            lock (_jobListCache)
            {
                return _jobListCache.Count == 0 ? new() : _jobListCache.Pop();
            }
        }

        private QueryFilter RentQueryFilter()
        {
            lock (_queryFilterCache)
            {
                return new QueryFilter(
                    this,
                    _queryFilterCache.Count == 0 ? new() : _queryFilterCache.Pop(),
                    _queryFilterCache.Count == 0 ? new() : _queryFilterCache.Pop(),
                    _queryFilterCache.Count == 0 ? new() : _queryFilterCache.Pop()
                );
            }
        }

        private void ReturnJobList(List<EnumerationJob> jobList)
        {
            lock (_jobListCache)
            {
                jobList.Clear();
                _jobListCache.Push(jobList);
            }
        }

        private void ReturnQueryFilter(in QueryFilter queryFilter)
        {
            lock (_queryFilterCache)
            {
                queryFilter.AnyTypes.Clear();
                queryFilter.NoTypes.Clear();
                queryFilter.WithTypes.Clear();
                _queryFilterCache.Push(queryFilter.AnyTypes);
                _queryFilterCache.Push(queryFilter.NoTypes);
                _queryFilterCache.Push(queryFilter.WithTypes);
            }
        }

        private void SetComponentData(EntityReference entityReference, EntityLayout entityLayout)
        {
            foreach (var pair in entityLayout.AddedComponents)
            {
                var metaData = ComponentMetaData.All[pair.Key];
                if (metaData.Size == 0) continue; // only set non-zero components
                var chunk = entityReference.Archetype.GetChunk(entityReference.Indices.ChunkIndex);
                var listOffset = entityReference.Archetype.GetListOffset(pair.Key);
                metaData.SetComponent(chunk, listOffset, entityReference.Indices.ListIndex, pair.Value);
            }
        }
    }
}

