namespace AsyncWorkloads.Results;

public readonly struct Result<TValue>
{
    private readonly TValue? _value;
    private readonly Exception? _exception;

    public bool IsSuccess => _exception is null;

    private Result(TValue? value, Exception? exception)
    {
        _value = value;
        _exception = exception;
    }

    public static Result<TValue> Success(TValue value)
        => new(value, null);
    
    public static Result<TValue> Failure(Exception exception)
        => new(default, exception);

    public TReturn Match<TReturn>(Func<TValue, TReturn> value, Func<Exception, TReturn> exception)
        => _exception is null
            ? value(_value!)
            : exception(_exception);
    
    public Result<TResult> Map<TResult>(Func<TValue, TResult> map)
        => _exception is null 
            ? Result<TResult>.Success(map(_value!))
            : Result<TResult>.Failure(_exception);

    public Result<TResult> Bind<TResult>(Func<TValue, Result<TResult>> bind)
        => _exception is null
            ? bind(_value!)
            : Result<TResult>.Failure(_exception);
}