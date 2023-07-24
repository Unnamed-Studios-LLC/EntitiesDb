using System;

namespace EntitiesDb
{
	public sealed class ReadOnlyException : Exception
	{
		public ReadOnlyException() : base("EntityDatabase is currently read-only, entities and components cannot be added or removed")
		{
		}
	}
}

