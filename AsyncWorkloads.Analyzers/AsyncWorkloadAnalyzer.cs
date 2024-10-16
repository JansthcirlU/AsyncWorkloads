using System;
using System.Collections.Immutable;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Diagnostics;

namespace AsyncWorkloads.Analyzers
{
    [DiagnosticAnalyzer(LanguageNames.CSharp)]
    public class AsyncWorkloadAnalyzer : DiagnosticAnalyzer
    {
        public override ImmutableArray<DiagnosticDescriptor> SupportedDiagnostics => throw new NotImplementedException();

        public override void Initialize(AnalysisContext context)
        {
            throw new NotImplementedException();
        }
    }
}
