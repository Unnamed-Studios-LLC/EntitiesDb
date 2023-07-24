namespace EntitiesDb
{
	internal readonly struct EntityIndex
	{
		public readonly int ChunkIndex;
		public readonly int ListIndex;

        public EntityIndex(int chunkIndex, int listIndex)
        {
            ChunkIndex = chunkIndex;
            ListIndex = listIndex;
        }
    }
}

