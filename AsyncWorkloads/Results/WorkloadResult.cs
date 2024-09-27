using AsyncWorkloads.Workloads;

namespace AsyncWorkloads.Results;

/// <summary>
/// Represents the result of a workload, containing either a value on success or an error on failure.
/// </summary>
/// <typeparam name="TValue">The type of the result value if the workload succeeds.</typeparam>
public record WorkloadResult<TValue>
{
    private readonly TValue? _value;
    private readonly Exception? _exception;

    /// <summary>
    /// Identifier of the workload that yielded this result.
    /// </summary>
    /// <value></value>
    public WorkloadId WorkloadId { get; }

    /// <summary>
    /// Identifier of the context in which the result was yielded.
    /// </summary>
    /// <value></value>
    public CorrelationId CorrelationId { get; }

    /// <summary>
    /// Indicates whether the workload result has a value or an error.
    /// </summary>
    public bool IsSuccess => _exception is null;

    private WorkloadResult(TValue? value, Exception? exception, WorkloadId workloadId, CorrelationId correlationId)
    {
        _value = value;
        _exception = exception;
        WorkloadId = workloadId;
        CorrelationId = correlationId;
    }

    /// <summary>
    /// Creates a successful result containing the provided value.
    /// </summary>
    public static WorkloadResult<TValue> Success(TValue value, WorkloadId workloadId, CorrelationId correlationId)
        => new(value, null, workloadId, correlationId);

    /// <summary>
    /// Creates a failure result from an exception, workload identifier and correlation identifier.
    /// </summary>
    public static WorkloadResult<TValue> Failure(Exception exception, WorkloadId workloadId, CorrelationId correlationId)
        => new(default, exception, workloadId, correlationId);

    /// <summary>
    /// Evaluates the result and returns either the success value or the error using the provided functions.
    /// </summary>
    public TReturn Match<TReturn>(Func<TValue, TReturn> value, Func<Exception, TReturn> error)
        => _exception is null
            ? value(_value!)
            : error(_exception);
    
    /// <summary>
    /// Transforms the success value using the provided mapping function, or returns the error unchanged.
    /// </summary>
    public WorkloadResult<TResult> Map<TResult>(Func<TValue, TResult> map, WorkloadId workloadId, CorrelationId correlationId)
    {
        try
        {
            return _exception is null 
                ? WorkloadResult<TResult>.Success(map(_value!), workloadId, correlationId)
                : WorkloadResult<TResult>.Failure(_exception, workloadId, correlationId);
        }
        catch (Exception ex)
        {
            return WorkloadResult<TResult>.Failure(ex, workloadId, correlationId);
        }
    }

    /// <summary>
    /// Binds the success value to a new result using the provided binding function, or returns the error unchanged.
    /// </summary>
    public WorkloadResult<TResult> Bind<TResult>(Func<TValue, WorkloadResult<TResult>> bind)
    {
        try
        {
            return _exception is null
                ? bind(_value!)
                : WorkloadResult<TResult>.Failure(_exception, WorkloadId, CorrelationId);
        }
        catch (Exception ex)
        {
            return WorkloadResult<TResult>.Failure(ex, WorkloadId, CorrelationId);
        }
    }
}