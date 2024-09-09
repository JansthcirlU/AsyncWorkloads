using AsyncWorkloads.Results;
using AsyncWorkloads.Workloads;

namespace AzureWorkloads.Workloads.CheckAzLogin;

public class CheckAzLoginWorkload : AsyncWorkload<bool>
{
    protected override async Task<Result<bool>> ExecuteWorkAsync(CancellationToken cancellationToken)
    {
        await Task.Delay(1000, cancellationToken);
        double next = Random.Shared.NextDouble();
        return Result<bool>.Success(next > 0.8);
    }
}
