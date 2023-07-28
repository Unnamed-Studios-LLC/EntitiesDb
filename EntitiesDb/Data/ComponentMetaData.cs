using System;
using System.Collections.Concurrent;
using System.Linq;
using System.Reflection;

namespace EntitiesDb
{
    internal unsafe abstract class ComponentMetaData
    {
        public static readonly ConcurrentDictionary<Type, ComponentMetaData> All = new();

        public ComponentMetaData(Type type, int size, int buffer)
		{
			Type = type;
			Size = size;
			InternalCapacity = buffer;
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

		public abstract object CreateDefault();

        public object GetComponent(Chunk chunk, int listOffset, int listIndex)
		{
			if (Size == 0) return CreateDefault();
			return GetComponent(chunk.GetComponent(listOffset, listIndex, Size));
		}

        public abstract object GetComponent(void* source);

		public abstract bool IsInstanceOfType(object value);

		public abstract void OnAddComponent(EventDispatcher eventDispatcher, uint entityId, Chunk chunk, int listOffset, int listIndex);

		public abstract void OnRemoveComponent(EventDispatcher eventDispatcher, uint entityId, Chunk chunk, int listOffset, int listIndex);

		public void SetComponent(Chunk chunk, int listOffset, int listIndex, object value)
		{
			if (Size == 0) return;
			SetComponent(chunk.GetComponent(listOffset, listIndex, Size), value);
		}

		public abstract void SetComponent(void* destination, object component);
	}

	internal unsafe sealed class ComponentMetaData<T> : ComponentMetaData where T : unmanaged
	{
		public static readonly ComponentMetaData<T> Instance;
		public static T Empty;

		static ComponentMetaData()
		{
			Instance = new();
			All[typeof(T)] = Instance;
		}

        public ComponentMetaData() : base(typeof(T), IsZeroSize(typeof(T)) ? 0 : sizeof(T), GetBuffer(typeof(T)))
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
			if (Size != 0) component = ref chunk.GetComponent<T>(listOffset, listIndex);
            eventDispatcher.OnAddComponent(entityId, ref component);
        }

        public override void OnRemoveComponent(EventDispatcher eventDispatcher, uint entityId, Chunk chunk, int listOffset, int listIndex)
        {
            ref var component = ref Empty;
            if (Size != 0) component = ref chunk.GetComponent<T>(listOffset, listIndex);
            eventDispatcher.OnRemoveComponent(entityId, ref component);
        }

        public override unsafe void SetComponent(void* destination, object component) => *(T*)destination = (T)component;

        private static int GetBuffer(Type type)
        {
			var internalCapacity = type.GetCustomAttribute<BufferAttribute>();
			return Math.Max(1, internalCapacity?.InternalCapacity ?? 1);
        }

        private static bool IsZeroSize(Type type)
        {
            var zeroSize = type.IsValueType &&
				!type.IsPrimitive &&
                type.GetFields((BindingFlags)0x34)
				.All(fi => IsZeroSize(fi.FieldType));
            return zeroSize;
        }
    }
}

