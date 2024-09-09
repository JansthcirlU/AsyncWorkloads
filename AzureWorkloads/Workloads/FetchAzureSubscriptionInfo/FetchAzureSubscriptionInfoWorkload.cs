using AsyncWorkloads.Results;
using AsyncWorkloads.Workloads;

namespace AzureWorkloads.Workloads.FetchAzureSubscriptionInfo;

public class FetchAzureSubscriptionInfoWorkload : AsyncWorkload<bool, string>
{
    public FetchAzureSubscriptionInfoWorkload(FetchAzureSubscriptionInfoWorkloadPrerequisite prerequisiteWorkloads) : base(prerequisiteWorkloads)
    {
    }

    protected override Task<Result<string>> ExecuteWorkAsync(Result<bool> prerequisite, CancellationToken cancellationToken)
        => Task.FromResult(
                prerequisite.Bind(
                    met => met
                        ? Result<string>.Success(Guid.NewGuid().ToString())
                        : Result<string>.Failure(new Exception("Guid info could not be found."))));
}
