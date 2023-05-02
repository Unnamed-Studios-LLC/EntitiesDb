using EntitiesDb.Components;
using EntitiesDb.Mapping;

namespace EntitiesDb
{
    public class EntityLayoutBuilder
    {
        private readonly HashSet<int> _addedIds = new();
        private readonly HashSet<int> _removedIds = new();

        internal EntityLayoutBuilder() { }

        public static EntityLayoutBuilder Create() => new();

        public EntityLayoutBuilder Add<T>() where T : unmanaged
        {
            var id = ComponentRegistry.Type<T>.Id;
            _removedIds.Remove(id);
            _addedIds.Add(id);
            return this;
        }

        public EntityLayout Build()
        {
            var addArchetype = Archetype.FromIds(_addedIds);
            var removeArchetype = Archetype.FromIds(_removedIds);
            return new EntityLayout(addArchetype, removeArchetype);
        }

        public EntityLayoutBuilder Remove<T>() where T : unmanaged
        {
            var id = ComponentRegistry.Type<T>.Id;
            _addedIds.Remove(id);
            _removedIds.Add(id);
            return this;
        }
    }
}
