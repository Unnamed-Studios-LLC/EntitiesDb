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

        public byte* GetComponent(int listOffset, int listIndex, int stride)
        {
			return (byte*)Data + listOffset + listIndex * stride;
        }

		public ref T GetComponent<T>(int listOffset, int listIndex, int stride) where T : unmanaged
		{
            return ref Unsafe.AsRef<T>(GetComponent(listOffset, listIndex, stride));
        }

        public byte* GetList(int listOffset)
        {
            return (byte*)Data + listOffset;
        }

        public T* GetList<T>(int listOffset) where T : unmanaged
        {
            return (T*)((byte*)Data + listOffset);
        }
    }
}

