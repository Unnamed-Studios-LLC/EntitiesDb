using System;
using System.Collections.Generic;

namespace EntitiesDb
{
    public sealed class EntityLayout
	{
		private readonly Dictionary<Type, object> _added = new();
		private readonly HashSet<Type> _removed = new();

		public IEnumerable<KeyValuePair<Type, object>> AddedComponents => _added;
		public IEnumerable<Type> RemovedComponents => _removed;

		/// <summary>
		/// Defines a component to be added.
		/// Overwrites any previous Add or Remove of the same type.
		/// </summary>
		/// <typeparam name="T">Component type</typeparam>
		/// <param name="component">The component to add</param>
		public void Add<T>(T component = default) where T : unmanaged
        {
            ComponentMetaData<T>.Register();
			_removed.Remove(typeof(T));
            _added[typeof(T)] = component;
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
        /// <param name="component">The component to remove</param>
        public void Remove<T>() where T : unmanaged
        {
            ComponentMetaData<T>.Register();
			_added.Remove(typeof(T));
            _removed.Add(typeof(T));
		}
	}
}

