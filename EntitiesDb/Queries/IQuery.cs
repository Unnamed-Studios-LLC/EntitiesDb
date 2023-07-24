using System.Collections.Generic;

namespace EntitiesDb
{
    internal interface IQuery
    {
        void EnumerateChunk(in EnumerationJob job);
        IEnumerable<ComponentMetaData> GetDelegateMetaData();
    }
}