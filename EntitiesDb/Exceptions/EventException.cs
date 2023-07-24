using System;

namespace EntitiesDb
{
	public sealed class EventException : Exception
	{
		public EventException(Exception innerException) : base("An exception occurred during an event", innerException)
		{
		}
	}
}

