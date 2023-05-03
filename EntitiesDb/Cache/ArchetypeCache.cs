using EntitiesDb.Mapping;

namespace EntitiesDb.Cache
{
    internal static class ArchetypeCache
    {
        private readonly static List<Stack<Archetype>> s_archetypeCache = new();

        static ArchetypeCache()
        {
            // prepopulate
            for (int i = 1; i <= 4; i++) s_archetypeCache.Add(new Stack<Archetype>());
        }

        public static Archetype Rent(int depth)
        {
            if (depth == 0) return default;
            lock (s_archetypeCache)
            {
                while (depth >= s_archetypeCache.Count) s_archetypeCache.Add(new Stack<Archetype>());
                var cache = s_archetypeCache[depth];
                if (!cache.TryPop(out var result)) result = new(new ulong[depth]);
                return result;
            }
        }

        public static void Return(Archetype archetype)
        {
            if (archetype.Depth == 0) return;
            lock (s_archetypeCache)
            {
                while (archetype.Depth >= s_archetypeCache.Count) s_archetypeCache.Add(new Stack<Archetype>());
                var cache = s_archetypeCache[archetype.Depth];
                archetype.Clear();
                cache.Push(archetype);
            }
        }
    }
}
