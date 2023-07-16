namespace EntitiesDb.Components
{
    public struct ComponentType
    {
        public Type Type;
        public int Id;
        public int Size;
        internal Func<EntityDatabase, uint, object> Getter;
        internal Action<EntityDatabase, uint, object> Setter;
        internal EventPublisher OnAdd;
        internal EventPublisher OnRemove;
        internal EntityLayout AddLayout;
        internal EntityLayout RemoveLayout;
        internal Action Clearer;

        public bool ZeroSize => Size == 0;
    }
}
