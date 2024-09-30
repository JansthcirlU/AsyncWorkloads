using AsyncWorkloads.Results;
using AsyncWorkloads.Workloads;
using AzureWorkloads.Workloads.FetchAzureSubscriptionInfo;
using Microsoft.Extensions.Logging;

namespace AzureWorkloads.Workloads.CheckAzLogin;

public sealed class CheckAzLoginWorkload : AsyncWorkload<bool>
{
    public FetchAzureSubscriptionInfoWorkload FetchAzureSubscriptionInfoWorkload { get; }

    public CheckAzLoginWorkload(
        ILogger<CheckAzLoginWorkload> logger,
        FetchAzureSubscriptionInfoWorkload fetchAzureSubscriptionInfoWorkload) : base(logger)
    {
        FetchAzureSubscriptionInfoWorkload = fetchAzureSubscriptionInfoWorkload;
    }

    protected override sealed async Task<WorkloadResult<bool>> ExecuteWorkAsync(CorrelationId correlationId, CancellationToken cancellationToken)
    {
        WorkloadResult<string> test = await FetchAzureSubscriptionInfoWorkload.ExecuteAsync(correlationId, cancellationToken);
        Console.WriteLine(test);
        await Task.Delay(10_000, cancellationToken);
        double next = Random.Shared.NextDouble();
        WorkloadResult<bool> result = Success(next > 0.8, correlationId);
        return result;
    }
}
