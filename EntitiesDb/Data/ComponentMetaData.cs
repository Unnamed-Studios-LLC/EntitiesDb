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
			Stride = !Bufferable ? Size : ComponentBuffer.HeaderSize + InternalCapacity * Size;
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

        public abstract object CreateDefault();

        public object GetComponent(Chunk chunk, int listOffset, int listIndex)
		{
			if (ZeroSize) return CreateDefault();
			return GetComponent(chunk.GetComponent(listOffset, listIndex, Size));
		}

        public abstract object GetComponent(void* source);

		public abstract bool IsInstanceOfType(object value);

		public abstract void OnAddComponent(EventDispatcher eventDispatcher, uint entityId, Chunk chunk, int listOffset, int listIndex);

		public abstract void OnRemoveComponent(EventDispatcher eventDispatcher, uint entityId, Chunk chunk, int listOffset, int listIndex);

		public void SetComponent(Chunk chunk, int listOffset, int listIndex, object value)
		{
			if (ZeroSize) return;
			SetComponent(chunk.GetComponent(listOffset, listIndex, Size), value);
		}

		public abstract void SetComponent(void* destination, object component);

		public abstract void SetComponentBuffer(Chunk chunk, int listOffset, int listIndex, object list, bool overwrite);
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

		public override object CreateDefault() => default(T);

        public override unsafe object GetComponent(void* source) => *(T*)source;

        public override bool IsInstanceOfType(object value) => value is T;

        public override void OnAddComponent(EventDispatcher eventDispatcher, uint entityId, Chunk chunk, int listOffset, int listIndex)
        {
			ref var component = ref Empty;
			if (!ZeroSize) component = ref chunk.GetComponent<T>(listOffset, listIndex, Stride);
            eventDispatcher.OnAddComponent(entityId, ref component);
        }

        public override void OnRemoveComponent(EventDispatcher eventDispatcher, uint entityId, Chunk chunk, int listOffset, int listIndex)
        {
            ref var component = ref Empty;
            if (!ZeroSize) component = ref chunk.GetComponent<T>(listOffset, listIndex, Stride);
            eventDispatcher.OnRemoveComponent(entityId, ref component);
        }

        public override unsafe void SetComponent(void* destination, object component) => *(T*)destination = (T)component;

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

        public override void SetComponentBuffer(Chunk chunk, int listOffset, int listIndex, object list, bool overwrite)
        {
			var typedList = (List<T>)list;
			ref var buffer = ref chunk.GetComponent<ComponentBuffer<T>>(listOffset, listIndex, Stride);
			if (overwrite) buffer.Dispose();
			buffer = new ComponentBuffer<T>(InternalCapacity, ReadOnlySpan<T>.Empty);
			foreach (var value in typedList)
			{
				buffer.Add(value);
			}
        }
    }
}

