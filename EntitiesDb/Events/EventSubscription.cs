using System.Runtime.CompilerServices;

namespace EntitiesDb.Events
{
    internal unsafe abstract class EventSubscription { }
    internal unsafe sealed class EventSubscription<T> : EventSubscription where T : unmanaged
    {
        public event ComponentHandler<T>? OnAdd;
        public event ComponentHandler<T>? OnRemove;

        public unsafe void PublishOnAdd(uint entityId, ref T component)
        {
            OnAdd?.Invoke(entityId, ref component);
        }

        public unsafe void PublishOnRemove(uint entityId, ref T component)
        {
            OnRemove?.Invoke(entityId, ref component);
        }
    }
}
