using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Testing;
using Microsoft.CodeAnalysis.Testing;

namespace AsyncWorkloads.Analyzers.Tests;

public class AsyncWorkloadAnalyzerTests
{
    [Fact]
    public async Task ClassWithoutSealedOrPartial_WhenDecoratedWithAsyncWorkloadAttribute_ShouldReportErrorDiagnostic()
    {
        // Arrange
        string source =
        """
        [AsyncWorkload<bool>("Test workload")]
        public class TestWorkload
        {
        }
        """;
        CSharpAnalyzerTest<AsyncWorkloadAnalyzer, DefaultVerifier> test = new() { TestCode = source };
        test.ExpectedDiagnostics.Add(
            new DiagnosticResult(AsyncWorkloadAnalyzer.DiagnosticId, DiagnosticSeverity.Error)
                .WithSpan(3, 26, 3, 39)
                .WithArguments("TestWorkload")
            );

        // Act
        await test.RunAsync();
    }
}