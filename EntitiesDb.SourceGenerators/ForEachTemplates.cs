using Microsoft.CodeAnalysis;
using System.Diagnostics;
using System.Text;

namespace EntitiesDb.SourceGenerators;

internal static class ForEachTemplates
{
    public static string Create(IMethodSymbol method, StringBuilder stringBuilder, out string uniqueName)
    {
        if (!Debugger.IsAttached)
        {
            //Debugger.Launch();
        }

        ParseParameters(method, out var queryableType, out var funcParameter, out var stateParameter);

        uniqueName = GetUniqueName(stringBuilder, method, funcParameter, queryableType);
        var inQueryFilter = queryableType.Name.Equals(Names.QueryFilterName, StringComparison.Ordinal);
        var queryName = GetQueryName(stringBuilder, uniqueName);
        var queryArguments = GetQueryArguments(stringBuilder, funcParameter, stateParameter);
        var extensionParameter = GetExtensionParameter(stringBuilder, queryableType);
        var extensionArgument = GetExtensionArgument(stringBuilder, queryableType);
        var parameters = GetParameters(stringBuilder, funcParameter, stateParameter);
        var fields = GetFields(stringBuilder, funcParameter, stateParameter);
        var fieldSetters = GetFieldSetters(stringBuilder, funcParameter, stateParameter);
        var enumeration = GetEnumeration(stringBuilder, funcParameter, stateParameter);
        var assertions = GetAssertions(stringBuilder, funcParameter, stateParameter);

        var source = $@"
using EntitiesDb;

internal static class {uniqueName}Extension
{{
    public static void {method.Name}(this {extensionParameter}, {parameters})
    {{
{(inQueryFilter ? string.Empty : $"        var queryFilter = {extensionArgument}.GetQueryFilter();\n")}{assertions}
        var query = new {queryName}({queryArguments});
        queryFilter.Query(query);
    }}
        
    private struct {queryName} : IQueryEnumerator
    {{
{fields}

        public {queryName}({parameters})
        {{
{fieldSetters}
        }}
        
        public void EnumerateChunk(in EnumerationJob job)
        {{
{enumeration}
        }}
    }}
}}
";
        return source;
    }

    private static void AssertTypeScope(ITypeSymbol type)
    {
        if (type.DeclaredAccessibility != Accessibility.Public &&
            type.DeclaredAccessibility != Accessibility.Internal)
        {
            throw new Exception("Only Public and Internal class may be used in queries.");
        }
    }

    private static string GetAssertions(StringBuilder stringBuilder, IParameterSymbol funcParameter, IParameterSymbol? stateParameter)
    {
        ParseDelegateParameters(funcParameter, out _, out _, out var componentParameters);

        stringBuilder.Clear();
        foreach (var componentParameter in componentParameters)
        {
            var parameterType = componentParameter.Type as INamedTypeSymbol ?? throw new Exception();
            var isBuffer = IsComponentBufferType(parameterType);
            var componentType = isBuffer ? parameterType.TypeArguments[0] : componentParameter.Type;
            stringBuilder.AppendLine($"        QueryFilter.AssertDelegateComponent<{componentType}>({GetTrueFalse(isBuffer)});");
            stringBuilder.AppendLine($"        queryFilter.WithTypes.Add(typeof({componentType}));");
        }
        return stringBuilder.ToString();
    }

    private static string GetExtensionParameter(StringBuilder stringBuilder, ITypeSymbol queryableType)
    {
        stringBuilder.Clear();
        stringBuilder.Append(queryableType);
        stringBuilder.Append(' ');
        stringBuilder.Append(queryableType.Name);

        var indexToLower = stringBuilder.Length - queryableType.Name.Length;
        stringBuilder[indexToLower] = char.ToLower(stringBuilder[indexToLower]);

        return stringBuilder.ToString();
    }

    private static string GetExtensionArgument(StringBuilder stringBuilder, ITypeSymbol queryableType)
    {
        stringBuilder.Clear();
        stringBuilder.Append(queryableType.Name);
        var indexToLower = stringBuilder.Length - queryableType.Name.Length;
        stringBuilder[indexToLower] = char.ToLower(stringBuilder[indexToLower]);
        return stringBuilder.ToString();
    }

    private static string GetFields(StringBuilder stringBuilder, IParameterSymbol funcParameter, IParameterSymbol? stateParameter)
    {
        stringBuilder.Clear();
        int indexToLower;
        if (stateParameter != null)
        {
            stringBuilder.Append("        private ");
            stringBuilder.Append(stateParameter.Type);
            stringBuilder.Append(" _");
            stringBuilder.Append(stateParameter.Name);

            indexToLower = stringBuilder.Length - stateParameter.Name.Length;
            stringBuilder[indexToLower] = char.ToLower(stringBuilder[indexToLower]);
            stringBuilder.Append(';');
            stringBuilder.Append(Environment.NewLine);
        }
        stringBuilder.Append("        private readonly ");
        stringBuilder.Append(funcParameter.Type);
        stringBuilder.Append(" _");
        stringBuilder.Append(funcParameter.Name);

        indexToLower = stringBuilder.Length - funcParameter.Name.Length;
        stringBuilder[indexToLower] = char.ToLower(stringBuilder[indexToLower]);
        stringBuilder.Append(';');
        return stringBuilder.ToString();
    }

    private static string GetFieldSetters(StringBuilder stringBuilder, IParameterSymbol funcParameter, IParameterSymbol? stateParameter)
    {
        stringBuilder.Clear();
        int indexToLower;
        if (stateParameter != null)
        {
            stringBuilder.Append("            _");
            stringBuilder.Append(stateParameter.Name);
            indexToLower = stringBuilder.Length - stateParameter.Name.Length;
            stringBuilder[indexToLower] = char.ToLower(stringBuilder[indexToLower]);

            stringBuilder.Append(" = ");
            stringBuilder.Append(stateParameter.Name);
            indexToLower = stringBuilder.Length - stateParameter.Name.Length;
            stringBuilder[indexToLower] = char.ToLower(stringBuilder[indexToLower]);
            stringBuilder.Append(';');
            stringBuilder.Append(Environment.NewLine);
        }

        stringBuilder.Append("            _");
        stringBuilder.Append(funcParameter.Name);
        indexToLower = stringBuilder.Length - funcParameter.Name.Length;
        stringBuilder[indexToLower] = char.ToLower(stringBuilder[indexToLower]);

        stringBuilder.Append(" = ");
        stringBuilder.Append(funcParameter.Name);
        indexToLower = stringBuilder.Length - funcParameter.Name.Length;
        stringBuilder[indexToLower] = char.ToLower(stringBuilder[indexToLower]);
        stringBuilder.Append(';');
        return stringBuilder.ToString();
    }

    private static string GetParameters(StringBuilder stringBuilder, IParameterSymbol funcParameter, IParameterSymbol? stateParameter)
    {
        stringBuilder.Clear();
        if (stateParameter != null)
        {
            stringBuilder.Append(stateParameter.Type);
            stringBuilder.Append(' ');
            stringBuilder.Append(stateParameter.Name);
            stringBuilder.Append(", ");
        }
        stringBuilder.Append(funcParameter.Type);
        stringBuilder.Append(' ');
        stringBuilder.Append(funcParameter.Name);
        return stringBuilder.ToString();
    }

    private static string GetEnumeration(StringBuilder stringBuilder, IParameterSymbol funcParameter, IParameterSymbol? stateParameter)
    {
        ParseDelegateParameters(funcParameter, out var hasEntityId, out var hasState, out var componentParameters);

        stringBuilder.Clear();
        if (hasEntityId) stringBuilder.AppendLine("            var entityHandle = job.GetEntityIdHandle();");
        int index = 0;
        foreach (var componentParameter in componentParameters)
        {
            var parameterType = componentParameter.Type as INamedTypeSymbol ?? throw new Exception();
            var isBuffer = IsComponentBufferType(parameterType);
            var componentType = isBuffer ? parameterType.TypeArguments[0] : componentParameter.Type;
            if (isBuffer) stringBuilder.AppendLine($"            var componentHandle{++index} = job.GetComponentBufferHandle<{componentType}>();");
            else stringBuilder.AppendLine($"            var componentHandle{++index} = job.GetComponentHandle<{componentType}>();");
        }

        stringBuilder.AppendLine("            for (int i = 0; i < job.Length; i++)");
        stringBuilder.AppendLine("            {");

        // invoke func
        stringBuilder.Append("                _func(");
        if (hasEntityId)
        {
            stringBuilder.Append(Environment.NewLine);
            stringBuilder.Append("                    entityHandle.Value");
        }
        index = 0;
        foreach (var componentParameter in componentParameters)
        {
            var parameterType = componentParameter.Type as INamedTypeSymbol ?? throw new Exception();
            var isBuffer = IsComponentBufferType(parameterType);

            if (index != 0 || hasEntityId) stringBuilder.Append(',');
            stringBuilder.Append(Environment.NewLine);
            if (isBuffer) stringBuilder.Append($"                    ref componentHandle{++index}.Buffer");
            else stringBuilder.Append($"                    ref componentHandle{++index}.AsRef()");
        }
        if (hasState)
        {
            stringBuilder.Append(',');
            stringBuilder.Append(Environment.NewLine);
            stringBuilder.Append("                    ref _state");
        }
        stringBuilder.Append(Environment.NewLine);
        stringBuilder.AppendLine("                );");

        // move next
        if (hasEntityId) stringBuilder.AppendLine("                entityHandle.Next();");
        index = 0;
        foreach (var componentParameter in componentParameters)
        {
            stringBuilder.AppendLine($"                componentHandle{++index}.Next();");
        }
        stringBuilder.Append("            }");

        return stringBuilder.ToString();
    }

    private static string GetQueryName(StringBuilder stringBuilder, string uniqueName)
    {
        stringBuilder.Clear();
        stringBuilder.Append(uniqueName);
        stringBuilder.Append("Query");
        return stringBuilder.ToString();
    }

    private static string GetQueryArguments(StringBuilder stringBuilder, IParameterSymbol funcParameter, IParameterSymbol? stateParameter)
    {
        stringBuilder.Clear();
        if (stateParameter != null)
        {
            stringBuilder.Append(stateParameter.Name);
            stringBuilder.Append(", ");
        }
        stringBuilder.Append(funcParameter.Name);
        return stringBuilder.ToString();
    }

    private static string GetTrueFalse(bool value) => value ? "true" : "false";

    private static string GetUniqueName(StringBuilder stringBuilder, IMethodSymbol method, IParameterSymbol funcParameter, ITypeSymbol queryableType)
    {
        var funcType = (funcParameter.Type as INamedTypeSymbol)!;
        var delegateMethod = funcType.DelegateInvokeMethod!;
        stringBuilder.Clear();
        stringBuilder.Append(queryableType.Name);
        stringBuilder.Append('_');
        stringBuilder.Append(funcType.Name);
        stringBuilder.Append('_');
        foreach (var parameter in delegateMethod.Parameters)
        {
            stringBuilder.Append('_');
            stringBuilder.Append(parameter.Type);
        }
        foreach (var parameter in method.Parameters)
        {
            stringBuilder.Append('_');
            stringBuilder.Append(parameter.Name);
        }
        stringBuilder.Replace(" ", "");
        stringBuilder.Replace('<', '_');
        stringBuilder.Replace('>', '_');
        stringBuilder.Replace(',', '_');
        stringBuilder.Replace('.', '_');
        return stringBuilder.ToString();
    }

    private static bool IsComponentBufferType(INamedTypeSymbol type)
    {
        var isBuffer = type.Name.Equals(Names.ComponentBufferName) &&
            type.OriginalDefinition != null &&
            type.ContainingNamespace.Name.Equals(Names.EntitiesDbName);
        return isBuffer;
    }

    private static void ParseDelegateParameters(IParameterSymbol funcParameter, out bool hasEntityId, out bool hasState, out ReadOnlySpan<IParameterSymbol> componentParameters)
    {
        var funcType = funcParameter.Type as INamedTypeSymbol ?? throw new Exception();
        var delegateMethod = funcType.DelegateInvokeMethod ?? throw new Exception();
        var genericDelegate = delegateMethod.OriginalDefinition ?? throw new Exception();
        var firstParameter = genericDelegate.Parameters.First();
        hasEntityId = firstParameter.Name.Equals("entityId");
        var lastParameter = genericDelegate.Parameters.Last();
        hasState = lastParameter.Name.Equals("state");

        componentParameters = delegateMethod.Parameters.AsSpan();
        foreach (var component in componentParameters) AssertTypeScope(component.Type);

        if (hasEntityId) componentParameters = componentParameters.Slice(1);
        if (hasState) componentParameters = componentParameters.Slice(0, componentParameters.Length - 1);

    }

    private static void ParseParameters(IMethodSymbol method, out ITypeSymbol queryableType, out IParameterSymbol funcParameter, out IParameterSymbol? stateParameter)
    {
        stateParameter = null;
        IParameterSymbol? func = null;
        foreach (var parameter in method.Parameters)
        {
            switch (parameter.Name)
            {
                case "func":
                    func = parameter;
                    break;
                case "state":
                    stateParameter = parameter;
                    break;
            }
        }

        funcParameter = func ?? throw new Exception();
        queryableType = method.ReceiverType ?? throw new Exception();
    }
}
