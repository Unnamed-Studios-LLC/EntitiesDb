namespace EntitiesDb.Mapping
{
    internal readonly struct EntityIndex
    {
        public EntityIndex(int chunk, int list)
        {
            Chunk = chunk;
            List = list;
        }

        public int Chunk { get; }
        public int List { get; }
    }
}
