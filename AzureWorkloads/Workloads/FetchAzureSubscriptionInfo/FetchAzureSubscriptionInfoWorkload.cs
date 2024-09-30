using AsyncWorkloads.Results;
using AsyncWorkloads.Workloads;
using AzureWorkloads.Workloads.CheckAzLogin;
using Microsoft.Extensions.Logging;

namespace AzureWorkloads.Workloads.FetchAzureSubscriptionInfo;

public class FetchAzureSubscriptionInfoWorkload : AsyncWorkload<string>
{
    CheckAzLoginWorkload CheckAzLoginWorkload { get; }

    public FetchAzureSubscriptionInfoWorkload(
        ILogger<FetchAzureSubscriptionInfoWorkload> logger,
        CheckAzLoginWorkload checkAzLoginWorkload) : base(logger)
    {
        CheckAzLoginWorkload = checkAzLoginWorkload;
    }

    protected override async Task<WorkloadResult<string>> ExecuteWorkAsync(CorrelationId correlationId, CancellationToken cancellationToken)
    {
        WorkloadResult<bool> checkAzLoginResult = await CheckAzLoginWorkload.ExecuteAsync(correlationId, cancellationToken);
        WorkloadResult<string> result = checkAzLoginResult.Bind(
            met => met
                ? Success(Guid.NewGuid().ToString(), correlationId)
                : Failure<string>(new Exception("Guid info could not be found."), correlationId),
            WorkloadId,
            correlationId);
        return result;
    }
}
