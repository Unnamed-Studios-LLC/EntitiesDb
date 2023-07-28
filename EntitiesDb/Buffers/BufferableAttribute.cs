using System;

namespace EntitiesDb;

[AttributeUsage(AttributeTargets.Struct)]
public sealed class BufferableAttribute : Attribute
{
    public BufferableAttribute(int internalCapacity)
    {
        InternalCapacity = internalCapacity;
    }

    public int InternalCapacity { get; }
}
