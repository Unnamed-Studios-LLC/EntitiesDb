using System;

namespace EntitiesDb;

[AttributeUsage(AttributeTargets.Struct)]
public sealed class BufferableAttribute : Attribute
{
    /// <summary>
    /// Marks a components as bufferable with an internal capacity.
    /// </summary>
    /// <param name="internalCapacity">The amount of components stored internally before allocating heap memory.</param>
    public BufferableAttribute(int internalCapacity)
    {
        InternalCapacity = internalCapacity;
    }

    /// <summary>
    /// The amount of components stored internally before allocating heap memory.
    /// </summary>
    public int InternalCapacity { get; }
}
