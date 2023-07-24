namespace EntitiesDb
{
    internal readonly struct EnumerationJob
    {
        public readonly Archetype Archetype;
        public readonly int ChunkIndex;

        public EnumerationJob(Archetype archetype, int chunkIndex)
        {
            Archetype = archetype;
            ChunkIndex = chunkIndex;
        }
    }
}
