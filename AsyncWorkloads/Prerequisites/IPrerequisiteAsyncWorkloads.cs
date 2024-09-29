using AsyncWorkloads.Workloads;

namespace AsyncWorkloads.Prerequisites;

/// <summary>
/// Marker interface for prerequisite workloads which need to run before the actual workload runs.
/// </summary>
/// <typeparam name="TResult">Expected combined result after running all the prerequisite workloads.</typeparam>
public interface IPrerequisiteAsyncWorkloads<TResult> : IAsyncWorkload<TResult>
{

}

/// <summary>
/// Interface for prerequisite workloads containing one prerequisite.
/// </summary>
/// <typeparam name="T1">Expected result from the first prerequisite workload.</typeparam>
/// <typeparam name="TResult">Expected combined result after running all the prerequisite workloads.</typeparam>
internal interface IPrerequisiteAsyncWorkloads<T1, TResult> : IPrerequisiteAsyncWorkloads<TResult>
{
    IAsyncWorkload<T1> FirstPrerequisite { get; }
}

/// <summary>
/// 
/// </summary>
/// <typeparam name="T1">Expected result from the first prerequisite workload.</typeparam>
/// <typeparam name="T2">Expected result from the second prerequisite workload.</typeparam>
/// <typeparam name="TResult">Expected combined result after running all the prerequisite workloads.</typeparam>
internal interface IPrerequisiteAsyncWorkloads<T1, T2, TResult> : IPrerequisiteAsyncWorkloads<T1, TResult>
{
    IAsyncWorkload<T2> SecondPrerequisite { get; }
}

/// <summary>
/// 
/// </summary>
/// <typeparam name="T1">Expected result from the first prerequisite workload.</typeparam>
/// <typeparam name="T2">Expected result from the second prerequisite workload.</typeparam>
/// <typeparam name="T3">Expected result from the third prerequisite workload.</typeparam>
/// <typeparam name="TResult">Expected combined result after running all the prerequisite workloads.</typeparam>
internal interface IPrerequisiteAsyncWorkloads<T1, T2, T3, TResult> : IPrerequisiteAsyncWorkloads<T1, T2, TResult>
{
    IAsyncWorkload<T3> ThirdPrerequisite { get; }
}

/// <summary>
/// 
/// </summary>
/// <typeparam name="T1">Expected result from the first prerequisite workload.</typeparam>
/// <typeparam name="T2">Expected result from the second prerequisite workload.</typeparam>
/// <typeparam name="T3">Expected result from the third prerequisite workload.</typeparam>
/// <typeparam name="T4">Expected result from the fourth prerequisite workload.</typeparam>
/// <typeparam name="TResult">Expected combined result after running all the prerequisite workloads.</typeparam>
internal interface IPrerequisiteAsyncWorkloads<T1, T2, T3, T4, TResult> : IPrerequisiteAsyncWorkloads<T1, T2, T3, TResult>
{
    IAsyncWorkload<T4> FourthPrerequisite { get; }
}

/// <summary>
/// 
/// </summary>
/// <typeparam name="T1">Expected result from the first prerequisite workload.</typeparam>
/// <typeparam name="T2">Expected result from the second prerequisite workload.</typeparam>
/// <typeparam name="T3">Expected result from the third prerequisite workload.</typeparam>
/// <typeparam name="T4">Expected result from the fourth prerequisite workload.</typeparam>
/// <typeparam name="T5">Expected result from the fifth prerequisite workload.</typeparam>
/// <typeparam name="TResult">Expected combined result after running all the prerequisite workloads.</typeparam>
internal interface IPrerequisiteAsyncWorkloads<T1, T2, T3, T4, T5, TResult> : IPrerequisiteAsyncWorkloads<T1, T2, T3, T4, TResult>
{
    IAsyncWorkload<T5> FifthPrerequisite { get; }
}