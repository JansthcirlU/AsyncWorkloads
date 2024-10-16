using System.Collections.Immutable;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Diagnostics;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace AsyncWorkloads.Analyzers
{
    [DiagnosticAnalyzer(LanguageNames.CSharp)]
    public class PrerequisiteWorkloadGenericsAnalyzer : DiagnosticAnalyzer
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
                    // Get the symbol for the attribute.
                    if (!(semanticModel.GetSymbolInfo(attribute).Symbol is IMethodSymbol attributeSymbol))
                        continue;

                    var attributeContainingType = attributeSymbol.ContainingType;
                    if (attributeContainingType.Name == "PrerequisiteWorkloadAttribute" &&
                        attributeContainingType.ContainingNamespace.ToDisplayString() == "AsyncWorkloads.Attributes")
                    {
                        // Get the attribute's type information.
                        if (!(semanticModel.GetTypeInfo(attribute).Type is INamedTypeSymbol attributeType) || attributeType.TypeArguments.Length == 0)
                        {
                            continue;
                        }

                        // Extract the generic type argument for TPrerequisiteWorkload.
                        if (attributeType.TypeArguments[0] is INamedTypeSymbol prerequisiteTypeSymbol)
                        {
                            // Check if prerequisiteTypeSymbol inherits from AsyncWorkload<>.
                            if (!InheritsFromAsyncWorkload(prerequisiteTypeSymbol, context))
                            {
                                var diagnostic = Diagnostic.Create(Rule, attribute.GetLocation(), prerequisiteTypeSymbol.Name);
                                context.ReportDiagnostic(diagnostic);
                            }
                        }
                    }
                }
            }
        }

        private static bool InheritsFromAsyncWorkload(INamedTypeSymbol typeSymbol, SyntaxNodeAnalysisContext context)
        {
            // Look up the AsyncWorkload base type by its metadata name.
            var asyncWorkloadBaseType = context.Compilation.GetTypeByMetadataName("AsyncWorkloads.Workloads.AsyncWorkload`1");
            if (asyncWorkloadBaseType == null)
            {
                // If the base type cannot be found, ensure the assembly reference is correct.
                return false;
            }

            // Traverse the base types to determine inheritance.
            var baseType = typeSymbol.BaseType;

            while (baseType != null)
            {
                if (SymbolEqualityComparer.Default.Equals(baseType.OriginalDefinition, asyncWorkloadBaseType))
                {
                    return true;
                }

                baseType = baseType.BaseType;
            }

            return false;
        }
    }
}

