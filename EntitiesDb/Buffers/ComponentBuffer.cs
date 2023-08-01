using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace EntitiesDb
{
    public static class ComponentBuffer
    {
        public const int HeaderSize = 8;
    }

    public unsafe struct ComponentBuffer<T> : IDisposable where T : unmanaged
    {
        private readonly int _internalCapacity;
        private int _size;
        private void* _heap;

        internal ComponentBuffer(int internalCapacity, int size, void* heap)
        {
            _internalCapacity = internalCapacity;
            _size = size;
            _heap = heap;
        }

        internal ComponentBuffer(int internalCapacity, ReadOnlySpan<T> data)
        {
            _internalCapacity = internalCapacity;
            _size = data.Length;

            if (data.Length == 0) return;

            var capacity = Capacity;
            if (capacity > _internalCapacity) _heap = Marshal.AllocHGlobal(capacity * sizeof(T)).ToPointer();
            else _heap = default;
            data.CopyTo(GetSpan());
        }

        public ref T this[int index]
        {
            get
            {
                if (index < 0 || index >= _size) throw new ArgumentOutOfRangeException(nameof(index));
                return ref Unsafe.AsRef<T>((T*)Data + index);
            }
        }

        /// <summary>
        /// The amount of items in the buffer
        /// </summary>
        public int Length => _size;

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

        /// <summary>
        /// <inheritdoc cref="Add(ref T)"/>
        /// </summary>
        /// <param name="item">Item to add</param>
        public void Add(T item) => Add(ref item);

        /// <summary>
        /// Adds an item to the buffer
        /// </summary>
        /// <param name="item">Item to add</param>
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
                    Buffer.MemoryCopy(source, destination, newHeapSize, capacity * sizeof(T));

                    // free old heap
                    if (_size != _internalCapacity) Marshal.FreeHGlobal((nint)source);

                    // assign new heap
                    _heap = destination;
                }
            }

            // increment and set item
            ((T*)Data)[_size++] = item;
        }

        /// <summary>
        /// Clears all items in the buffer
        /// </summary>
        public void Clear()
        {
            if (_size > _internalCapacity)
            {
                Marshal.FreeHGlobal((nint)_heap);
            }
            _size = 0;
        }

        /// <summary>
        /// Disposes unmanaged resource.
        /// </summary>
        public void Dispose() => Clear();

        /// <summary>
        /// Returns a span representation of the buffer.
        /// Span should not be used after any add or remove call.
        /// </summary>
        public Span<T> GetSpan() => new(Data, _size);

        /// <summary>
        /// Reinterpret this buffer to a given type of the same size.
        /// </summary>
        /// <typeparam name="TDestination">Destination type</typeparam>
        /// <returns></returns>
        public ComponentBuffer<TDestination> Reinterpret<TDestination>() where TDestination : unmanaged
        {
            if (sizeof(T) != sizeof(TDestination)) throw new ReinterpretSizeException(typeof(T), typeof(TDestination));
            return new ComponentBuffer<TDestination>(_internalCapacity, _size, _heap);
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
