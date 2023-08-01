using System;

namespace EntitiesDb;

public sealed class ZeroSizeBufferException : Exception
{
    public ZeroSizeBufferException(Type componentType) : base($"Zero-size components cannot be used in buffers, invalid component type: {componentType}")
    {
        ComponentType = componentType;
    }

    public Type ComponentType { get; }
}

