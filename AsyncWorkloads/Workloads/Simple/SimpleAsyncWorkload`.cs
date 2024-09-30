using AsyncWorkloads.Workloads.Base;
using Microsoft.Extensions.Logging;

namespace AsyncWorkloads.Workloads.Simple;

/// <summary>
/// Abstract class encapsulating the fundamental execution logic of a simple asynchronous workflow.
/// </summary>
/// <typeparam name="TResult">Expected result from running the async workload.</typeparam>
public abstract class SimpleAsyncWorkload<TResult> : AsyncWorkload<TResult>
{
    protected SimpleAsyncWorkload(ILogger<SimpleAsyncWorkload<TResult>> logger) : base(logger)
    {
    }
}