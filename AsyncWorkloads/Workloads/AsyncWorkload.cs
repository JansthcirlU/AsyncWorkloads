using AsyncWorkloads.Prerequisites;
using AsyncWorkloads.Results;

namespace AsyncWorkloads.Workloads;

public abstract class AsyncWorkload<TResult> : IAsyncWorkload<TResult>
{
    private WorkloadState _workloadState = WorkloadState.Undefined;

    public WorkloadState WorkloadState => _workloadState;

    protected abstract Task<Result<TResult>> ExecuteWorkAsync(CancellationToken cancellationToken);

    public async Task<Result<TResult>> ExecuteAsync(CancellationToken cancellationToken)
    {
        try
        {
            _workloadState = WorkloadState.Running;
            Result<TResult> result = await ExecuteWorkAsync(cancellationToken);
            _workloadState = result.IsSuccess ? WorkloadState.Completed : WorkloadState.Faulted;
            return result;
        }
        catch (OperationCanceledException operationCanceledException)
        {
            _workloadState = WorkloadState.Cancelled;
            return Result<TResult>.Failure(operationCanceledException);
        }
        catch (Exception ex)
        {
            _workloadState = WorkloadState.Faulted;
            return Result<TResult>.Failure(ex);
        }
    }
}

public abstract class AsyncWorkload<TPrerequisite, TResult> :
    IAsyncWorkload<TPrerequisite, TResult>
{
    private WorkloadState _workloadState = WorkloadState.Undefined;

    public WorkloadState WorkloadState => throw new NotImplementedException();
    public IPrerequisiteAsyncWorkloads<TPrerequisite> PrerequisiteWorkloads { get; }

    public AsyncWorkload(IPrerequisiteAsyncWorkloads<TPrerequisite> prerequisiteWorkloads)
    {
        PrerequisiteWorkloads = prerequisiteWorkloads;
    }

    protected abstract Task<Result<TResult>> ExecuteWorkAsync(Result<TPrerequisite> prerequisite, CancellationToken cancellationToken);

    public async Task<Result<TResult>> ExecuteAsync(CancellationToken cancellationToken)
    {
        try
        {
            _workloadState = WorkloadState.Running;
            Result<TPrerequisite> prerequisite = await PrerequisiteWorkloads.ExecuteAsync(cancellationToken);
            Result<TResult> result = await ExecuteWorkAsync(prerequisite, cancellationToken);
            _workloadState = result.IsSuccess ? WorkloadState.Completed : WorkloadState.Faulted;
            return result;
        }
        catch (OperationCanceledException operationCanceledException)
        {
            _workloadState = WorkloadState.Cancelled;
            return Result<TResult>.Failure(operationCanceledException);
        }
        catch (Exception ex)
        {
            _workloadState = WorkloadState.Faulted;
            return Result<TResult>.Failure(ex);
        }
    }
}