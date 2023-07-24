namespace EntitiesDb
{
    internal abstract class ComponentEvents
	{

	}

	internal sealed class ComponentEvents<T> : ComponentEvents where T : unmanaged
    {
        public event ComponentHandler<T> OnAdd;
        public event ComponentHandler<T> OnRemove;

        public unsafe void OnAddComponent(uint entityId, ref T component)
        {
            OnAdd?.Invoke(entityId, ref component);
        }

        public unsafe void OnRemoveComponent(uint entityId, ref T component)
        {
            OnRemove?.Invoke(entityId, ref component);
        }
    }
}

