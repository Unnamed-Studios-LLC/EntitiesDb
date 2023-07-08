using EntitiesDb.Components;

namespace EntitiesDb
{
    public unsafe class EntityLayout
    {
        internal readonly EntityArchetype AddArchetype;
        internal readonly EntityArchetype RemoveArchetype;
        internal readonly byte[] ComponentData;
        internal readonly ulong[]? HasDataMask;

        private readonly Dictionary<int, int> _dataOffsets = new();

        internal EntityLayout(EntityArchetype addArchetype, EntityArchetype removeArchetype)
        {
            AddArchetype = addArchetype;
            RemoveArchetype = removeArchetype;

            var dataSize = 0;
            foreach (var componentId in AddArchetype.GetIds())
            {
                ref var component = ref ComponentRegistry.Get(componentId);
                _dataOffsets[componentId] = dataSize;
                dataSize += component.Size;
            }

            ComponentData = new byte[dataSize];
            HasDataMask = addArchetype.Depth == 0 ? null : new ulong[addArchetype.Depth];
        }

        public void Clear()
        {
            if (HasDataMask == null) return;
            for (int i = 0; i < HasDataMask.Length; i++) HasDataMask[i] = 0;
        }

        public void Set<T>(T component) where T : unmanaged
        {
            var componentType = ComponentRegistry.Get<T>();
            if (componentType.ZeroSize) return;
            var id = componentType.Id;
            if (!_dataOffsets.TryGetValue(id, out var offset))
            {
                throw new Exception($"This layout does not have an Add action for '{typeof(T)}'.");
            }

            fixed (byte* ptr = ComponentData)
            {
                var destination = (T*)(ptr + offset);
                *destination = component;
            }

            if (HasDataMask == null) return;

            var index = id / 64;
            var relMask = 1ul << (id % 64);
            HasDataMask[index] |= relMask;
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
