using System;

namespace EntitiesDb
{
	public sealed class EntityConflictException : Exception
	{
		public EntityConflictException(uint entityId) : base($"Entity id conflict occurred for id: {entityId}")
		{
			EntityId = entityId;
		}

		public uint EntityId { get; }
	}
}

