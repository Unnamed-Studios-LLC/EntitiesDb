using System.Runtime.CompilerServices;

namespace EntitiesDb.Components
{
    public unsafe abstract class ComponentMetaData
    {
        public Type Type;
        public int Size;

        internal EntityLayout AddLayout;
        internal EntityLayout RemoveLayout;

        protected ComponentMetaData(Type type, int size, EntityLayout addLayout, EntityLayout removeLayout)
        {
            Type = type;
            Size = size;
            AddLayout = addLayout;
            RemoveLayout = removeLayout;
        }

        public bool ZeroSize => Size == 0;

        public abstract object? GetComponent(EntityDatabase entityDatabase, uint entityId);

        public abstract void OnAdd(EntityDatabase entityDatabase, uint entityId, void* component);
        public abstract void OnRemove(EntityDatabase entityDatabase, uint entityId, void* component);

        public abstract void SetComponent(EntityDatabase entityDatabase, uint entityId, object? value);
    }

    public unsafe sealed class ComponentMetaData<T> : ComponentMetaData where T : unmanaged
    {
        public static readonly ComponentMetaData Cached = new ComponentMetaData<T>();

        internal ComponentMetaData() : base(typeof(T), sizeof(T), CreateAddLayout(), CreateRemoveLayout())
        {

        }

        internal static EntityLayout CreateAddLayout() => new EntityLayout().Add<T>();
        internal static EntityLayout CreateRemoveLayout() => new EntityLayout().Remove<T>();

        public override object? GetComponent(EntityDatabase entityDatabase, uint entityId) => entityDatabase.GetComponent<T>(entityId);

        public override unsafe void OnAdd(EntityDatabase entityDatabase, uint entityId, void* component) => entityDatabase.PublishAddEvent(entityId, ref Unsafe.AsRef<T>(component));
        public override unsafe void OnRemove(EntityDatabase entityDatabase, uint entityId, void* component) => entityDatabase.PublishRemoveEvent(entityId, ref Unsafe.AsRef<T>(component));

        public override void SetComponent(EntityDatabase entityDatabase, uint entityId, object? value)
        {
            if (value is not T componentValue) throw new Exception($"Cannot set component value, invalid component type.");
            ref var component = ref entityDatabase.GetComponent<T>(entityId);
            component = componentValue;
        }
    }
}
