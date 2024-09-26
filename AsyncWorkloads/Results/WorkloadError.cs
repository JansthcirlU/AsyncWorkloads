namespace AsyncWorkloads.Results;

public readonly record struct WorkloadError
{
    public Exception Exception { get; }
    public Guid WorkloadId { get; }
    public Guid CorrelationId { get; }

    public WorkloadError(
        Exception exception,
        Guid workloadId,
        Guid correlationId)
    {
        Exception = exception;
        WorkloadId = workloadId;
        CorrelationId = correlationId;
    }
}