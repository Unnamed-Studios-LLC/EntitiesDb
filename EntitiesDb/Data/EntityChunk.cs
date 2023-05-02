using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace EntitiesDb.Data
{
    internal unsafe struct EntityChunk
    {
        private readonly void* _ptr;

        public EntityChunk(void* ptr)
        {
            _ptr = ptr;
        }

        public uint* EntityIds => (uint*)_ptr;

        public void Free()
        {
            Marshal.FreeHGlobal(new IntPtr(_ptr));
        }

        public byte* GetComponent(int offset, int componentSize, int listIndex) => (byte*)_ptr + offset + componentSize * listIndex;
        public T* GetList<T>(int offset) where T : unmanaged => (T*)((byte*)_ptr + offset);
    }
}
