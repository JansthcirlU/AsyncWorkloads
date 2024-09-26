using AsyncWorkloads.Workloads;

namespace AsyncWorkloads.Results;

public readonly record struct WorkloadError
{
    public Exception Exception { get; }
    public WorkloadId WorkloadId { get; }
    public CorrelationId CorrelationId { get; }

    public WorkloadError(
        Exception exception,
        WorkloadId workloadId,
        CorrelationId correlationId)
    {
        Exception = exception;
        WorkloadId = workloadId;
        CorrelationId = correlationId;
    }
}