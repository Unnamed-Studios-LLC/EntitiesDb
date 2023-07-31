using System.Collections.Immutable;
using System.Diagnostics;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace EntitiesDb.SourceGenerators;

[Generator]
internal sealed class ForEachGenerator : IIncrementalGenerator
{
    public void Initialize(IncrementalGeneratorInitializationContext context)
    {
        var callsites = context.SyntaxProvider.CreateSyntaxProvider(FindNodes, ProcessNodes)
            .Where(x => x != null);
        context.RegisterSourceOutput(callsites.Collect(), Generate!);
    }

    private static bool FindNodes(SyntaxNode syntaxNode, CancellationToken cancellationToken) => syntaxNode switch
    {
        FunctionPointerCallingConventionSyntax => true,
        _ => false
    };

    private static void Generate(SourceProductionContext context, ImmutableArray<FunctionPointerCallingConventionSyntax> callsites)
    {
        if (!Debugger.IsAttached)
        {
            Debugger.Launch();
        }

        foreach (var callsite in callsites)
        {
            if (context.CancellationToken.IsCancellationRequested) return;
            callsite.ToFullString();
            if (context.CancellationToken.IsCancellationRequested) return;

            /*
            var displayName = type.ToDisplayString(SymbolDisplayFormat.FullyQualifiedFormat);
            var fileName = $"{displayName.Replace("global::", "")}MetaData.g";
            context.AddSource(fileName, source);
            */
        }
    }

    private static FunctionPointerCallingConventionSyntax? ProcessNodes(GeneratorSyntaxContext syntaxContext, CancellationToken cancellationToken) => syntaxContext.Node switch
    {
        FunctionPointerCallingConventionSyntax x => x,
        _ => null
    };
}


