using AsyncWorkloads.Prerequisites;
using AsyncWorkloads.Results;

namespace AsyncWorkloads.Workloads;

public interface IAsyncWorkload<TResult>
{
    WorkloadState WorkloadState { get; }
    Task<Result<TResult>> ExecuteAsync(CancellationToken cancellationToken);
}

public interface IAsyncWorkload<TPrerequisite, TResult> : IAsyncWorkload<TResult>
{
    IPrerequisiteAsyncWorkloads<TPrerequisite> PrerequisiteWorkloads { get; }
}