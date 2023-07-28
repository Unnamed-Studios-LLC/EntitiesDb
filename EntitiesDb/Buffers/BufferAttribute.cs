using System;

namespace EntitiesDb;

[AttributeUsage(AttributeTargets.Struct)]
public sealed class BufferAttribute : Attribute
{
    public BufferAttribute(int internalCapacity)
    {
        InternalCapacity = internalCapacity;
    }

    public int InternalCapacity { get; }
}
