using System;

namespace EntitiesDb
{
	public sealed class EntityMaxException : Exception
	{
		public EntityMaxException(int maxCount) : base($"Maximum amount of entities reached, max: {maxCount}")
		{
			MaxCount = maxCount;
		}

		public int MaxCount { get; }
	}
}

