using AsyncWorkloads.Workloads;

namespace AsyncWorkloads.Results;

/// <summary>
/// Represents an error that occurred during a workload, including the associated exception, workload, and correlation identifiers.
/// </summary>
public record WorkloadError
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