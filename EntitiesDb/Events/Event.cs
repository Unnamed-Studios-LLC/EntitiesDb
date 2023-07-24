using System;
using System.Linq;

namespace EntitiesDb
{
	internal sealed class Event<T>
	{
		private T[] _array;
		private readonly object _lock = new();

        public T[] View => _array ?? Array.Empty<T>();

        public void Add(T item)
        {
            lock (_lock)
            {
				var array = View;
				if (array.Contains(item)) return;
				Array.Resize(ref array, array.Length + 1);
				array[array.Length - 1] = item;
            }
        }

		public void Clear()
		{
			lock (_lock)
			{
				_array = null;
			}
		}

		public void Remove(T item)
		{
			lock (_lock)
            {
                var array = View;
				int index = 0;
				foreach (ref var o in array.AsSpan())
				{
					if (o.Equals(item)) break;
					index++;
				}

				if (index >= array.Length) return;

				var newArray = new T[array.Length - 1];
				if (index > 0) Array.Copy(array, 0, newArray, 0, index); // bottom slice
				if (index < array.Length - 1) Array.Copy(array, index + 1, newArray, index, array.Length - index - 1); // top slice
				_array = newArray;
            }
		}
	}
}

