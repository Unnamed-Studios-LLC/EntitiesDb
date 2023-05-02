using EntitiesDb.Events;
using System.Runtime.InteropServices;
using EntitiesDb.Components;

namespace EntitiesDb.Events
{
    internal sealed class EventDispatcher
    {
        private readonly Dictionary<int, EventSubscription> _subscriptions = new();

        public void Clear()
        {
            _subscriptions.Clear();
        }

        public void PublishAdd<T>(uint entityId, ref T component) where T : unmanaged
        {
            if (!_subscriptions.TryGetValue(ComponentRegistry.Type<T>.Id, out var subscription) ||
                subscription is not EventSubscription<T> componentSubscription) return;
            componentSubscription.PublishOnAdd(entityId, ref component);
        }

        public void PublishRemove<T>(uint entityId, ref T component) where T : unmanaged
        {
            if (!_subscriptions.TryGetValue(ComponentRegistry.Type<T>.Id, out var subscription) ||
                subscription is not EventSubscription<T> componentSubscription) return;
            componentSubscription.PublishOnRemove(entityId, ref component);
        }

        public void SubscribeOnAdd<T>(ComponentHandler<T> handler) where T : unmanaged
        {
            if (handler is null) throw new ArgumentNullException(nameof(handler));
            var subscription = GetSubscription<T>();
            subscription.OnAdd += handler;
        }

        public void SubscribeOnRemove<T>(ComponentHandler<T> handler) where T : unmanaged
        {
            if (handler is null) throw new ArgumentNullException(nameof(handler));
            var subscription = GetSubscription<T>();
            subscription.OnRemove += handler;
        }

        public void UnsubscribeOnAdd<T>(ComponentHandler<T> handler) where T : unmanaged
        {
            if (handler is null) throw new ArgumentNullException(nameof(handler));
            var subscription = GetSubscription<T>();
            subscription.OnAdd -= handler;
        }

        public void UnsubscribeOnRemove<T>(ComponentHandler<T> handler) where T : unmanaged
        {
            if (handler is null) throw new ArgumentNullException(nameof(handler));
            var subscription = GetSubscription<T>();
            subscription.OnRemove -= handler;
        }

        private EventSubscription<T> GetSubscription<T>() where T : unmanaged
        {
            EventSubscription<T> componentSubscription;
            ref var subscription = ref CollectionsMarshal.GetValueRefOrAddDefault(_subscriptions, ComponentRegistry.Type<T>.Id, out var exists);
            if (!exists)
            {
                componentSubscription = new EventSubscription<T>();
                subscription = componentSubscription;
            }
            else componentSubscription = (EventSubscription<T>)subscription!;
            return componentSubscription;
        }
    }
}
