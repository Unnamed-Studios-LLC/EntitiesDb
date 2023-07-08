using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using EntitiesDb.Components;

namespace EntitiesDb.Data
{
    internal unsafe sealed partial class EntityGroup : IDisposable
    {
        private const int ChunkAllocSize = 16384;

        public readonly EntityArchetype Archetype;

        private readonly List<EntityChunk> _chunks = new();
        private readonly Dictionary<int, int> _idMap = new();
        private readonly int _chunkCapacity;
        private int _workingChunkCount;
        private bool _disposedValue;

        public EntityGroup(EntityArchetype archetype)
        {
            Archetype = archetype;

            var componentIds = archetype.GetNonZeroIds().ToArray();
            var listOffsets = new int[componentIds.Length];
            var chunkCapacity = GetChunkCapacity(componentIds);

            int offset = sizeof(uint) * chunkCapacity;
            for (int i = 0; i < componentIds.Length; i++)
            {
                var id = componentIds[i];
                _idMap[id] = i;
                listOffsets[i] = offset;
                var componentSize = ComponentRegistry.Get(id).Size;
                offset += componentSize * chunkCapacity;
            }

            ComponentIds = componentIds;
            ListOffsets = listOffsets;
            _chunkCapacity = chunkCapacity;
        }

        ~EntityGroup()
        {
            // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
            Dispose(disposing: false);
        }

        public int[] ComponentIds { get; }
        public int[] ListOffsets { get; }
        public int[] QueryIndicies { get; } = new int[6];
        public int ChunkCount => _chunks.Count;

        public EntityIndex Add(uint entityId)
        {
            int chunkIndex;
            EntityChunk chunk;

            if (_chunks.Count > 0 &&
                _workingChunkCount < _chunkCapacity)
            {
                // use last chunk
                chunkIndex = _chunks.Count - 1;
                chunk = _chunks[chunkIndex];
                _workingChunkCount++;
            }
            else
            {
                // add new chunk
                chunk = CreateChunk();
                chunkIndex = _chunks.Count;
                _chunks.Add(chunk);
                _workingChunkCount = 1;
            }

            var listIndex = _workingChunkCount - 1;
            chunk.EntityIds[listIndex] = entityId;
            return new EntityIndex(chunkIndex, listIndex);
        }

        public void Dispose()
        {
            // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }

        public EntityChunk GetChunk(int index) => _chunks[index];
        public int GetChunkLength(int index) => index == _chunks.Count - 1 ? _workingChunkCount : _chunkCapacity;

        public unsafe ref T GetComponent<T>(int chunkIndex, int listIndex) where T : unmanaged
        {
            var chunk = GetChunk(chunkIndex);
            return ref GetComponent<T>(chunk, listIndex);
        }
        public unsafe ref T GetComponent<T>(EntityChunk chunk, int listIndex) where T : unmanaged
        {
            var id = ComponentRegistry.Type<T>.Id;
            if (!Archetype.Contains(id))
            {
                throw new Exception($"Component of type '{typeof(T)}' not found in entity.");
            }

            var index = IndexOf(id);
            if (index < 0) return ref Unsafe.NullRef<T>();

            var list = chunk.GetList<T>(ListOffsets[index]);
            return ref Unsafe.AsRef<T>(list + listIndex);
        }

        public int IndexOf(int id) => _idMap.TryGetValue(id, out var index) ? index : -1;

        public unsafe uint Remove(EntityIndex index)
        {
            uint remappedEntityId;
            var chunk = GetChunk(index.Chunk);
            var lastChunkIndex = _chunks.Count - 1;
            var lastChunk = GetChunk(lastChunkIndex);
            var lastListIndex = --_workingChunkCount;

            if (index.Chunk == lastChunkIndex &&
                index.List == lastListIndex)
            {
                // end entity was removed
                remappedEntityId = 0;
            }
            else
            {
                remappedEntityId = chunk.EntityIds[index.List] = chunk.EntityIds[lastListIndex]; // remap entityId
                for (int i = 0; i < ComponentIds.Length; i++)
                {
                    var componentSize = ComponentRegistry.Get(ComponentIds[i]).Size;
                    var sourcePtr = lastChunk.GetComponent(ListOffsets[i], componentSize, lastListIndex);
                    var destinationPtr = chunk.GetComponent(ListOffsets[i], componentSize, index.List);
                    Buffer.MemoryCopy(sourcePtr, destinationPtr, componentSize, componentSize);
                }
            }

            if (lastListIndex == 0)
            {
                // the last entity of a chunk was removed
                _chunks.RemoveAt(lastChunkIndex);
                lastChunk.Free();

                if (_chunks.Count > 0) _workingChunkCount = _chunkCapacity;
            }

            return remappedEntityId;
        }

        public unsafe ref T TryGetComponent<T>(int chunkIndex, int listIndex, out bool found) where T : unmanaged
        {
            var chunk = GetChunk(chunkIndex);
            return ref TryGetComponent<T>(chunk, listIndex, out found);
        }
        public unsafe ref T TryGetComponent<T>(EntityChunk chunk, int listIndex, out bool found) where T : unmanaged
        {
            var id = ComponentRegistry.Type<T>.Id;
            if (!Archetype.Contains(id))
            {
                found = false;
                return ref Unsafe.NullRef<T>();
            }

            found = true;
            var index = IndexOf(id);
            if (index < 0) return ref Unsafe.NullRef<T>();

            var list = chunk.GetList<T>(ListOffsets[index]);
            return ref Unsafe.AsRef<T>(list + listIndex);
        }

        public unsafe bool TryGetLastEntityId(out uint entityId)
        {
            if (_chunks.Count == 0)
            {
                entityId = 0;
                return false;
            }

            var chunk = _chunks[^1];
            entityId = chunk.EntityIds[_workingChunkCount - 1];
            return true;
        }

        private static EntityChunk CreateChunk()
        {
            var ptr = Marshal.AllocHGlobal(ChunkAllocSize);
            return new EntityChunk(ptr.ToPointer());
        }

        private static int GetChunkCapacity(int[] componentIds)
        {
            var lineSize = sizeof(uint); // entityId
            for (int i = 0; i < componentIds.Length; i++)
            {
                lineSize += ComponentRegistry.Get(componentIds[i]).Size;
            }

            var lines = ChunkAllocSize / lineSize;
            return lines;
        }

        private void Dispose(bool disposing)
        {
            if (!_disposedValue)
            {
                if (disposing)
                {
                    // TODO: dispose managed state (managed objects)
                }

                var span = CollectionsMarshal.AsSpan(_chunks);
                foreach (ref var chunk in span) chunk.Free();
                _chunks.Clear();
                _disposedValue = true;
            }
        }
    }
}
