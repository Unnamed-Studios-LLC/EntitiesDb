using EntitiesDb.Data;

namespace EntitiesDb.Queries
{
    internal struct IdEntityQuery : IQuery
    {
        private readonly IdEntityFunc _idEntityFunc;

        public IdEntityQuery(IdEntityFunc idEntityFunc)
        {
            _idEntityFunc = idEntityFunc;
        }

        public void CopyIndices(EntityGroup group, Span<int> indices)
        {

        }

        public unsafe void EnumerateChunk(in EnumerationJob job, Span<int> indices)
        {
            var entityCount = job.Group.GetChunkLength(job.ChunkIndex);
            var chunk = job.Group.GetChunk(job.ChunkIndex);
            var entity = new Entity(job.Group, chunk, 0);
            for (int i = 0; i < entityCount; i++)
            {
                entity.ListIndex = i;
                _idEntityFunc(chunk.EntityIds[i], in entity);
            }
        }

        public IEnumerable<int> GetRequiredIds() => Enumerable.Empty<int>();
    }
}
