using AsyncWorkloads.Results;
using AsyncWorkloads.Workloads;
using AsyncWorkloads.Workloads.Prerequisites;
using AzureWorkloads.Workloads.CheckAzLogin;
using Microsoft.Extensions.Logging;

namespace AzureWorkloads.Workloads.FetchAzureSubscriptionInfo;

public class FetchAzureSubscriptionInfoWorkload : PrerequisiteAsyncWorkload<CheckAzLoginWorkload, bool, string>
{
    public FetchAzureSubscriptionInfoWorkload(
        ILogger<FetchAzureSubscriptionInfoWorkload> logger,
        CheckAzLoginWorkload checkAzLoginWorkload) : base(logger, checkAzLoginWorkload)
    {
    }

    protected override async Task<WorkloadResult<string>> ExecuteWorkAsync(CorrelationId correlationId, CancellationToken cancellationToken)
    {
        CheckAzLoginWorkload checkAzLoginWorkload = FirstPrerequisite;
        WorkloadResult<bool> checkAzLoginResult = await checkAzLoginWorkload.ExecuteAsync(correlationId, cancellationToken);
        WorkloadResult<string> result = checkAzLoginResult.Bind(
            met => met
                ? Success(Guid.NewGuid().ToString(), correlationId)
                : Failure<string>(new Exception("Guid info could not be found."), correlationId),
            WorkloadId,
            correlationId);
        return result;
    }
}
