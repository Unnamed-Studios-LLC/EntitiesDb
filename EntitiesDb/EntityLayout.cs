namespace EntitiesDb
{
    public unsafe class EntityLayout
    {
        internal readonly Dictionary<Type, object> Added = new();
        internal readonly HashSet<Type> Removed = new();

        public void Clear()
        {
            Added.Clear();
            Removed.Clear();
        }

        public EntityLayout Add<T>(T component = default) where T : unmanaged
        {
            var type = typeof(T);
            Added[type] = component;
            Removed.Remove(type);
            return this;
        }

        public EntityLayout Remove<T>() where T : unmanaged
        {
            var type = typeof(T);
            Added.Remove(type);
            Removed.Add(type);
            return this;
        }

        internal void Apply(EntityArchetype inputArchetype, EntityArchetype resultArchetype)
        {
            for (int i = 0; i < resultArchetype.Depth; i++)
            {
                ulong mask = 0;
                if (i < inputArchetype.Depth) mask = inputArchetype[i];
                if (i < RemoveArchetype.Depth) mask &= ~RemoveArchetype[i];
                if (i < AddArchetype.Depth) mask |= AddArchetype[i];
                resultArchetype[i] = mask;
            }
        }

        internal int GetDepthResult(EntityArchetype targetArchetype)
        {
            int depth = targetArchetype.Depth;
            var addDepth = AddArchetype.Depth;
            if (addDepth >= depth) return addDepth;
            var removeDepth = RemoveArchetype.Depth;
            if (depth > removeDepth) return depth;
            for (int i = depth; i > addDepth; i--)
            {
                var mask = targetArchetype[i - 1];
                var removeMask = RemoveArchetype[i - 1];
                if ((mask & (~removeMask)) != 0) return i; // not all components removed at this depth
            }
            return 0;
        }
    }
}
