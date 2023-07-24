namespace EntitiesDb
{
    public delegate void ComponentHandler<T>(uint entityId, ref T component) where T : unmanaged;
    public delegate void EntityHandler(uint entityId);
}

