using System;

namespace EntitiesDb;

public sealed class ForEachBufferableException : Exception
{
	public ForEachBufferableException(Type componentType) : base($"Components marked with Bufferable attribute must be wrapped by ComponentBuffer<T> in ForEach delegates, component type: {componentType}")
	{
        ComponentType = componentType;
    }

    public Type ComponentType { get; }
}

