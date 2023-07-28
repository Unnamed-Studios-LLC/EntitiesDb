using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;

namespace EntitiesDb
{
    internal sealed class Archetype : IDisposable
    {
        private const int ChunkAllocSize = 16384;

        private readonly Dictionary<Type, int> _offsets = new();
		private readonly List<Chunk> _chunks = new();
        private readonly int _chunkCapacity;
        private bool _disposed;
        private int _workingCount;

        public Archetype(ulong[] mask, Span<Type> types)
		{
			Mask = mask;
            Types = types.ToArray();
            MetaData = Types.Select(x => ComponentMetaData.All[x]).ToArray();

            // get entity size
            var entitySize = sizeof(uint);
            foreach (var metaData in MetaData)
            {
                _offsets[metaData.Type] = metaData.Size != 0 ? entitySize : -1;
                entitySize += metaData.Size * metaData.Buffer;
            }

            _chunkCapacity = ChunkAllocSize / entitySize;
        }

        ~Archetype() => Dispose();

        public int ChunkCount => _chunks.Count;
        public ulong[] Mask { get; }
        public ComponentMetaData[] MetaData { get; }
        public Type[] Types { get; }

		public EntityIndex Add(uint entityId)
		{
            Chunk chunk;
            if (_workingCount == 0)
            {
                // create chunk
                var data = Marshal.AllocHGlobal(ChunkAllocSize);
                chunk = new Chunk(data);
                _chunks.Add(chunk);
            }
            else
            {
                // use last chunk
                chunk = _chunks[_chunks.Count - 1];
            }

            var listIndex = _workingCount++;
            if (_workingCount == _chunkCapacity) _workingCount = 0;
            chunk[listIndex] = entityId;
            return new EntityIndex(_chunks.Count - 1, listIndex);
        }

        public void Dispose()
        {
            if (_disposed) return;
            foreach (var chunk in _chunks)
            {
                Marshal.FreeHGlobal(chunk.Data);
            }
            _chunks.Clear();
            _disposed = true;
        }

        public bool ContainsType(Type type) => _offsets.ContainsKey(type);

        public Chunk GetChunk(int index) => _chunks[index];
        public int GetChunkLength(int index) => index == _chunks.Count - 1 ? _workingCount : _chunkCapacity;

        public uint GetLastEntityId()
        {
            var count = _workingCount == 0 ? _chunkCapacity : _workingCount;
            var lastChunk = _chunks[_chunks.Count - 1];
            return lastChunk[count - 1];
        }

        public int GetListOffset(Type type) => _offsets[type] * _chunkCapacity;

        public uint Remove(EntityIndex entityIndex)
        {
            if (_workingCount == 0) _workingCount = _chunkCapacity;

            var chunk = _chunks[entityIndex.ChunkIndex];
            var lastChunkIndex = _chunks.Count - 1;
            var lastListIndex = --_workingCount;
            var lastChunk = _chunks[lastChunkIndex];

            uint remappedEntityId;
            if (entityIndex.ChunkIndex == lastChunkIndex &&
                entityIndex.ListIndex == lastListIndex)
            {
                // last entity of last chunk removed
                remappedEntityId = 0;
            }
            else
            {
                // remap last entity to the removed entity position

                // move entity id
                remappedEntityId = chunk[entityIndex.ListIndex] = lastChunk[lastListIndex];
                // move components
                foreach (var metaData in MetaData)
                {
                    if (metaData.Size == 0) continue; // ignore zero-size components
                    var listOffset = _offsets[metaData.Type] * _chunkCapacity;
                    Chunk.Copy(
                        lastChunk, listOffset + lastListIndex * metaData.Size,
                        chunk, listOffset + entityIndex.ListIndex * metaData.Size,
                        metaData.Size
                    );
                }
            }

            if (lastListIndex == 0)
            {
                // last entity of a chunk removed
                _chunks.RemoveAt(lastChunkIndex);
                Marshal.FreeHGlobal(lastChunk.Data);
            }

            return remappedEntityId;
        }

        public bool TryGetListOffset(Type type, out int listOffset)
        {
            if (!_offsets.TryGetValue(type, out listOffset)) return false;
            listOffset *= _chunkCapacity;
            return true;
        }
    }
}

