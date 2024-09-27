using AsyncWorkloads.Workloads;

namespace AsyncWorkloads.Results;

/// <summary>
/// Represents the result of a workload, containing either a value on success or an error on failure.
/// </summary>
/// <typeparam name="TValue">The type of the result value if the workload succeeds.</typeparam>
public record WorkloadResult<TValue>
{
    private readonly TValue? _value;
    private readonly WorkloadError? _error;

    /// <summary>
    /// Indicates whether the workload completed successfully.
    /// </summary>
    public bool IsSuccess => _error is null;

    private WorkloadResult(TValue? value, WorkloadError? error)
    {
        _value = value;
        _error = error;
    }

    /// <summary>
    /// Creates a successful result containing the provided value.
    /// </summary>
    public static WorkloadResult<TValue> Success(TValue value)
        => new(value, null);
    
    /// <summary>
    /// Creates a failure result containing the given error information.
    /// </summary>
    public static WorkloadResult<TValue> Failure(WorkloadError error)
        => new(default, error);

    /// <summary>
    /// Creates a failure result from an exception, workload identifier and correlation identifier.
    /// </summary>
    public static WorkloadResult<TValue> Failure(Exception exception, WorkloadId workloadId, CorrelationId correlationId)
        => Failure(new(exception, workloadId, correlationId));

    /// <summary>
    /// Evaluates the result and returns either the success value or the error using the provided functions.
    /// </summary>
    public TReturn Match<TReturn>(Func<TValue, TReturn> value, Func<WorkloadError, TReturn> error)
        => _error is null
            ? value(_value!)
            : error(_error);
    
    /// <summary>
    /// Transforms the success value using the provided mapping function, or returns the error unchanged.
    /// </summary>
    public WorkloadResult<TResult> Map<TResult>(Func<TValue, TResult> map)
        => _error is null 
            ? WorkloadResult<TResult>.Success(map(_value!))
            : WorkloadResult<TResult>.Failure(_error);

    /// <summary>
    /// Binds the success value to a new result using the provided binding function, or returns the error unchanged.
    /// </summary>
    public WorkloadResult<TResult> Bind<TResult>(Func<TValue, WorkloadResult<TResult>> bind)
        => _error is null
            ? bind(_value!)
            : WorkloadResult<TResult>.Failure(_error);
}