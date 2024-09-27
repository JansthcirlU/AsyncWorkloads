using AsyncWorkloads.Workloads;

namespace AsyncWorkloads.Results;

public record WorkloadResult<TValue>
{
    private readonly TValue? _value;
    private readonly WorkloadError? _error;

    public bool IsSuccess => _error is null;

    private WorkloadResult(TValue? value, WorkloadError? error)
    {
        _value = value;
        _error = error;
    }

    public static WorkloadResult<TValue> Success(TValue value)
        => new(value, null);
    
    public static WorkloadResult<TValue> Failure(WorkloadError error)
        => new(default, error);

    public static WorkloadResult<TValue> Failure(Exception exception, WorkloadId workloadId, CorrelationId correlationId)
        => Failure(new(exception, workloadId, correlationId));

    public TReturn Match<TReturn>(Func<TValue, TReturn> value, Func<WorkloadError, TReturn> error)
        => _error is null
            ? value(_value!)
            : error(_error);
    
    public WorkloadResult<TResult> Map<TResult>(Func<TValue, TResult> map)
        => _error is null 
            ? WorkloadResult<TResult>.Success(map(_value!))
            : WorkloadResult<TResult>.Failure(_error);

    public WorkloadResult<TResult> Bind<TResult>(Func<TValue, WorkloadResult<TResult>> bind)
        => _error is null
            ? bind(_value!)
            : WorkloadResult<TResult>.Failure(_error);
}