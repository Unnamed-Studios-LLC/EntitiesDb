using System;

namespace EntitiesDb;

public sealed class CodeGenerationException : Exception
{
    public CodeGenerationException() : base("Code not generated for this callsite!")
    {

    }
}
