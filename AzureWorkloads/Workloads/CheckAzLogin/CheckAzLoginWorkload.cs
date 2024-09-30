using AsyncWorkloads.Results;
using AsyncWorkloads.Workloads;
using Microsoft.Extensions.Logging;

namespace AzureWorkloads.Workloads.CheckAzLogin;

public sealed class CheckAzLoginWorkload : AsyncWorkload<bool>
{
    public CheckAzLoginWorkload(
        ILogger<CheckAzLoginWorkload> logger) : base("Check az login", logger)
    {
    }

    protected override sealed async Task<WorkloadResult<bool>> ExecuteWorkAsync(CorrelationId correlationId, CancellationToken cancellationToken)
    {
        await Task.Delay(10_000, cancellationToken);
        double next = Random.Shared.NextDouble();
        WorkloadResult<bool> result = Success(next > 0.8, correlationId);
        return result;
    }
}
