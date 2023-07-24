using System;
using System.Runtime.CompilerServices;

namespace EntitiesDb
{
    internal readonly unsafe struct Chunk
	{
		public readonly IntPtr Data;

		public Chunk(IntPtr data)
		{
			Data = data;
		}

		public uint this[int index]
		{
			get => ((uint*)Data)[index];
			set => ((uint*)Data)[index] = value;
        }

		public static void Copy(Chunk source, int sourceOffset, Chunk destination, int destinationOffset, int size)
		{
			Buffer.MemoryCopy(
				(byte*)source.Data + sourceOffset,
				(byte*)destination.Data + destinationOffset,
				size, size
			);
        }

        public byte* GetComponent(int listOffset, int listIndex, int size)
        {
			return (byte*)Data + listOffset + listIndex * size;
        }

		public ref T GetComponent<T>(int listOffset, int listIndex) where T : unmanaged
		{
            return ref Unsafe.AsRef<T>(GetComponent(listOffset, listIndex, sizeof(T)));
        }

        public T* GetList<T>(int listOffset) where T : unmanaged
        {
            return (T*)((byte*)Data + listOffset);
        }
    }
}

