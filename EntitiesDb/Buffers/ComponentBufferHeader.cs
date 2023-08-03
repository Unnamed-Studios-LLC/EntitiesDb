using System.Runtime.InteropServices;

namespace EntitiesDb;

internal unsafe struct ComponentBufferHeader
{
    public const int DataOffset = sizeof(int) * 2;

    public int InternalCapacity;
    public int Size;
    public void* Heap;

    /// <summary>
    /// Disposes unmanaged resource.
    /// </summary>
    public void Dispose()
    {
        if (Size > InternalCapacity)
        {
            Marshal.FreeHGlobal((nint)Heap);
        }
        Size = 0;
    }
}
