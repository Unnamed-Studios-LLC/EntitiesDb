using System;

namespace EntitiesDb;

public sealed class BufferableException : Exception
{
	public BufferableException(Type componentType) : base($"Component is bufferable and must be added as a buffer, component type: {componentType}")
	{
        ComponentType = componentType;
    }

    public Type ComponentType { get; }
}

