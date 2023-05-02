using EntitiesDb.Events;

namespace EntitiesDb
{
    public partial class EntityDatabase
    {
        public void Subscribe<T>(Event @event, ComponentHandler<T> handler) where T : unmanaged
        {
            ThrowIfStructuralChangeBlocked();
            switch (@event)
            {
                case Event.OnAdd:
                    _eventDispatcher.SubscribeOnAdd(handler);
                    break;
                case Event.OnRemove:
                    _eventDispatcher.SubscribeOnRemove(handler);
                    break;
            }
        }

        public void Unsubscribe<T>(Event @event, ComponentHandler<T> handler) where T : unmanaged
        {
            ThrowIfStructuralChangeBlocked();
            switch (@event)
            {
                case Event.OnAdd:
                    _eventDispatcher.UnsubscribeOnAdd(handler);
                    break;
                case Event.OnRemove:
                    _eventDispatcher.UnsubscribeOnRemove(handler);
                    break;
            }
        }

        internal void PublishAddEvent<T>(uint entityId, ref T component) where T : unmanaged
        {
            _inEvent = true;
            try
            {
                _eventDispatcher.PublishAdd(entityId, ref component);
            }
            catch (Exception e)
            {
                _eventExceptions.Add(e);
            }
            _inEvent = false;
        }

        internal void PublishRemoveEvent<T>(uint entityId, ref T component) where T : unmanaged
        {
            _inEvent = true;
            try
            {
                _eventDispatcher.PublishRemove(entityId, ref component);
            }
            catch (Exception e)
            {
                _eventExceptions.Add(e);
            }
            _inEvent = false;
        }
    }
}
