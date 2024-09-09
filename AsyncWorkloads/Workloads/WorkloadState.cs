namespace AsyncWorkloads.Workloads;

public enum WorkloadState
{
    Undefined,
    Queued,
    Running,
    Completed,
    Faulted,
    Cancelled
}