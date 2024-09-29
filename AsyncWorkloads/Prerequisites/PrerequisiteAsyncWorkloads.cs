using AsyncWorkloads.Workloads;
using Microsoft.Extensions.Logging;

namespace AsyncWorkloads.Prerequisites;

/// <summary>
/// Abstract class encapsulating the execution logic of an asynchronous prerequisite workflow.
/// </summary>
/// <typeparam name="TResult">Expected combined result after running all the prerequisite workloads.</typeparam>
internal abstract class PrerequisiteAsyncWorkloads<TResult> :
    AsyncWorkload<TResult>,
    IPrerequisiteAsyncWorkloads<TResult>
{
    protected PrerequisiteAsyncWorkloads(ILogger<AsyncWorkload<TResult>> logger) : base(logger)
    {
    }
}

/// <summary>
/// Abstract class encapsulating the execution logic of an asynchronous prerequisite workflow with one prerequisite.
/// </summary>
/// <typeparam name="T1">Expected result from running the first prerequisite.</typeparam>
/// <typeparam name="TResult">Expected combined result after running all the prerequisite workloads.</typeparam>
internal abstract class PrerequisiteAsyncWorkloads<T1, TResult> :
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
}

/// <summary>
/// Abstract class encapsulating the execution logic of an asynchronous prerequisite workflow with two prerequisites.
/// </summary>
/// <typeparam name="T1">Expected result from running the first prerequisite workload.</typeparam>
/// <typeparam name="T2">Expected result from running the second prerequisite workload.</typeparam>
/// <typeparam name="TResult">Expected combined result after running all the prerequisite workloads.</typeparam>
internal abstract class PrerequisiteAsyncWorkloads<T1, T2, TResult> :
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

/// <summary>
/// Abstract class encapsulating the execution logic of an asynchronous prerequisite workflow with three prerequisites.
/// </summary>
/// <typeparam name="T1">Expected result from running the first prerequisite workload.</typeparam>
/// <typeparam name="T2">Expected result from running the second prerequisite workload.</typeparam>
/// <typeparam name="T3">Expected result from running the third prerequisite workload.</typeparam>
/// <typeparam name="TResult">Expected combined result after running all the prerequisite workloads.</typeparam>
internal abstract class PrerequisiteAsyncWorkloads<T1, T2, T3, TResult> :
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

/// <summary>
/// Abstract class encapsulating the execution logic of an asynchronous prerequisite workflow with four prerequisites.
/// </summary>
/// <typeparam name="T1">Expected result from running the first prerequisite workload.</typeparam>
/// <typeparam name="T2">Expected result from running the second prerequisite workload.</typeparam>
/// <typeparam name="T3">Expected result from running the third prerequisite workload.</typeparam>
/// <typeparam name="T4">Expected result from running the fourth prerequisite workload.</typeparam>
/// <typeparam name="TResult">Expected combined result after running all the prerequisite workloads.</typeparam>
internal abstract class PrerequisiteAsyncWorkloads<T1, T2, T3, T4, TResult> :
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

/// <summary>
/// Abstract class encapsulating the execution logic of an asynchronous prerequisite workflow with five prerequisites.
/// </summary>
/// <typeparam name="T1">Expected result from running the first prerequisite workload.</typeparam>
/// <typeparam name="T2">Expected result from running the second prerequisite workload.</typeparam>
/// <typeparam name="T3">Expected result from running the third prerequisite workload.</typeparam>
/// <typeparam name="T4">Expected result from running the fourth prerequisite workload.</typeparam>
/// <typeparam name="T5">Expected result from running the fifth prerequisite workload.</typeparam>
/// <typeparam name="TResult">Expected combined result after running all the prerequisite workloads.</typeparam>
internal abstract class PrerequisiteAsyncWorkloads<T1, T2, T3, T4, T5, TResult> :
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