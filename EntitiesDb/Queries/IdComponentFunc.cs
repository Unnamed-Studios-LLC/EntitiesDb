namespace EntitiesDb;

public delegate void IdComponentFunc(
        uint entityId
    )
    ;

public delegate void IdComponentFunc<T1>(
        uint entityId,
        ref T1 componentA
    )
    where T1 : unmanaged
    ;

public delegate void IdComponentFunc<T1, T2>(
        uint entityId,
        ref T1 componentA,
        ref T2 componentB
    )
    where T1 : unmanaged
    where T2 : unmanaged
    ;

public delegate void IdComponentFunc<T1, T2, T3>(
        uint entityId,
        ref T1 componentA,
        ref T2 componentB,
        ref T3 componentC
    )
    where T1 : unmanaged
    where T2 : unmanaged
    where T3 : unmanaged
    ;

public delegate void IdComponentFunc<T1, T2, T3, T4>(
        uint entityId,
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

public delegate void IdComponentFunc<T1, T2, T3, T4, T5>(
        uint entityId,
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

public delegate void IdComponentFunc<T1, T2, T3, T4, T5, T6>(
        uint entityId,
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
