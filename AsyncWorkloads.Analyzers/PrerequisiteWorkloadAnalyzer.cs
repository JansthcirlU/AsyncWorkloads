using System.Collections.Immutable;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Diagnostics;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace AsyncWorkloads.Analyzers
{
    [DiagnosticAnalyzer(LanguageNames.CSharp)]
    public class PrerequisiteWorkloadAnalyzer : DiagnosticAnalyzer
    {
        public const string DiagnosticId = "AW002";
        private static readonly LocalizableString Title = "Prerequisite workload type constraint violation";
        private static readonly LocalizableString MessageFormat = "Type '{0}' must inherit from 'AsyncWorkload<>' to be used as a prerequisite";
        private static readonly LocalizableString Description = "Ensure that the prerequisite workload type inherits from the AsyncWorkload<> base class.";
        private const string Category = "Usage";

        private static readonly DiagnosticDescriptor Rule = new DiagnosticDescriptor(
            DiagnosticId, Title, MessageFormat, Category,
            DiagnosticSeverity.Error, isEnabledByDefault: true, description: Description);

        public override ImmutableArray<DiagnosticDescriptor> SupportedDiagnostics => ImmutableArray.Create(Rule);

        public override void Initialize(AnalysisContext context)
        {
            context.ConfigureGeneratedCodeAnalysis(GeneratedCodeAnalysisFlags.None);
            context.EnableConcurrentExecution();
            context.RegisterSyntaxNodeAction(AnalyzeClassDeclaration, SyntaxKind.ClassDeclaration);
        }

        private static void AnalyzeClassDeclaration(SyntaxNodeAnalysisContext context)
        {
            var classDeclaration = (ClassDeclarationSyntax)context.Node;

            // Look for PrerequisiteWorkloadAttribute
            var semanticModel = context.SemanticModel;
            foreach (var attributeList in classDeclaration.AttributeLists)
            {
                foreach (var attribute in attributeList.Attributes)
                {
                    if (!(semanticModel.GetSymbolInfo(attribute).Symbol is IMethodSymbol attributeSymbol))
                        continue;

                    var attributeContainingType = attributeSymbol.ContainingType;
                    if (attributeContainingType.Name == "PrerequisiteWorkloadAttribute" &&
                        attributeContainingType.ContainingNamespace.ToDisplayString() == "AsyncWorkloads.Attributes")
                    {
                        // Get the type argument for TPrerequisiteWorkload
                        if (attribute.ArgumentList?.Arguments.Count > 0)
                        {
                            var prerequisiteTypeSyntax = attribute.ArgumentList.Arguments[0].Expression;

                            if (semanticModel.GetTypeInfo(prerequisiteTypeSyntax).Type is INamedTypeSymbol prerequisiteTypeSymbol)
                            {
                                // Check if prerequisiteTypeSymbol inherits from AsyncWorkload<>
                                if (!InheritsFromAsyncWorkload(prerequisiteTypeSymbol))
                                {
                                    var diagnostic = Diagnostic.Create(Rule, prerequisiteTypeSyntax.GetLocation(), prerequisiteTypeSymbol.Name);
                                    context.ReportDiagnostic(diagnostic);
                                }
                            }
                        }
                    }
                }
            }
        }

        private static bool InheritsFromAsyncWorkload(INamedTypeSymbol typeSymbol)
        {
            var baseType = typeSymbol.BaseType;

            while (baseType != null)
            {
                if (baseType.Name == "AsyncWorkload" &&
                    baseType.ContainingNamespace.ToDisplayString() == "AsyncWorkloads.Workloads" &&
                    baseType.IsGenericType)
                {
                    return true;
                }

                baseType = baseType.BaseType;
            }

            return false;
        }
    }
}

