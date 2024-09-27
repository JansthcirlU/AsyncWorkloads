using AsyncWorkloads.Results;
using AsyncWorkloads.Workloads;
using Microsoft.Extensions.Logging;

namespace AzureWorkloads.Workloads.FetchAzureSubscriptionInfo;

public class FetchAzureSubscriptionInfoWorkload : AsyncWorkload<bool, string>
{
    public FetchAzureSubscriptionInfoWorkload(
        ILogger<FetchAzureSubscriptionInfoWorkload> logger,
        FetchAzureSubscriptionInfoWorkloadPrerequisite prerequisiteWorkloads) : base(logger, prerequisiteWorkloads)
    {
    }

    protected override Task<WorkloadResult<string>> ExecuteWorkAsync(WorkloadResult<bool> prerequisite, CorrelationId correlationId, CancellationToken cancellationToken)
        => Task.FromResult(
                prerequisite.Bind(
                    met => met
                        ? Success(Guid.NewGuid().ToString(), correlationId)
                        : Failure<string>(new Exception("Guid info could not be found."), correlationId),
                    WorkloadId,
                    correlationId)
                );
}
