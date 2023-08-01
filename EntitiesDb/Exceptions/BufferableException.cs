using System;

namespace EntitiesDb;

public sealed class BufferableException : Exception
{
	public BufferableException(Type componentType) : base($"Component is bufferable and must be added or removed using buffer methods, component type: {componentType}")
	{
        ComponentType = componentType;
    }

    public Type ComponentType { get; }
}

