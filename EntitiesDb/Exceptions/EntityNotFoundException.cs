using System;

namespace EntitiesDb
{
	public sealed class EntityNotFoundException : Exception
	{
		public EntityNotFoundException(uint entityId) : base($"Entity not found for id: {entityId}")
		{
			EntityId = entityId;
		}

		public uint EntityId { get; }
	}
}

