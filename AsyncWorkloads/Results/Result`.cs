using AsyncWorkloads.Workloads;

namespace AsyncWorkloads.Results;

public readonly struct Result<TValue>
{
    private readonly TValue? _value;
    private readonly WorkloadError? _error;

    public bool IsSuccess => _error is null;

    private Result(TValue? value, WorkloadError? error)
    {
        _value = value;
        _error = error;
    }

    public static Result<TValue> Success(TValue value)
        => new(value, null);
    
    public static Result<TValue> Failure(WorkloadError error)
        => new(default, error);

    public static Result<TValue> Failure(Exception exception, WorkloadId workloadId, CorrelationId correlationId)
        => Failure(new(exception, workloadId, correlationId));

    public TReturn Match<TReturn>(Func<TValue, TReturn> value, Func<WorkloadError, TReturn> error)
        => _error is null
            ? value(_value!)
            : error(_error);
    
    public Result<TResult> Map<TResult>(Func<TValue, TResult> map)
        => _error is null 
            ? Result<TResult>.Success(map(_value!))
            : Result<TResult>.Failure(_error);

    public Result<TResult> Bind<TResult>(Func<TValue, Result<TResult>> bind)
        => _error is null
            ? bind(_value!)
            : Result<TResult>.Failure(_error);
}