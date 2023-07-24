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

		public void Add<T>(T component = default) where T : unmanaged
        {
            ComponentMetaData<T>.Register();
			_removed.Remove(typeof(T));
            _added[typeof(T)] = component;
		}

		public void Clear()
		{
			_added.Clear();
			_removed.Clear();
		}

		public void Remove<T>() where T : unmanaged
        {
            ComponentMetaData<T>.Register();
			_added.Remove(typeof(T));
            _removed.Add(typeof(T));
		}
	}
}

