using System;
using System.Collections.Generic;

namespace EntitiesDb
{
	internal sealed class EventDispatcher
    {
        public event EntityHandler OnAdd;
        public event EntityHandler OnRemove;

        private readonly Dictionary<Type, ComponentEvents> _componentEvents = new();

        public void Clear()
        {
            OnAdd = null;
            OnRemove = null;
            _componentEvents.Clear();
        }

        public void OnAddComponent<T>(uint entityId, ref T component) where T : unmanaged
        {
            if (!_componentEvents.TryGetValue(typeof(T), out var events) ||
                events is not ComponentEvents<T> typedEvents) return;
            typedEvents.OnAddComponent(entityId, ref component);
        }

        public void OnAddEntity(uint entityId)
        {
            OnAdd?.Invoke(entityId);
        }

        public void OnRemoveComponent<T>(uint entityId, ref T component) where T : unmanaged
        {
            if (!_componentEvents.TryGetValue(typeof(T), out var events) ||
                events is not ComponentEvents<T> typedEvents) return;
            typedEvents.OnRemoveComponent(entityId, ref component);
        }

        public void OnRemoveEntity(uint entityId)
        {
            OnRemove?.Invoke(entityId);
        }

        public void AddComponentEvent<T>(EventAction eventAction, ComponentHandler<T> handler) where T : unmanaged
        {
            if (handler is null) throw new ArgumentNullException(nameof(handler));
            var componentEvents = GetComponentEvents<T>();
            switch (eventAction)
            {
                case EventAction.Add:
                    componentEvents.OnAdd += handler;
                    break;
                case EventAction.Remove:
                    componentEvents.OnRemove += handler;
                    break;
            }
        }

        public void RemoveComponentEvent<T>(EventAction eventAction, ComponentHandler<T> handler) where T : unmanaged
        {
            if (handler is null) throw new ArgumentNullException(nameof(handler));
            var componentEvents = GetComponentEvents<T>();
            switch (eventAction)
            {
                case EventAction.Add:
                    componentEvents.OnAdd -= handler;
                    break;
                case EventAction.Remove:
                    componentEvents.OnRemove -= handler;
                    break;
            }
        }

        private ComponentEvents<T> GetComponentEvents<T>() where T : unmanaged
        {
            if (_componentEvents.TryGetValue(typeof(T), out var events))
            {
                return (ComponentEvents<T>)events;
            }

            var typedEvents = new ComponentEvents<T>();
            _componentEvents.Add(typeof(T), typedEvents);
            return typedEvents;
        }
    }
}

