using System.Runtime.CompilerServices;

namespace EntitiesDb;

public unsafe ref struct ComponentBufferHandle<T> where T : unmanaged
{
    public ComponentBuffer<T> Buffer;
    private byte* _pointer;
    private readonly int _stride;

    internal ComponentBufferHandle(byte* pointer, int stride)
    {
        _pointer = pointer;
        _stride = stride;
        Buffer = new ComponentBuffer<T>(_pointer);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public void Next()
    {
        _pointer += _stride;
        Buffer = new ComponentBuffer<T>(_pointer);
    }
}
