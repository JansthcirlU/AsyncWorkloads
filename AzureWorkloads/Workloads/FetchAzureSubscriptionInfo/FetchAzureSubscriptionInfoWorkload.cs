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
                        ? WorkloadResult<string>.Success(Guid.NewGuid().ToString(), WorkloadId, correlationId)
                        : WorkloadResult<string>.Failure(new Exception("Guid info could not be found."), WorkloadId, correlationId)));
}
