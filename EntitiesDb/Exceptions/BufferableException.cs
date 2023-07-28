using System;

namespace EntitiesDb;

public sealed class BufferableException : Exception
{
    public BufferableException(Type componentType) : base($"Missing or invalid {nameof(BufferableAttribute)} on component type: {componentType}")
    {
        ComponentType = componentType;
    }

    public Type ComponentType { get; }
}
