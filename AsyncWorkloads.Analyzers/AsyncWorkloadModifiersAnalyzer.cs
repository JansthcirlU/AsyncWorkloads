using System.Collections.Immutable;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Diagnostics;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System.Linq;

namespace AsyncWorkloads.Analyzers
{
    [DiagnosticAnalyzer(LanguageNames.CSharp)]
    public class AsyncWorkloadModifiersAnalyzer : DiagnosticAnalyzer
    {
        public const string DiagnosticId = "AW001";
        private static readonly LocalizableString Title = "Workload must be sealed and partial";
        private static readonly LocalizableString MessageFormat = "Class '{0}' must be sealed and partial to support source generation";
        private static readonly LocalizableString Description = "Ensure classes marked as workloads are sealed and partial.";
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

            // Check if the class has the AsyncWorkloadAttribute
            var hasWorkloadAttribute = classDeclaration.AttributeLists
                .SelectMany(al => al.Attributes)
                .Any(attr => attr.Name.ToString().Contains("AsyncWorkload"));

            if (!hasWorkloadAttribute) return;

            // Check if the class is partial and sealed
            var isPartial = classDeclaration.Modifiers.Any(SyntaxKind.PartialKeyword);
            var isSealed = classDeclaration.Modifiers.Any(SyntaxKind.SealedKeyword);

            if (!isPartial || !isSealed)
            {
                var diagnostic = Diagnostic.Create(Rule, classDeclaration.Identifier.GetLocation(), classDeclaration.Identifier.Text);
                context.ReportDiagnostic(diagnostic);
            }
        }
    }
}
