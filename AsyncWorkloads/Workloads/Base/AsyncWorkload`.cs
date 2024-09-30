using AsyncWorkloads.Results;
using Microsoft.Extensions.Logging;

namespace AsyncWorkloads.Workloads.Base;

/// <summary>
/// Abstract class encapsulating the fundamental execution logic of an asynchronous workflow.
/// </summary>
/// <typeparam name="TResult">Expected result from running the async workload.</typeparam>
public abstract class AsyncWorkload<TResult>
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

    /// <summary>
    /// Executes the core logic of the workload asynchronously. 
    /// This method is responsible for performing the actual work and must be implemented by derived classes.
    /// </summary>
    /// <param name="correlationId">Unique identifier to track the workload execution context.</param>
    /// <param name="cancellationToken">Token to cancel the execution if needed.</param>
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

    protected WorkloadResult<TValue> Success<TValue>(TValue @value, CorrelationId correlationId)
        => WorkloadResult<TValue>.Success(@value, WorkloadId, correlationId);
    
    protected WorkloadResult<TValue> Failure<TValue>(Exception exception, CorrelationId correlationId)
        => WorkloadResult<TValue>.Failure(exception, WorkloadId, correlationId);
}