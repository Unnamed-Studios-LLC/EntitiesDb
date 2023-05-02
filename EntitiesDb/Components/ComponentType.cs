using EntitiesDb.Events;

namespace EntitiesDb.Components
{
    internal struct ComponentType
    {
        public int Id;
        public int Size;
        public EventPublisher OnAdd;
        public EventPublisher OnRemove;

        public bool ZeroSize => Size == 0;
    }
}
