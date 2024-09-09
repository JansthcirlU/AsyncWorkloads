using AsyncWorkloads.Workloads;

namespace AsyncWorkloads.Prerequisites;

public abstract class PrerequisiteAsyncWorkloads<TResult> : 
    AsyncWorkload<TResult>,
    IPrerequisiteAsyncWorkloads<TResult>
{

}

public abstract class PrerequisiteAsyncWorkloads<T1, TResult> :
    PrerequisiteAsyncWorkloads<TResult>,
    IPrerequisiteAsyncWorkloads<T1, TResult>
{
    public IAsyncWorkload<T1> FirstPrerequisite { get; }

    public PrerequisiteAsyncWorkloads(AsyncWorkload<T1> firstPrerequisite)
    {
        FirstPrerequisite = firstPrerequisite;
    }
}

public abstract class PrerequisiteAsyncWorkloads<T1, T2, TResult> :
    PrerequisiteAsyncWorkloads<T1, TResult>,
    IPrerequisiteAsyncWorkloads<T1, T2, TResult>
{
    public IAsyncWorkload<T2> SecondPrerequisite { get; }

    public PrerequisiteAsyncWorkloads(
        AsyncWorkload<T1> firstPrerequisite,
        AsyncWorkload<T2> secondPrerequisite)
        : base(firstPrerequisite)
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
        AsyncWorkload<T1> firstPrerequisite,
        AsyncWorkload<T2> secondPrerequisite,
        AsyncWorkload<T3> thirdPrerequisite)
        : base(
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
        AsyncWorkload<T1> firstPrerequisite,
        AsyncWorkload<T2> secondPrerequisite,
        AsyncWorkload<T3> thirdPrerequisite,
        AsyncWorkload<T4> fourthPrerequisite)
        : base(
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
        AsyncWorkload<T1> firstPrerequisite,
        AsyncWorkload<T2> secondPrerequisite,
        AsyncWorkload<T3> thirdPrerequisite,
        AsyncWorkload<T4> fourthPrerequisite,
        AsyncWorkload<T5> fifthPrerequisite)
        : base(
            firstPrerequisite,
            secondPrerequisite,
            thirdPrerequisite,
            fourthPrerequisite)
    {
        FifthPrerequisite = fifthPrerequisite;
    }
}