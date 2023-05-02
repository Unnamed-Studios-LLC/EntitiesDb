using EntitiesDb.Events;

namespace EntitiesDb
{
    public partial class EntityDatabase
    {
        public void Subscribe<T>(Event @event, ComponentHandler<T> handler) where T : unmanaged
        {
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
    }
}
