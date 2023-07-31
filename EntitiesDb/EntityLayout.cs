﻿using System;
using System.Collections.Generic;

namespace EntitiesDb
{
    public sealed class EntityLayout
	{
		private readonly Dictionary<Type, object> _added = new();
        private readonly Dictionary<Type, object> _addedBuffers = new();
        private readonly HashSet<Type> _removed = new();

		public IEnumerable<KeyValuePair<Type, object>> Added => _added;
        public IEnumerable<KeyValuePair<Type, object>> AddedBuffers => _addedBuffers;
        public IEnumerable<Type> Removed => _removed;

        /// <summary>
        /// Defines a component to be added.
        /// Overwrites any previous Add or Remove of the same type.
        /// </summary>
        /// <typeparam name="T">Component type</typeparam>
        /// <param name="component">The component to add</param>
        /// <exception cref="BufferableException"></exception>
        public void Add<T>(T? component = default) where T : unmanaged
        {
            var metaData = ComponentMetaData<T>.Instance;
            if (metaData.Bufferable) throw new BufferableException(typeof(T));
			_removed.Remove(typeof(T));
            _added[typeof(T)] = component;
		}

        /// <summary>
        /// Defines a buffer component to be added.
        /// Overwrites any previous buffer Add or Remove of the same type.
        /// </summary>
        /// <typeparam name="T">Component type</typeparam>
        /// <param name="component">The components to add</param>
        /// <exception cref="InvalidBufferableException"></exception>
        public void AddBuffer<T>(ReadOnlySpan<T> components) where T : unmanaged
        {
            var metaData = ComponentMetaData<T>.Instance;
            if (metaData.ZeroSize) throw new ZeroSizeBufferException(typeof(T));
            if (!metaData.Bufferable) throw new InvalidBufferableException(typeof(T));
            ComponentMetaData<ComponentBuffer<T>>.Register();
            _removed.Remove(typeof(T));
            SetBufferComponents(metaData.Type, components);
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
        /// Defines a component to be removed.
        /// Overwrites any previous Add or Remove of the same type.
        /// </summary>
        /// <typeparam name="T">Component type</typeparam>
        public void Remove<T>() where T : unmanaged
        {
            ComponentMetaData<T>.Register();
			_added.Remove(typeof(T));
            _removed.Add(typeof(T));
		}

        /// <summary>
        /// Defines a buffer component to be removed.
        /// Overwrites any previous buffer Add or Remove of the same type.
        /// </summary>
        /// <typeparam name="T">Component type</typeparam>
        public void RemoveBuffer<T>() where T : unmanaged
        {
            var metaData = ComponentMetaData<T>.Instance;
            if (metaData.ZeroSize) throw new ZeroSizeBufferException(typeof(T));
            ComponentMetaData<ComponentBuffer<T>>.Register();
            var bufferType = typeof(ComponentBuffer<T>);
            _addedBuffers.Remove(bufferType);
            _removed.Add(bufferType);
        }

        private void SetBufferComponents<T>(Type componentType, ReadOnlySpan<T> components) where T : unmanaged
        {
            // re-use existing list if available
            List<T> list;
            if (_addedBuffers.TryGetValue(componentType, out var listObject))
            {
                list = (List<T>)listObject;
                list.Clear();
            }
            else
            {
                list = new List<T>();
                _addedBuffers.Add(componentType, list);
            }

            for (int i = 0; i < components.Length; i++)
            {
                list.Add(components[i]);
            }
        }
	}
}

