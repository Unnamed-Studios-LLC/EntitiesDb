using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace EntitiesDb
{
    internal unsafe abstract class ComponentMetaData
    {
        public static readonly ConcurrentDictionary<Type, ComponentMetaData> All = new();

        public ComponentMetaData(Type type, int size, int? internalCapacity)
		{
			Type = type;
			Size = size;
			InternalCapacity = internalCapacity ?? 0;
			Bufferable = InternalCapacity > 0;
			Stride = !Bufferable ? Size : ComponentBufferHeader.DataOffset + Math.Max(InternalCapacity * Size, sizeof(void*));
		}

		/// <summary>
		/// Type of the component
		/// </summary>
		public Type Type { get; }

        /// <summary>
        /// Size of the component in bytes
        /// </summary>
        public int Size { get; }

		/// <summary>
		/// The internal buffer capacity of the component
		/// </summary>
		public int InternalCapacity { get; }

		/// <summary>
		/// If this component
		/// </summary>
		public bool Bufferable { get; }

		/// <summary>
		/// If the component contains no data
		/// </summary>
		public bool ZeroSize => Size == 0;

		public int Stride { get; }

		public abstract void OnAddComponent(EventDispatcher eventDispatcher, uint entityId, Chunk chunk, int listOffset, int listIndex);

		public abstract void OnRemoveComponent(EventDispatcher eventDispatcher, uint entityId, Chunk chunk, int listOffset, int listIndex);

		public void SetComponent(Chunk chunk, int listOffset, int listIndex, object boxed)
		{
			if (ZeroSize || boxed == null) return;
			SetComponent(chunk.GetComponent(listOffset, listIndex, Size), boxed);
		}

		public abstract void SetBuffer(Chunk chunk, int listOffset, int listIndex, object list, bool overwrite);
		public abstract void SetComponent(void* destination, object component);
    }

	internal unsafe sealed class ComponentMetaData<T> : ComponentMetaData where T : unmanaged
	{
		public static readonly ComponentMetaData<T> Instance;
		public static T Empty;
        public static ComponentBuffer<T> EmptyBuffer;

        static ComponentMetaData()
		{
			Instance = new();
			All[typeof(T)] = Instance;
		}

        public ComponentMetaData() : base(typeof(T), IsZeroSize(typeof(T)) ? 0 : sizeof(T), GetInternalCapacity(typeof(T)))
        {
        }

        /// <summary>
        /// Called to ensure component meta data is generated
        /// </summary>
        public static void Register() { }

        public override void OnAddComponent(EventDispatcher eventDispatcher, uint entityId, Chunk chunk, int listOffset, int listIndex)
        {
			if (Bufferable)
            {
                var buffer = chunk.GetBuffer<T>(listOffset, listIndex, Stride);
                eventDispatcher.OnAddComponent(entityId, ref buffer);
            }
			else
            {
                ref var component = ref Empty;
                if (!ZeroSize) component = ref chunk.GetComponent<T>(listOffset, listIndex, Stride);
                eventDispatcher.OnAddComponent(entityId, ref component);
            }
        }

        public override void OnRemoveComponent(EventDispatcher eventDispatcher, uint entityId, Chunk chunk, int listOffset, int listIndex)
        {
            if (Bufferable)
            {
                var buffer = chunk.GetBuffer<T>(listOffset, listIndex, Stride);
                eventDispatcher.OnAddComponent(entityId, ref buffer);
            }
            else
            {
                ref var component = ref Empty;
                if (!ZeroSize) component = ref chunk.GetComponent<T>(listOffset, listIndex, Stride);
                eventDispatcher.OnAddComponent(entityId, ref component);
            }
        }

        public override unsafe void SetComponent(void* destination, object component)
		{
			T value;
			if (component is Boxed<T> boxed) value = boxed.Value;
            else value = (T)component;
            *(T*)destination = value;
        }

        private static int? GetInternalCapacity(Type type)
        {
			var bufferable = type.GetCustomAttribute<BufferableAttribute>();
			if (bufferable == null || bufferable.InternalCapacity < 1) return null;
			return bufferable.InternalCapacity;
        }

        private static bool IsZeroSize(Type type)
        {
            var zeroSize = type.IsValueType &&
				!type.IsPrimitive &&
                type.GetFields((BindingFlags)0x34)
				.All(fi => IsZeroSize(fi.FieldType));
            return zeroSize;
        }

        public override void SetBuffer(Chunk chunk, int listOffset, int listIndex, object list, bool overwrite)
        {
			var typedList = (List<T>)list;
			var buffer = chunk.GetBuffer<T>(listOffset, listIndex, Stride);
			if (overwrite) buffer.Dispose();
			buffer = new ComponentBuffer<T>(buffer, InternalCapacity, ReadOnlySpan<T>.Empty);
			foreach (var value in typedList)
			{
				buffer.Add(value);
			}
        }
    }
}

