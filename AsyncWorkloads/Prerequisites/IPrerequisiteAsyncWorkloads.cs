using AsyncWorkloads.Workloads;

namespace AsyncWorkloads.Prerequisites;

public interface IPrerequisiteAsyncWorkloads<TResult> : IAsyncWorkload<TResult>
{

}

public interface IPrerequisiteAsyncWorkloads<T1, TResult> : IPrerequisiteAsyncWorkloads<TResult>
{
    IAsyncWorkload<T1> FirstPrerequisite { get; }
}

public interface IPrerequisiteAsyncWorkloads<T1, T2, TResult> : IPrerequisiteAsyncWorkloads<T1, TResult>
{
    IAsyncWorkload<T2> SecondPrerequisite { get; }
}

public interface IPrerequisiteAsyncWorkloads<T1, T2, T3, TResult> : IPrerequisiteAsyncWorkloads<T1, T2, TResult>
{
    IAsyncWorkload<T3> ThirdPrerequisite { get; }
}

public interface IPrerequisiteAsyncWorkloads<T1, T2, T3, T4, TResult> : IPrerequisiteAsyncWorkloads<T1, T2, T3, TResult>
{
    IAsyncWorkload<T4> FourthPrerequisite { get; }
}

public interface IPrerequisiteAsyncWorkloads<T1, T2, T3, T4, T5, TResult> : IPrerequisiteAsyncWorkloads<T1, T2, T3, T4, TResult>
{
    IAsyncWorkload<T5> FifthPrerequisite { get; }
}