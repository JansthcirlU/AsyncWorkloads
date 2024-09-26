namespace AsyncWorkloads.Workloads;

public record WorkloadId
{
    public Guid Id { get; }

    private WorkloadId(Guid id)
    {
        Id = id;
    }

    public static WorkloadId Create()
        => new(Guid.NewGuid());
}
