namespace EntitiesDb.Components
{
    internal sealed class ComponentRegistry
    {
        private readonly List<ComponentType> _components = new List<ComponentType>();
        private readonly Dictionary<Type, ComponentType> _componentMap = new();
        private readonly object s_lock = new();

        public void Clear()
        {
            lock (s_lock)
            {
                _components.Clear();
                _componentMap.Clear();
            }
        }

        public ComponentType Get(int componentId) => _components[componentId];
        public ComponentType Get(Type type) => _componentMap[type];
        public ComponentType Get<T>() where T : unmanaged
        {
            var type = typeof(T);
            var metaData = ComponentMetaData<T>.Cached;

            lock (s_lock)
            {
                if (!_componentMap.TryGetValue(type, out var componentType))
                {
                    var id = _components.Count;
                    componentType = new ComponentType(id, metaData);
                    _components.Add(componentType);
                    _componentMap.Add(type, componentType);
                }
                return componentType;
            }
        }
    }
}
