namespace EntitiesDb
{
    internal readonly struct EntityReference
	{
		public readonly Archetype Archetype;
		public readonly EntityIndex Indices;

        public EntityReference(Archetype archetype, EntityIndex index)
        {
            Archetype = archetype;
            Indices = index;
        }

        public Chunk GetChunk() => Archetype.GetChunk(Indices.ChunkIndex);
    }
}

