using System;

namespace EntitiesDb;

public sealed class InvalidBufferableException : Exception
{
    public InvalidBufferableException(Type componentType) : base($"Missing or invalid {nameof(BufferableAttribute)} on component type: {componentType}")
    {
        ComponentType = componentType;
    }

    public Type ComponentType { get; }
}
