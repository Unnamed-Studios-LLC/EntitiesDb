namespace EntitiesDb;

public interface IQuery
{
    void EnumerateChunk(in EnumerationJob job);
}