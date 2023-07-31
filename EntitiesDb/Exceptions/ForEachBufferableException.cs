using System;

namespace EntitiesDb;

public sealed class ForEachBufferableException : Exception
{
	public ForEachBufferableException(Type componentType) : base($"Bufferable types cannot be used outside of ComponentBuffer<T> as ForEach ref parameter, component type: {componentType}")
	{
        ComponentType = componentType;
    }

    public Type ComponentType { get; }
}

