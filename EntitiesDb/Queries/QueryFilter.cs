using EntitiesDb.Mapping;

namespace EntitiesDb.Queries
{
    internal struct QueryFilter
    {
        public Archetype Any;
        public Archetype No;
        public Archetype With;

        public bool Contains(in Archetype archetype)
        {
            return archetype.ContainsAny(in Any) &&
                !archetype.ContainsAny(in No) &&
                archetype.ContainsAll(in With);
        }
    }
}
