namespace EntitiesDb;

public delegate void EntityFunc(
        in Entity entity
    )
    ;

public delegate void EntityStateFunc<TState>(
        in Entity entity,
        TState state
    )
    ;
