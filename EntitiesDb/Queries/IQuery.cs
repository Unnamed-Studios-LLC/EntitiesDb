using EntitiesDb.Data;

namespace EntitiesDb.Queries
{
    internal interface IQuery
    {
        void CopyIndices(EntityGroup group, Span<int> indices);
        void EnumerateChunk(in EnumerationJob job, Span<int> indices);
        IEnumerable<int> GetRequiredIds();
    }
}
