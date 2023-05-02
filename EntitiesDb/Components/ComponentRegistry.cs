using System.Runtime.CompilerServices;

namespace EntitiesDb.Components
{
    internal static class ComponentRegistry
    {
        internal unsafe static class Type<T> where T : unmanaged
        {
            private static int? _id;
            public static int Id = GetId();
            public static bool ZeroSize = typeof(T).IsZeroSize();

            private static int GetId()
            {
                lock (s_lock)
                {
                    if (_id == null)
                    {
                        var id = typeof(T) == typeof(Disabled) ? 0 : s_nextId++;
                        if (id >= _types.Length)
                        {
                            var newArray = new ComponentType[_types.Length * 2];
                            Array.Copy(_types, newArray, _types.Length);
                            _types = newArray;
                        }

                        _types[id] = new ComponentType
                        {
                            Id = id,
                            Size = ZeroSize ? 0 : sizeof(T),
                            OnAdd = (entityDatabase, entityId, component) => entityDatabase.PublishAddEvent(entityId, ref Unsafe.AsRef<T>(component)),
                            OnRemove = (entityDatabase, entityId, component) => entityDatabase.PublishRemoveEvent(entityId, ref Unsafe.AsRef<T>(component)),
                        };
                        _typeMap[typeof(T)] = id;
                        _id = id;
                    }
                    return _id.Value;
                }
            }
        }

        private static ComponentType[] _types = new ComponentType[64];
        private static readonly Dictionary<Type, int> _typeMap = new();
        private static readonly object s_lock = new();
        private static int s_nextId = 1;

        public static int Count => s_nextId;

        public static ref ComponentType GetType(int componentId) => ref _types[componentId];
        public static ref ComponentType GetType(Type type) => ref _types[_typeMap[type]];
    }
}
