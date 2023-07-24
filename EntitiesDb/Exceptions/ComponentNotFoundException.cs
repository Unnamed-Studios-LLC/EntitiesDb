using System;

namespace EntitiesDb
{
	public sealed class ComponentNotFoundException : Exception
	{
		public ComponentNotFoundException(uint entityId, Type componentType) : base($"Component {componentType} not found for entity id: {entityId}")
		{
		}

		public uint EntityId { get; }
		public Type ComponentType { get; }
	}
}

