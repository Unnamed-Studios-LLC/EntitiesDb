using EntitiesDb.Components;

namespace EntitiesDb
{
    public sealed class ComponentType
    {
        internal ComponentType(int id, ComponentMetaData metaData)
        {
            Id = id;
            MetaData = metaData;
        }

        public int Id { get; }
        public ComponentMetaData MetaData { get; }
    }
}
