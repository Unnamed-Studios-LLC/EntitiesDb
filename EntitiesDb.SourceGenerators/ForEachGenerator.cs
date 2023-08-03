using System.Collections.Immutable;
using System.Text;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace EntitiesDb.SourceGenerators;

[Generator]
internal sealed class ForEachGenerator : IIncrementalGenerator
{
    public void Initialize(IncrementalGeneratorInitializationContext context)
    {
        var callsites = context.SyntaxProvider.CreateSyntaxProvider(FindNodes, ProcessNodes)
            .Where(x => x != null)
            .WithComparer(SymbolEqualityComparer.Default);
        context.RegisterSourceOutput(callsites.Collect(), Generate!);
    }

    private static bool FindNodes(SyntaxNode syntaxNode, CancellationToken cancellationToken)
    {
        return syntaxNode switch
        {
            InvocationExpressionSyntax => true,
            _ => false
        };
    }

    private static void Generate(SourceProductionContext context, ImmutableArray<IMethodSymbol> methods)
    {
        var uniqueMethods = methods.ToImmutableHashSet<IMethodSymbol>(SymbolEqualityComparer.Default);
        var stringBuilder = new StringBuilder();
        foreach (var method in uniqueMethods)
        {
            if (context.CancellationToken.IsCancellationRequested) return;
            try
            {
                var source = ForEachTemplates.Create(method, stringBuilder, out var uniqueName);
                if (context.CancellationToken.IsCancellationRequested) return;
                var fileName = $"{uniqueName}.g";
                context.AddSource(fileName, source);
            }
            catch
            {
                continue;
            }
        }
    }

    private static IMethodSymbol? GetMethodSymbol(GeneratorSyntaxContext syntaxContext, CancellationToken cancellationToken)
    {
        if (syntaxContext.Node is not InvocationExpressionSyntax invocationExpressionSyntax) return null;
        var symbolInfo = syntaxContext.SemanticModel.GetSymbolInfo(invocationExpressionSyntax.Expression);
        var symbol = symbolInfo.Symbol;
        if (symbol == null ||
            symbol is not IMethodSymbol methodSymbol) return null;

        // method name check
        if (!methodSymbol.Name.Equals(Names.ForEachName, StringComparison.Ordinal)) return null;

        // type check
        var containingType = methodSymbol.ContainingType;
        if (containingType == null ||
            !containingType.Name.Equals(Names.ForEachExtensionsName, StringComparison.Ordinal)) return null;

        // namespace check
        var containingNamespace = methodSymbol.ContainingNamespace;
        if (containingNamespace == null ||
            !containingNamespace.Name.Equals(Names.EntitiesDbName, StringComparison.Ordinal)) return null;

        return methodSymbol;
    }

    private static IMethodSymbol? ProcessNodes(GeneratorSyntaxContext syntaxContext, CancellationToken cancellationToken) => syntaxContext.Node switch
    {
        InvocationExpressionSyntax => GetMethodSymbol(syntaxContext, cancellationToken),
        _ => null
    };
}


