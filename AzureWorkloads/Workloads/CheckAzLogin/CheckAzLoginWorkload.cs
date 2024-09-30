using AsyncWorkloads.Results;
using AsyncWorkloads.Workloads;
using AsyncWorkloads.Workloads.Simple;
using Microsoft.Extensions.Logging;

namespace AzureWorkloads.Workloads.CheckAzLogin;

public sealed class CheckAzLoginWorkload : SimpleAsyncWorkload<bool>
{
    public CheckAzLoginWorkload(ILogger<CheckAzLoginWorkload> logger) : base(logger)
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
