using System;

namespace EntitiesDb;

[AttributeUsage(AttributeTargets.Struct)]
public sealed class BufferAttribute : Attribute
{
    public BufferAttribute(int chunkMax = 2)
    {
        ChunkMax = chunkMax;
    }

    public int ChunkMax { get; }
}
