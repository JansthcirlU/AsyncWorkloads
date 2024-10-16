using Microsoft.CodeAnalysis.CSharp.Testing;
using Microsoft.CodeAnalysis.Testing;
using AsyncWorkloads.Attributes;
using AsyncWorkloads.Workloads;

namespace AsyncWorkloads.Analyzers.Tests;

using AsyncWorkloadModifiersAnalyzerTestTest = CSharpAnalyzerTest<AsyncWorkloadModifiersAnalyzer, DefaultVerifier>;
using PrerequisiteWorkloadGenericsAnalyzerTest = CSharpAnalyzerTest<PrerequisiteWorkloadGenericsAnalyzer, DefaultVerifier>;

public class AsyncWorkloadAnalyzerTests
{
    [Fact]
    public async Task ClassWithoutSealedOrPartial_WhenDecoratedWithAsyncWorkloadAttribute_ShouldReportErrorDiagnostic()
    {
        // Arrange
        AsyncWorkloadModifiersAnalyzerTestTest test = new()
        {
            ReferenceAssemblies = ReferenceAssemblies.NetStandard.NetStandard20
        };
        test.TestState.AdditionalReferences.Add(typeof(AsyncWorkloadAttribute<>).Assembly);

        // Expect
        test.TestCode = /* lang=c#-test */
        """
        using AsyncWorkloads.Attributes;

        [AsyncWorkload<bool>("Test workload")]
        public class {|AW001:TestWorkload|}
        {
        }
        """;
        
        // Verify
        await test.RunAsync();
    }

    [Fact]
    public async Task PrerequisiteWorkloadAttribute_WhenNotInheritsFromAsyncWorkload_ShouldReportErrorDiagnostic()
    {
        // Arrange
        PrerequisiteWorkloadGenericsAnalyzerTest test = new()
        {
            ReferenceAssemblies = ReferenceAssemblies.NetStandard.NetStandard20
        };
        test.TestState.AdditionalReferences.Add(typeof(AsyncWorkloadAttribute<>).Assembly);

        // Expect
        test.TestCode = /* lang=c#-test */
        """
        using AsyncWorkloads.Attributes;

        public class TestPrerequisiteWorkload
        {
            
        }

        [AsyncWorkload<bool>("Test workload")]
        [{|AW002:PrerequisiteWorkload<TestPrerequisiteWorkload>|}]
        public class TestWorkload
        {
        }
        """;
        
        // Verify
        await test.RunAsync();
    }
}