using System;
using System.Collections.Generic;

namespace EntitiesDb
{
    public sealed class EntityLayout
	{
        private readonly Dictionary<Type, object> _added = new();
        private readonly HashSet<Type> _removed = new();

		public IEnumerable<KeyValuePair<Type, object>> Added => _added;
        public IEnumerable<Type> Removed => _removed;

        /// <summary>
        /// Defines a buffer component to be added.
        /// Overwrites any previous buffer Add or Remove of the same type.
        /// </summary>
        /// <typeparam name="T">Component type</typeparam>
        /// <param name="component">The components to add</param>
        /// <exception cref="ZeroSizeBufferException"></exception>
        /// <exception cref="InvalidBufferableException"></exception>
        public void AddBuffer<T>(ReadOnlySpan<T> components) where T : unmanaged
        {
            var metaData = ComponentMetaData<T>.Instance;
            if (metaData.ZeroSize) throw new ZeroSizeBufferException(typeof(T));
            if (!metaData.Bufferable) throw new InvalidBufferableException(typeof(T));
            _removed.Remove(typeof(T));
            SetBuffer(metaData.Type, components);
        }

        /// <summary>
        /// Defines a component to be added.
        /// Overwrites any previous Add or Remove of the same type.
        /// </summary>
        /// <typeparam name="T">Component type</typeparam>
        /// <param name="component">The component to add</param>
        /// <exception cref="BufferableException"></exception>
        public void AddComponent<T>(T component = default) where T : unmanaged
        {
            var metaData = ComponentMetaData<T>.Instance;
            if (metaData.Bufferable) throw new BufferableException(typeof(T));
            _removed.Remove(typeof(T));
            SetComponent(metaData.Type, component);
        }

		/// <summary>
		/// Clears Added and Removed components
		/// </summary>
		public void Clear()
		{
			_added.Clear();
			_removed.Clear();
        }

        /// <summary>
        /// Defines a buffer component to be removed.
        /// Overwrites any previous buffer Add or Remove of the same type.
        /// </summary>
        /// <typeparam name="T">Component type</typeparam>
        /// <exception cref="ZeroSizeBufferException"></exception>
        /// <exception cref="InvalidBufferableException"></exception>
        public void RemoveBuffer<T>() where T : unmanaged
        {
            var metaData = ComponentMetaData<T>.Instance;
            if (metaData.ZeroSize) throw new ZeroSizeBufferException(typeof(T));
            if (!metaData.Bufferable) throw new InvalidBufferableException(typeof(T));
            var bufferType = typeof(ComponentBuffer<T>);
            _added.Remove(bufferType);
            _removed.Add(bufferType);
        }

        /// <summary>
        /// Defines a component to be removed.
        /// Overwrites any previous Add or Remove of the same type.
        /// </summary>
        /// <typeparam name="T">Component type</typeparam>
        /// <exception cref="BufferableException"></exception>
        public void RemoveComponent<T>() where T : unmanaged
        {
            var metaData = ComponentMetaData<T>.Instance;
            if (metaData.Bufferable) throw new BufferableException(typeof(T));
            _added.Remove(typeof(T));
            _removed.Add(typeof(T));
		}

        private void SetBuffer<T>(Type componentType, ReadOnlySpan<T> components) where T : unmanaged
        {
            // re-use existing list if available
            List<T> list;
            if (_added.TryGetValue(componentType, out var listObject))
            {
                list = (List<T>)listObject;
                list.Clear();
            }
            else
            {
                list = new List<T>();
                _added.Add(componentType, list);
            }

            for (int i = 0; i < components.Length; i++)
            {
                list.Add(components[i]);
            }
        }

        private void SetComponent<T>(Type componentType, T component) where T : unmanaged
        {
            // re-use existing boxed if available
            Boxed<T> boxed;
            if (!_added.TryGetValue(componentType, out var boxedObject))
            {
                boxed = (Boxed<T>)boxedObject;
            }
            else
            {
                boxed = new Boxed<T>();
                _added.Add(typeof(T), boxed);
            }
            boxed.Value = component;
        }
	}
}

