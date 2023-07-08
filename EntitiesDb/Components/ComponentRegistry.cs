using System.Runtime.CompilerServices;

namespace EntitiesDb.Components
{
    internal static class ComponentRegistry
    {
        internal unsafe static class Type<T> where T : unmanaged
        {
            static Type()
            {
                Id = GetId();
                ref var component = ref _components[Id];
                component.AddLayout = EntityLayoutBuilder.Create().Add<T>().Build();
                component.RemoveLayout = EntityLayoutBuilder.Create().Remove<T>().Build();
            }

            private static int? _id;
            public static int Id;
            public static bool ZeroSize = typeof(T).IsZeroSize();

            private static int GetId()
            {
                lock (s_lock)
                {
                    if (_id == null)
                    {
                        var id = s_nextId++;
                        if (id >= _components.Length)
                        {
                            var newArray = new ComponentType[_components.Length * 2];
                            Array.Copy(_components, newArray, _components.Length);
                            _components = newArray;
                        }

                        var type = typeof(T);
                        _components[id] = new ComponentType
                        {
                            Type = type,
                            Id = id,
                            Size = ZeroSize ? 0 : sizeof(T),
                            OnAdd = (entityDatabase, entityId, component) => entityDatabase.PublishAddEvent(entityId, ref Unsafe.AsRef<T>(component)),
                            OnRemove = (entityDatabase, entityId, component) => entityDatabase.PublishRemoveEvent(entityId, ref Unsafe.AsRef<T>(component)),
                            Getter = (database, id) => ZeroSize ? new T() : database.GetComponent<T>(id),
                            Setter = (database, id, value) =>
                            {
                                if (ZeroSize) return;
                                ref var component = ref database.GetComponent<T>(id);
                                component = (T)value;
                            }
                        };
                        _id = id;
                        _componentMap[type] = id;
                    }
                    return _id.Value;
                }
            }
        }

        private static ComponentType[] _components = new ComponentType[64];
        private static readonly Dictionary<Type, int> _componentMap = new();
        private static readonly object s_lock = new();
        private static int s_nextId = 1;

        public static int Count => s_nextId;

        public static ref ComponentType Get(int componentId) => ref _components[componentId];
        public static ref ComponentType Get(Type type) => ref _components[_componentMap[type]];
        public static ref ComponentType Get<T>() where T : unmanaged => ref Get(Type<T>.Id);
    }
}
