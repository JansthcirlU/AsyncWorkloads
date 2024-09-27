using AsyncWorkloads.Prerequisites;
using AsyncWorkloads.Results;
using AsyncWorkloads.Workloads;
using AzureWorkloads.Workloads.CheckAzLogin;
using Microsoft.Extensions.Logging;

namespace AzureWorkloads.Workloads.FetchAzureSubscriptionInfo;

public class FetchAzureSubscriptionInfoWorkloadPrerequisite : PrerequisiteAsyncWorkloads<bool, bool>
{
    public FetchAzureSubscriptionInfoWorkloadPrerequisite(
        ILogger<FetchAzureSubscriptionInfoWorkloadPrerequisite> logger,
        CheckAzLoginWorkload firstPrerequisite) : base(logger, firstPrerequisite)
    {
    }

    protected override Task<WorkloadResult<bool>> ExecuteWorkAsync(CorrelationId correlationId, CancellationToken cancellationToken)
        => FirstPrerequisite.ExecuteAsync(correlationId, cancellationToken);
}
