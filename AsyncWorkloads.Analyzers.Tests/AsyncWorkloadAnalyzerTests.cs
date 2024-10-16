using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Testing;
using Microsoft.CodeAnalysis.Testing;
using AsyncWorkloads.Attributes;

namespace AsyncWorkloads.Analyzers.Tests;

using AsyncWorkloadAnalyzerTest = CSharpAnalyzerTest<AsyncWorkloadAnalyzer, DefaultVerifier>;

public class AsyncWorkloadAnalyzerTests
{
    [Fact]
    public async Task ClassWithoutSealedOrPartial_WhenDecoratedWithAsyncWorkloadAttribute_ShouldReportErrorDiagnostic()
    {
        // Arrange
        string source = /* lang=c#-test */
        """
        using AsyncWorkloads.Attributes;

        [AsyncWorkload<bool>("Test workload")]
        public class TestWorkload
        {
        }
        """;

        AsyncWorkloadAnalyzerTest test = new()
        {
            TestCode = source,
            ReferenceAssemblies = ReferenceAssemblies.NetStandard.NetStandard20
        };
        test.TestState.AdditionalReferences.Add(typeof(AsyncWorkloadAttribute<bool>).Assembly);

        // Expect
        DiagnosticResult partialOrSealedError = new DiagnosticResult(AsyncWorkloadAnalyzer.DiagnosticId, DiagnosticSeverity.Error)
                .WithSpan(4, 14, 4, 26)
                .WithArguments("TestWorkload");
        test.ExpectedDiagnostics.Add(partialOrSealedError);
        
        // Verify
        await test.RunAsync();
    }
}