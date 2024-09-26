namespace AsyncWorkloads.Workloads;

public record CorrelationId
{
    public Guid Id { get; }

    private CorrelationId(Guid id)
    {
        Id = id;
    }

    public static CorrelationId Create()
        => new(Guid.NewGuid());
}