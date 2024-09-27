using AsyncWorkloads.Prerequisites;
using AsyncWorkloads.Results;

namespace AsyncWorkloads.Workloads;

public interface IAsyncWorkload<TResult>
{
    WorkloadId WorkloadId { get; }
    WorkloadState WorkloadState { get; }
    Task<WorkloadResult<TResult>> ExecuteAsync(CorrelationId correlationId, CancellationToken cancellationToken);
}

public interface IAsyncWorkload<TPrerequisite, TResult> : IAsyncWorkload<TResult>
{
    IPrerequisiteAsyncWorkloads<TPrerequisite> PrerequisiteWorkloads { get; }
}