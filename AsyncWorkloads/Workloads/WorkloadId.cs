namespace AsyncWorkloads.Workloads;

public readonly record struct WorkloadId
{
    public Guid Id { get; }

    private WorkloadId(Guid id)
        => Id = id;

    public static WorkloadId Create()
        => new(Guid.NewGuid());
}
