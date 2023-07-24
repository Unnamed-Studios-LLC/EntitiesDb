using System;

namespace EntitiesDb
{
	public sealed class ForEachTagException : Exception
	{
		public ForEachTagException(Type componentType) : base($"ForEach delegate cannot contain zero-size tag component: {componentType}")
		{
			ComponentType = componentType;
		}

		public Type ComponentType { get; }
	}
}

