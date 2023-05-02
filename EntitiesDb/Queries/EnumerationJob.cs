using EntitiesDb.Data;

namespace EntitiesDb.Queries
{
    internal readonly struct EnumerationJob
    {
        public readonly EntityGroup Group;
        public readonly int ChunkIndex;

        public EnumerationJob(EntityGroup group, int chunkIndex)
        {
            Group = group;
            ChunkIndex = chunkIndex;
        }
    }
}
