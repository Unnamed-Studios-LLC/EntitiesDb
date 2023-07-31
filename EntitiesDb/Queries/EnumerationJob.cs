namespace EntitiesDb;

public readonly struct EnumerationJob
{
    internal readonly Archetype Archetype;
    internal readonly int ChunkIndex;

    internal EnumerationJob(Archetype archetype, int chunkIndex)
    {
        Archetype = archetype;
        ChunkIndex = chunkIndex;
    }
}
