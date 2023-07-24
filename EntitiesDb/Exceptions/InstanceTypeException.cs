using System;

namespace EntitiesDb
{
    public sealed class InstanceTypeException : Exception
    {
        public InstanceTypeException(Type expectedType, object value) : base($"Incorrect instance type, expectecd type: {expectedType}")
        {
            ExpectedType = expectedType;
            Value = value;
        }

        public Type ExpectedType { get; }
        public object Value { get; }
    }
}
