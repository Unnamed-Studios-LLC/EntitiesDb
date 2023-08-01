using System.Runtime.CompilerServices;

namespace EntitiesDb;

public unsafe ref struct ComponentHandle<T> where T : unmanaged
{
    private byte* _pointer;
    private readonly int _stride;

    public T Value => *(T*)_pointer;

    internal ComponentHandle(byte* pointer, int stride)
    {
        _pointer = pointer;
        _stride = stride;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public ref T AsRef() => ref Unsafe.AsRef<T>(_pointer);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public void Next() => _pointer += _stride;
}
