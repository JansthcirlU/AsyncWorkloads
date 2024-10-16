namespace AsyncWorkloads.Analyzers
{
    using System.Collections.Immutable;
    using Microsoft.CodeAnalysis;
    using Microsoft.CodeAnalysis.Diagnostics;
    using Microsoft.CodeAnalysis.CSharp;
    using Microsoft.CodeAnalysis.CSharp.Syntax;

    namespace AsyncWorkloads.Analyzers
    {
        [DiagnosticAnalyzer(LanguageNames.CSharp)]
        public class WorkloadWithPrerequisitesAnalyzer : DiagnosticAnalyzer
        {
            public const string DiagnosticId = "AW003";

            private static readonly DiagnosticDescriptor Rule = new DiagnosticDescriptor(
                DiagnosticId,
                "Type is not a workload but has prerequisite workloads",
                "Type '{0}' must be decorated with the [AsyncWorkload<>] attribute to enable prerequisite workloads",
                "Usage",
                DiagnosticSeverity.Error,
                isEnabledByDefault: true,
                description: "Ensure that the type with prerequisite workloads is an async workload itself."
            );

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
                var semanticModel = context.SemanticModel;

                // Check if the class has the PrerequisiteWorkloadAttribute
                bool hasPrerequisiteAttribute = false;
                foreach (var attributeList in classDeclaration.AttributeLists)
                {
                    foreach (var attribute in attributeList.Attributes)
                    {
                        var attributeSymbol = semanticModel.GetSymbolInfo(attribute).Symbol as IMethodSymbol;
                        if (attributeSymbol == null)
                            continue;

                        var attributeContainingType = attributeSymbol.ContainingType;
                        if (attributeContainingType.Name == "PrerequisiteWorkloadAttribute" &&
                            attributeContainingType.ContainingNamespace.ToDisplayString() == "AsyncWorkloads.Attributes")
                        {
                            hasPrerequisiteAttribute = true;
                            break;
                        }
                    }

                    if (hasPrerequisiteAttribute)
                        break;
                }

                if (!hasPrerequisiteAttribute)
                    return;

                // Check if the class has the AsyncWorkload attribute
                bool hasAsyncWorkloadAttribute = false;
                foreach (var attributeList in classDeclaration.AttributeLists)
                {
                    foreach (var attribute in attributeList.Attributes)
                    {
                        var attributeSymbol = semanticModel.GetSymbolInfo(attribute).Symbol as IMethodSymbol;
                        if (attributeSymbol == null)
                            continue;

                        var attributeContainingType = attributeSymbol.ContainingType;
                        if (attributeContainingType.Name.StartsWith("AsyncWorkloadAttribute") &&
                            attributeContainingType.ContainingNamespace.ToDisplayString() == "AsyncWorkloads.Attributes")
                        {
                            hasAsyncWorkloadAttribute = true;
                            break;
                        }
                    }

                    if (hasAsyncWorkloadAttribute)
                        break;
                }

                // Report a diagnostic if the class has prerequisites but is not an async workload
                if (!hasAsyncWorkloadAttribute)
                {
                    var diagnostic = Diagnostic.Create(Rule, classDeclaration.Identifier.GetLocation(), classDeclaration.Identifier.Text);
                    context.ReportDiagnostic(diagnostic);
                }
            }
        }
    }
}
