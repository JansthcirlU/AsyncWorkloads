using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Testing;
using Microsoft.CodeAnalysis.Testing;

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
        [AsyncWorkload<bool>("Test workload")]
        public class TestWorkload
        {
        }
        """;
        AsyncWorkloadAnalyzerTest test = new() { TestCode = source };

        // Expect
        DiagnosticResult partialOrSealedError = new DiagnosticResult(AsyncWorkloadAnalyzer.DiagnosticId, DiagnosticSeverity.Error)
                .WithSpan(3, 26, 3, 39)
                .WithArguments("TestWorkload");
        test.ExpectedDiagnostics.Add(partialOrSealedError);
        
        // Verify
        await test.RunAsync();
    }
}