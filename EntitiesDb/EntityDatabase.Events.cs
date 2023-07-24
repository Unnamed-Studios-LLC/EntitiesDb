using System;

namespace EntitiesDb
{
    public partial class EntityDatabase
    {
        public void AddComponentEvent<T>(Event eventAction, ComponentHandler<T> handler) where T : unmanaged
        {
            _eventDispatcher.AddComponentEvent(eventAction, handler);
        }

        public void AddEntityEvent(Event eventAction, EntityHandler handler)
        {
            if (handler is null)
            {
                throw new ArgumentNullException(nameof(handler));
            }

            switch (eventAction)
            {
                case Event.OnAdd:
                    _eventDispatcher.OnAdd += handler;
                    break;
                case Event.OnRemove:
                    _eventDispatcher.OnRemove += handler;
                    break;
            }
        }

        public void RemoveComponentEvent<T>(Event eventAction, ComponentHandler<T> handler) where T : unmanaged
        {
            _eventDispatcher.RemoveComponentEvent(eventAction, handler);
        }

        public void RemoveEntityEvent(Event eventAction, EntityHandler handler)
        {
            if (handler is null)
            {
                throw new ArgumentNullException(nameof(handler));
            }

            switch (eventAction)
            {
                case Event.OnAdd:
                    _eventDispatcher.OnAdd -= handler;
                    break;
                case Event.OnRemove:
                    _eventDispatcher.OnRemove -= handler;
                    break;
            }
        }
    }
}
