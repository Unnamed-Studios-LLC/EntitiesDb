using System;
using System.Collections.Generic;

namespace EntitiesDb;

public interface IQueryEnumerator
{
    void EnumerateChunk(in EnumerationJob job);
}