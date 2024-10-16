using Microsoft.CodeAnalysis.CSharp.Testing;
using Microsoft.CodeAnalysis.Testing;
using AsyncWorkloads.Attributes;
using AsyncWorkloads.Workloads;

namespace AsyncWorkloads.Analyzers.Tests;

using AsyncWorkloadModifiersAnalyzerTestTest = CSharpAnalyzerTest<AsyncWorkloadModifiersAnalyzer, DefaultVerifier>;
using PrerequisiteWorkloadGenericsAnalyzerTest = CSharpAnalyzerTest<PrerequisiteWorkloadGenericsAnalyzer, DefaultVerifier>;
using WorkloadWithPrerequisitesAnalyzerTest = CSharpAnalyzerTest<WorkloadWithPrerequisitesAnalyzer, DefaultVerifier>;

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

    [Fact]
    public async Task ClassWithPrerequisiteWorkloadAttribute_ButNotAsyncWorkload_ShouldReportErrorDiagnostic()
    {
        // Arrange
        WorkloadWithPrerequisitesAnalyzerTest test = new()
        {
            ReferenceAssemblies = ReferenceAssemblies.Net.Net80
        };
        test.TestState.AdditionalReferences.Add(typeof(AsyncWorkloadAttribute<>).Assembly);
        test.TestState.AdditionalReferences.Add(typeof(AsyncWorkload<>).Assembly);
        test.TestState.AdditionalReferences.Add(typeof(Microsoft.Extensions.Logging.ILogger).Assembly);

        // Expect
        test.TestCode = /* lang=c#-test */
        """
        using System.Threading;
        using System.Threading.Tasks;
        using AsyncWorkloads.Attributes;
        using AsyncWorkloads.Results;
        using AsyncWorkloads.Workloads;
        using Microsoft.Extensions.Logging;

        public class TestPrerequisiteWorkload : AsyncWorkload<int>
        {
            public TestPrerequisiteWorkload(ILogger<AsyncWorkload<int>> logger)
                : base("Test workload", logger)
            {
            }

            protected override Task<WorkloadResult<int>> ExecuteWorkAsync(CorrelationId correlationId, CancellationToken cancellationToken)
                => Task.FromResult(Success(69, correlationId));
        }

        [PrerequisiteWorkload<TestPrerequisiteWorkload>]
        public class {|AW003:TestWorkload|}
        {
        }
        """;
        
        // Verify
        await test.RunAsync();
    }
}
