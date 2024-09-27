using AsyncWorkloads.Prerequisites;
using AsyncWorkloads.Results;

namespace AsyncWorkloads.Workloads;

/// <summary>
/// Interface for an async workload that yields a workload result.
/// </summary>
/// <typeparam name="TResult">Expected result from running the async workload.</typeparam>
public interface IAsyncWorkload<TResult>
{
    /// <summary>
    /// Unique identifier of the workload.
    /// </summary>
    WorkloadId WorkloadId { get; }

    /// <summary>
    /// Current state of the workload.
    /// </summary>
    WorkloadState WorkloadState { get; }

    /// <summary>
    /// Executes the async workload and returns its result.
    /// If the workload has already run before, this returns the cached result.
    /// </summary>
    /// <param name="correlationId">Unique identifier to track the workload execution context.</param>
    /// <param name="cancellationToken">Token to cancel the execution if needed.</param>
    Task<WorkloadResult<TResult>> ExecuteAsync(CorrelationId correlationId, CancellationToken cancellationToken);
}

/// <summary>
/// Interface for an async workload yields a workload result based on the result of its prerequisite workloads.
/// </summary>
/// <typeparam name="TPrerequisite">Expected combined result after running all the prerequisite workloads.</typeparam>
/// <typeparam name="TResult">Expected result from running the actual workload itself.</typeparam>
public interface IAsyncWorkload<TPrerequisite, TResult> : IAsyncWorkload<TResult>
{
    /// <summary>
    /// Prerequisite workloads that need to be run before the actual workload.
    /// </summary>
    IPrerequisiteAsyncWorkloads<TPrerequisite> PrerequisiteWorkloads { get; }
}