using AsyncWorkloads.Results;
using AsyncWorkloads.Workloads;
using Microsoft.Extensions.Logging;

namespace AsyncWorkloads.Prerequisites;

public abstract class PrerequisiteAsyncWorkloads<TResult> :
    AsyncWorkload<TResult>,
    IPrerequisiteAsyncWorkloads<TResult>
{
    protected PrerequisiteAsyncWorkloads(ILogger<AsyncWorkload<TResult>> logger) : base(logger)
    {
    }
}

public abstract class PrerequisiteAsyncWorkloads<T1, TResult> :
    PrerequisiteAsyncWorkloads<TResult>,
    IPrerequisiteAsyncWorkloads<T1, TResult>
{
    public IAsyncWorkload<T1> FirstPrerequisite { get; }

    public PrerequisiteAsyncWorkloads(
        ILogger<AsyncWorkload<TResult>> logger,
        AsyncWorkload<T1> firstPrerequisite)
        : base(
            logger)
    {
        FirstPrerequisite = firstPrerequisite;
    }

    protected override Task<WorkloadResult<TResult>> ExecuteWorkAsync(CorrelationId correlationId, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}

public abstract class PrerequisiteAsyncWorkloads<T1, T2, TResult> :
    PrerequisiteAsyncWorkloads<T1, TResult>,
    IPrerequisiteAsyncWorkloads<T1, T2, TResult>
{
    public IAsyncWorkload<T2> SecondPrerequisite { get; }

    public PrerequisiteAsyncWorkloads(
        ILogger<AsyncWorkload<TResult>> logger,
        AsyncWorkload<T1> firstPrerequisite,
        AsyncWorkload<T2> secondPrerequisite)
        : base(
            logger,
            firstPrerequisite)
    {
        SecondPrerequisite = secondPrerequisite;
    }
}

public abstract class PrerequisiteAsyncWorkloads<T1, T2, T3, TResult> :
    PrerequisiteAsyncWorkloads<T1, T2, TResult>,
    IPrerequisiteAsyncWorkloads<T1, T2, T3, TResult>
{
    public IAsyncWorkload<T3> ThirdPrerequisite { get; }

    public PrerequisiteAsyncWorkloads(
        ILogger<AsyncWorkload<TResult>> logger,
        AsyncWorkload<T1> firstPrerequisite,
        AsyncWorkload<T2> secondPrerequisite,
        AsyncWorkload<T3> thirdPrerequisite)
        : base(
            logger,
            firstPrerequisite,
            secondPrerequisite)
    {
        ThirdPrerequisite = thirdPrerequisite;
    }
}

public abstract class PrerequisiteAsyncWorkloads<T1, T2, T3, T4, TResult> :
    PrerequisiteAsyncWorkloads<T1, T2, T3, TResult>,
    IPrerequisiteAsyncWorkloads<T1, T2, T3, T4, TResult>
{
    public IAsyncWorkload<T4> FourthPrerequisite { get; }

    public PrerequisiteAsyncWorkloads(
        ILogger<AsyncWorkload<TResult>> logger,
        AsyncWorkload<T1> firstPrerequisite,
        AsyncWorkload<T2> secondPrerequisite,
        AsyncWorkload<T3> thirdPrerequisite,
        AsyncWorkload<T4> fourthPrerequisite)
        : base(
            logger,
            firstPrerequisite,
            secondPrerequisite,
            thirdPrerequisite)
    {
        FourthPrerequisite = fourthPrerequisite;
    }
}

public abstract class PrerequisiteAsyncWorkloads<T1, T2, T3, T4, T5, TResult> :
    PrerequisiteAsyncWorkloads<T1, T2, T3, T4, TResult>,
    IPrerequisiteAsyncWorkloads<T1, T2, T3, T4, T5, TResult>
{
    public IAsyncWorkload<T5> FifthPrerequisite { get; }

    public PrerequisiteAsyncWorkloads(
        ILogger<AsyncWorkload<TResult>> logger,
        AsyncWorkload<T1> firstPrerequisite,
        AsyncWorkload<T2> secondPrerequisite,
        AsyncWorkload<T3> thirdPrerequisite,
        AsyncWorkload<T4> fourthPrerequisite,
        AsyncWorkload<T5> fifthPrerequisite)
        : base(
            logger,
            firstPrerequisite,
            secondPrerequisite,
            thirdPrerequisite,
            fourthPrerequisite)
    {
        FifthPrerequisite = fifthPrerequisite;
    }
}