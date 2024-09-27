using AsyncWorkloads.Prerequisites;
using AsyncWorkloads.Results;
using Microsoft.Extensions.Logging;

namespace AsyncWorkloads.Workloads;

public abstract class AsyncWorkload<TResult> : IAsyncWorkload<TResult>
{
    private WorkloadState _workloadState = WorkloadState.Undefined;
    private WorkloadResult<TResult>? _result;
    protected readonly ILogger<AsyncWorkload<TResult>> _logger;

    public WorkloadState WorkloadState => _workloadState;
    public WorkloadId WorkloadId { get; } = WorkloadId.Create();

    public AsyncWorkload(ILogger<AsyncWorkload<TResult>> logger)
    {
        _logger = logger;
    }

    protected abstract Task<WorkloadResult<TResult>> ExecuteWorkAsync(CorrelationId correlationId, CancellationToken cancellationToken);

    public async Task<WorkloadResult<TResult>> ExecuteAsync(CorrelationId correlationId, CancellationToken cancellationToken)
    {
        // If the workload has already run, simply return the result
        if (_result is not null) return _result;

        try
        {
            // Start the workload and save the result
            _logger.LogInformation("Starting workload {WorkloadId} with correlation {CorrelationId}.", WorkloadId, correlationId);
            _workloadState = WorkloadState.Running;
            _result = await ExecuteWorkAsync(correlationId, cancellationToken);
            _workloadState = _result.IsSuccess ? WorkloadState.Completed : WorkloadState.Faulted;
        }
        catch (OperationCanceledException operationCanceledException)
        {
            // If cancelled, save the failed result
            _logger.LogWarning("Cancelled workload {WorkloadId} with correlation {CorrelationId}.", WorkloadId, correlationId);
            _result = WorkloadResult<TResult>.Failure(operationCanceledException, WorkloadId, correlationId);
            _workloadState = WorkloadState.Cancelled;
        }
        catch (Exception ex)
        {
            // If faulted, save the failed result as well
            _logger.LogError(ex, "Exception during workload {WorkloadId} with correlation {CorrelationId}.", WorkloadId, correlationId);
            _result = WorkloadResult<TResult>.Failure(ex, WorkloadId, correlationId);
            _workloadState = WorkloadState.Faulted;
        }
        return _result;
    }
}

public abstract class AsyncWorkload<TPrerequisite, TResult> :
    IAsyncWorkload<TPrerequisite, TResult>
{
    private WorkloadState _workloadState = WorkloadState.Undefined;
    private WorkloadResult<TResult>? _result;
    protected readonly ILogger<AsyncWorkload<TPrerequisite, TResult>> _logger;

    public WorkloadState WorkloadState => _workloadState;
    public WorkloadId WorkloadId { get; } = WorkloadId.Create();
    public IPrerequisiteAsyncWorkloads<TPrerequisite> PrerequisiteWorkloads { get; }

    public AsyncWorkload(
        ILogger<AsyncWorkload<TPrerequisite, TResult>> logger,
        IPrerequisiteAsyncWorkloads<TPrerequisite> prerequisiteWorkloads)
    {
        _logger = logger;
        PrerequisiteWorkloads = prerequisiteWorkloads;
    }

    protected abstract Task<WorkloadResult<TResult>> ExecuteWorkAsync(WorkloadResult<TPrerequisite> prerequisite, CorrelationId correlationId, CancellationToken cancellationToken);

    public async Task<WorkloadResult<TResult>> ExecuteAsync(CorrelationId correlationId, CancellationToken cancellationToken)
    {
        // If the workload has already run, simply return the result
        if (_result is not null) return _result;

        try
        {
            // Run prerequisites
            _logger.LogInformation("Running prerequisites for workload {WorkloadId} with correlation {CorrelationId}", WorkloadId, correlationId);
            _workloadState = WorkloadState.Running;
            WorkloadResult<TPrerequisite> prerequisite = await PrerequisiteWorkloads.ExecuteAsync(correlationId, cancellationToken);
            
            // Start the actual workload and save the result
            _logger.LogInformation("Starting workload {WorkloadId} with correlation {CorrelationId}.", WorkloadId, correlationId);
            _result = await ExecuteWorkAsync(prerequisite, correlationId, cancellationToken);
            _workloadState = _result.IsSuccess ? WorkloadState.Completed : WorkloadState.Faulted;
        }
        catch (OperationCanceledException operationCanceledException)
        {
            // If cancelled, save the failed result
            _logger.LogWarning("Cancelled workload {WorkloadId} with correlation {CorrelationId}.", WorkloadId, correlationId);
            _result = WorkloadResult<TResult>.Failure(operationCanceledException, WorkloadId, correlationId);
            _workloadState = WorkloadState.Cancelled;
        }
        catch (Exception ex)
        {
            // If faulted, save the failed result as well
            _logger.LogError(ex, "Exception during workload {WorkloadId} with correlation {CorrelationId}.", WorkloadId, correlationId);
            _result = WorkloadResult<TResult>.Failure(ex, WorkloadId, correlationId);
            _workloadState = WorkloadState.Faulted;
        }
        return _result;
    }
}