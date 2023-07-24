using System;

namespace EntitiesDb
{
    public partial class EntityDatabase
    {
        public void AddComponentEvent<T>(EventAction eventAction, ComponentHandler<T> handler) where T : unmanaged
        {
            _eventDispatcher.AddComponentEvent(eventAction, handler);
        }

        public void AddEntityEvent(EventAction eventAction, EntityHandler handler)
        {
            if (handler is null)
            {
                throw new ArgumentNullException(nameof(handler));
            }

            switch (eventAction)
            {
                case EventAction.Add:
                    _eventDispatcher.OnAdd += handler;
                    break;
                case EventAction.Remove:
                    _eventDispatcher.OnRemove += handler;
                    break;
            }
        }

        public void RemoveComponentEvent<T>(EventAction eventAction, ComponentHandler<T> handler) where T : unmanaged
        {
            _eventDispatcher.RemoveComponentEvent(eventAction, handler);
        }

        public void RemoveEntityEvent(EventAction eventAction, EntityHandler handler)
        {
            if (handler is null)
            {
                throw new ArgumentNullException(nameof(handler));
            }

            switch (eventAction)
            {
                case EventAction.Add:
                    _eventDispatcher.OnAdd -= handler;
                    break;
                case EventAction.Remove:
                    _eventDispatcher.OnRemove -= handler;
                    break;
            }
        }
    }
}
