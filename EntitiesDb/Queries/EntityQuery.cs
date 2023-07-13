using EntitiesDb.Data;

namespace EntitiesDb.Queries
{
    internal struct EntityQuery : IQuery
    {
        private readonly EntityFunc _entityFunc;

        public EntityQuery(EntityFunc entityFunc)
        {
            _entityFunc = entityFunc;
        }

        public void CopyIndices(EntityGroup group, Span<int> indices)
        {

        }

        public void EnumerateChunk(in EnumerationJob job, Span<int> indices)
        {
            var entityCount = job.Group.GetChunkLength(job.ChunkIndex);
            var chunk = job.Group.GetChunk(job.ChunkIndex);
            var entity = new Entity(job.Group, chunk, 0);
            for (int i = 0; i < entityCount; i++)
            {
                entity.ListIndex = i;
                _entityFunc(in entity);
            }
        }

        public IEnumerable<int> GetRequiredIds() => Enumerable.Empty<int>();
    }

    internal struct EntityStateQuery<TState> : IQuery
    {
        private readonly EntityStateFunc<TState> _entityFunc;
        private readonly TState _state;

        public EntityStateQuery(EntityStateFunc<TState> entityFunc, TState state)
        {
            _entityFunc = entityFunc;
            _state = state;
        }

        public void CopyIndices(EntityGroup group, Span<int> indices)
        {

        }

        public void EnumerateChunk(in EnumerationJob job, Span<int> indices)
        {
            var entityCount = job.Group.GetChunkLength(job.ChunkIndex);
            var chunk = job.Group.GetChunk(job.ChunkIndex);
            var entity = new Entity(job.Group, chunk, 0);
            for (int i = 0; i < entityCount; i++)
            {
                entity.ListIndex = i;
                _entityFunc(in entity, _state);
            }
        }

        public IEnumerable<int> GetRequiredIds() => Enumerable.Empty<int>();
    }
}
