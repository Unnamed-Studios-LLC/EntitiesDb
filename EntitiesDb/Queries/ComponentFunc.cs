namespace EntitiesDb
{
    public delegate void ComponentFunc<T1>(
            ref T1 componentA
        )
        where T1 : unmanaged
        ;

    public delegate void ComponentFunc<T1, T2>(
            ref T1 componentA,
            ref T2 componentB
        )
        where T1 : unmanaged
        where T2 : unmanaged
        ;

    public delegate void ComponentFunc<T1, T2, T3>(
            ref T1 componentA,
            ref T2 componentB,
            ref T3 componentC
        )
        where T1 : unmanaged
        where T2 : unmanaged
        where T3 : unmanaged
        ;

    public delegate void ComponentFunc<T1, T2, T3, T4>(
            ref T1 componentA,
            ref T2 componentB,
            ref T3 componentC,
            ref T4 componentD
        )
        where T1 : unmanaged
        where T2 : unmanaged
        where T3 : unmanaged
        where T4 : unmanaged
        ;

    public delegate void ComponentFunc<T1, T2, T3, T4, T5>(
            ref T1 componentA,
            ref T2 componentB,
            ref T3 componentC,
            ref T4 componentD,
            ref T5 componentE
        )
        where T1 : unmanaged
        where T2 : unmanaged
        where T3 : unmanaged
        where T4 : unmanaged
        where T5 : unmanaged
        ;

    public delegate void ComponentFunc<T1, T2, T3, T4, T5, T6>(
            ref T1 componentA,
            ref T2 componentB,
            ref T3 componentC,
            ref T4 componentD,
            ref T5 componentE,
            ref T6 componentF
        )
        where T1 : unmanaged
        where T2 : unmanaged
        where T3 : unmanaged
        where T4 : unmanaged
        where T5 : unmanaged
        where T6 : unmanaged
        ;

    public delegate void ComponentStateFunc<T1, TState>(
            ref T1 componentA,
            ref TState state
        )
        where T1 : unmanaged
        ;

    public delegate void ComponentStateFunc<T1, T2, TState>(
            ref T1 componentA,
            ref T2 componentB,
            ref TState state
        )
        where T1 : unmanaged
        where T2 : unmanaged
        ;

    public delegate void ComponentStateFunc<T1, T2, T3, TState>(
            ref T1 componentA,
            ref T2 componentB,
            ref T3 componentC,
            ref TState state
        )
        where T1 : unmanaged
        where T2 : unmanaged
        where T3 : unmanaged
        ;

    public delegate void ComponentStateFunc<T1, T2, T3, T4, TState>(
            ref T1 componentA,
            ref T2 componentB,
            ref T3 componentC,
            ref T4 componentD,
            ref TState state
        )
        where T1 : unmanaged
        where T2 : unmanaged
        where T3 : unmanaged
        where T4 : unmanaged
        ;

    public delegate void ComponentStateFunc<T1, T2, T3, T4, T5, TState>(
            ref T1 componentA,
            ref T2 componentB,
            ref T3 componentC,
            ref T4 componentD,
            ref T5 componentE,
            ref TState state
        )
        where T1 : unmanaged
        where T2 : unmanaged
        where T3 : unmanaged
        where T4 : unmanaged
        where T5 : unmanaged
        ;

    public delegate void ComponentStateFunc<T1, T2, T3, T4, T5, T6, TState>(
            ref T1 componentA,
            ref T2 componentB,
            ref T3 componentC,
            ref T4 componentD,
            ref T5 componentE,
            ref T6 componentF,
            ref TState state
        )
        where T1 : unmanaged
        where T2 : unmanaged
        where T3 : unmanaged
        where T4 : unmanaged
        where T5 : unmanaged
        where T6 : unmanaged
        ;
}