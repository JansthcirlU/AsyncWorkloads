using AsyncWorkloads.Prerequisites;
using AsyncWorkloads.Results;
using AzureWorkloads.Workloads.CheckAzLogin;

namespace AzureWorkloads.Workloads.FetchAzureSubscriptionInfo;

public class FetchAzureSubscriptionInfoWorkloadPrerequisite : PrerequisiteAsyncWorkloads<bool, bool>
{
    public FetchAzureSubscriptionInfoWorkloadPrerequisite(CheckAzLoginWorkload firstPrerequisite) : base(firstPrerequisite)
    {
    }

    protected override Task<Result<bool>> ExecuteWorkAsync(CancellationToken cancellationToken)
        => FirstPrerequisite.ExecuteAsync(cancellationToken);
}
