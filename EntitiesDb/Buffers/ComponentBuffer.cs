using System;
using System.Runtime.InteropServices;

namespace EntitiesDb
{
    public unsafe struct ComponentBuffer<T> : IDisposable where T : unmanaged
    {
        private readonly int _internalCapacity;
        private int _size;
        private void* _heap;

        public ComponentBuffer(int internalCapacity, ReadOnlySpan<T> data) : this()
        {
            _internalCapacity = internalCapacity;
            _size = data.Length;

            if (data.Length == 0) return;

            var capacity = Capacity;
            if (capacity > _internalCapacity) _heap = Marshal.AllocHGlobal(capacity * sizeof(T)).ToPointer();
            data.CopyTo(AsSpan());
        }

        private int Capacity
        {
            get
            {
                var capacity = _internalCapacity;
                while (capacity < _size)
                {
                    capacity *= 2;
                }
                return capacity;
            }
        }

        private void* Data
        {
            get
            {
                if (_size > _internalCapacity) return _heap;
                fixed (void** ptr = &_heap)
                {
                    return ptr;
                }
            }
        }

        public void Add(T item) => Add(ref item);
        public void Add(ref T item)
        {
            // only check for resize on significant size values
            if (_size >= _internalCapacity &&
                (_size % _internalCapacity) == 0)
            {
                // possible capacity change
                var capacity = Capacity;
                if (_size == capacity)
                {
                    // determine destination, new heap
                    var newHeapSize = capacity * 2 * sizeof(T);
                    var source = _size == _internalCapacity ? Data : _heap;
                    var destination = Marshal.AllocHGlobal(newHeapSize).ToPointer();

                    // copy items
                    Buffer.MemoryCopy(source, destination, newHeapSize, newHeapSize);

                    // free old heap
                    if (_size != _internalCapacity) Marshal.FreeHGlobal((nint)source);

                    // assign new heap
                    _heap = destination;
                }
            }

            // increment and set item
            ((T*)Data)[_size++] = item;
        }


        public Span<T> AsSpan() => new(Data, _size);

        public void Dispose()
        {
            if (_size <= _internalCapacity) return;
            _size = 0;
            Marshal.FreeHGlobal((nint)_heap);
        }

        /// <summary>
        /// Removes an item at a given index.
        /// </summary>
        /// <param name="index">The index of the item to remove.</param>
        /// <exception cref="ArgumentOutOfRangeException"></exception>
        public void RemoveAtSwapBack(int index)
        {
            if (index < 0 || index >= _size) throw new ArgumentOutOfRangeException(nameof(index));

            // decrement and swap back
            if (index != --_size) ((T*)Data)[index] = ((T*)Data)[_size];

            // only check for resize on significant size values
            if (_size < _internalCapacity ||
                (_size % _internalCapacity) != 0) return;

            // possible capacity change
            var capacity = Capacity;
            if (_size != capacity) return;

            // determine destination (new heap or internal)
            var newHeapSize = capacity * sizeof(T);
            var source = _heap;
            var destination = _size == _internalCapacity ? Data : Marshal.AllocHGlobal(newHeapSize).ToPointer();

            // copy items
            Buffer.MemoryCopy(source, destination, newHeapSize, newHeapSize);

            // free old heap
            Marshal.FreeHGlobal((nint)source);

            // assign new heap, if not internal
            if (_size != _internalCapacity) _heap = destination;
        }
    }
}
