using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace EntitiesDb;

public unsafe readonly struct ComponentBuffer<T> : IDisposable where T : unmanaged
{
    private readonly ComponentBufferHeader* _header;

    internal ComponentBuffer(ref ComponentBufferHeader headerRef)
    {
        _header = (ComponentBufferHeader*)Unsafe.AsPointer(ref headerRef);
    }

    internal ComponentBuffer(ComponentBufferHeader* header)
    {
        _header = header;
    }

    internal ComponentBuffer(void* header)
    {
        _header = (ComponentBufferHeader*)header;
    }

    internal ComponentBuffer(ComponentBuffer<T> previous, int internalCapacity, ReadOnlySpan<T> data)
    {
        _header = previous._header;
        _header->InternalCapacity = internalCapacity;
        _header->Size = data.Length;

        if (data.Length == 0) return;

        var capacity = Capacity;
        if (capacity > _header->InternalCapacity) _header->Heap = Marshal.AllocHGlobal(capacity * sizeof(T)).ToPointer();
        data.CopyTo(Span);
    }

    public ref T this[int index]
    {
        get
        {
            if (index < 0 || index >= _header->Size) throw new ArgumentOutOfRangeException(nameof(index));
            return ref Unsafe.AsRef<T>((T*)Data + index);
        }
    }

    /// <summary>
    /// The amount of items in the buffer
    /// </summary>
    public int Length => _header->Size;

    /// <summary>
    /// Returns a span representation of the buffer.
    /// Span should not be used after any add or remove call.
    /// </summary>
    public Span<T> Span => new(Data, _header->Size);

    private int Capacity
    {
        get
        {
            var capacity = _header->InternalCapacity;
            while (capacity < _header->Size)
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
            if (_header->Size > _header->InternalCapacity) return _header->Heap;
            return (byte*)_header + ComponentBufferHeader.DataOffset;
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
        if (_header->Size >= _header->InternalCapacity &&
            (_header->Size % _header->InternalCapacity) == 0)
        {
            // possible capacity change
            var capacity = Capacity;
            if (_header->Size == capacity)
            {
                // determine destination, new heap
                var newHeapSize = capacity * 2 * sizeof(T);
                var source = _header->Size == _header->InternalCapacity ? Data : _header->Heap;
                var destination = Marshal.AllocHGlobal(newHeapSize).ToPointer();

                // copy items
                Buffer.MemoryCopy(source, destination, newHeapSize, capacity * sizeof(T));

                // free old heap
                if (_header->Size != _header->InternalCapacity) Marshal.FreeHGlobal((nint)source);

                // assign new heap
                _header->Heap = destination;
            }
        }

        // increment and set item
        _header->Size++;
        ((T*)Data)[_header->Size - 1] = item;
    }

    /// <summary>
    /// Clears all items in the buffer
    /// </summary>
    public void Clear()
    {
        if (_header->Size > _header->InternalCapacity)
        {
            Marshal.FreeHGlobal((nint)_header->Heap);
        }
        _header->Size = 0;
    }

    /// <summary>
    /// Disposes unmanaged resource.
    /// </summary>
    public void Dispose() => Clear();

    /// <summary>
    /// Gets an item from the buffer
    /// </summary>
    /// <param name="index"></param>
    /// <returns></returns>
    /// <exception cref="ArgumentOutOfRangeException"></exception>
    public ref T Get(int index)
    {
        if (index < 0 || index >= _header->Size) throw new ArgumentOutOfRangeException(nameof(index));
        return ref Unsafe.AsRef<T>((T*)Data + index);
    }

    /// <summary>
    /// Removes an item at a given index.
    /// </summary>
    /// <param name="index">The index of the item to remove.</param>
    /// <exception cref="ArgumentOutOfRangeException"></exception>
    public void RemoveAtSwapBack(int index)
    {
        if (index < 0 || index >= _header->Size) throw new ArgumentOutOfRangeException(nameof(index));

        // decrement and swap back
        var nextSize = _header->Size - 1;
        if (index != nextSize) ((T*)Data)[index] = ((T*)Data)[nextSize];
        _header->Size = nextSize;

        // only check for resize on significant size values
        if (_header->Size < _header->InternalCapacity ||
            (_header->Size % _header->InternalCapacity) != 0) return;

        // possible capacity change
        var capacity = Capacity;
        if (_header->Size != capacity) return;

        // determine destination (new heap or internal)
        var newHeapSize = capacity * sizeof(T);
        var source = _header->Heap;
        var destination = _header->Size == _header->InternalCapacity ? Data : Marshal.AllocHGlobal(newHeapSize).ToPointer();

        // copy items
        Buffer.MemoryCopy(source, destination, newHeapSize, newHeapSize);

        // free old heap
        Marshal.FreeHGlobal((nint)source);

        // assign new heap, if not internal
        if (_header->Size != _header->InternalCapacity) _header->Heap = destination;
    }
}
