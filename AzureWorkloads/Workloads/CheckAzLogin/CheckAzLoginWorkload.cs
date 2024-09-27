using AsyncWorkloads.Results;
using AsyncWorkloads.Workloads;
using Microsoft.Extensions.Logging;

namespace AzureWorkloads.Workloads.CheckAzLogin;

public class CheckAzLoginWorkload : AsyncWorkload<bool>
{
    public CheckAzLoginWorkload(ILogger<CheckAzLoginWorkload> logger) : base(logger)
    {
    }

    protected override async Task<WorkloadResult<bool>> ExecuteWorkAsync(CorrelationId correlationId, CancellationToken cancellationToken)
    {
        await Task.Delay(10_000, cancellationToken);
        double next = Random.Shared.NextDouble();
        return WorkloadResult<bool>.Success(next > 0.8, WorkloadId, correlationId);
    }
}
