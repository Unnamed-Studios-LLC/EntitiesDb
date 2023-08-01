using System;

namespace EntitiesDb;

public sealed class ReinterpretSizeException : Exception
{
    public ReinterpretSizeException(Type fromType, Type toType) : base($"Cannot reinterpret from {fromType} to {toType}. Types do not have equal size.")
    {
        FromType = fromType;
        ToType = toType;
    }

    public Type FromType { get; }
    public Type ToType { get; }
}
