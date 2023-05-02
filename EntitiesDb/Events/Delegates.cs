using EntitiesDb.Events;

namespace EntitiesDb
{
    public unsafe delegate void ComponentHandler<T>(uint entityId, ref T component) where T : unmanaged;

    internal unsafe delegate void EventPublisher(EventDispatcher dispatcher, uint entityId, void* component);
}
