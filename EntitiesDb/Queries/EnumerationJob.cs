namespace EntitiesDb;

public readonly unsafe struct EnumerationJob
{
    public readonly int Length;

    internal readonly Archetype Archetype;
    internal readonly Chunk Chunk;

    internal EnumerationJob(Archetype archetype, Chunk chunk, int length)
    {
        Archetype = archetype;
        Chunk = chunk;
        Length = length;
    }

    public ComponentHandle<uint> GetEntityIdHandle()
    {
        var pointer = (byte*)Chunk.Data.ToPointer();
        return new ComponentHandle<uint>(pointer, sizeof(uint));
    }

    public ComponentHandle<T> GetComponentHandle<T>() where T : unmanaged
    {
        var (offset, stride) = Archetype.GetListOffsetAndStride(typeof(T));
        var pointer = Chunk.GetList(offset);
        return new ComponentHandle<T>(pointer, stride);
    }

    public ComponentHandle<ComponentBuffer<T>> GetComponentBufferHandle<T>() where T : unmanaged
    {
        var (offset, stride) = Archetype.GetListOffsetAndStride(typeof(T));
        var pointer = Chunk.GetList(offset);
        return new ComponentHandle<ComponentBuffer<T>>(pointer, stride);
    }
}
