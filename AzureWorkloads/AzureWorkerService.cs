using AsyncWorkloads.Results;
using AsyncWorkloads.Workloads;
using AzureWorkloads.Workloads.CheckAzLogin;
using AzureWorkloads.Workloads.FetchAzureSubscriptionInfo;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace AzureWorkloads;

public class AzureWorkerService : BackgroundService
{
    private readonly ILogger<AzureWorkerService> _logger;
    private readonly FetchAzureSubscriptionInfoWorkload _fetchAzureSubscriptionInfoWorkload;

    public AzureWorkerService(
        ILogger<AzureWorkerService> logger,
        FetchAzureSubscriptionInfoWorkload fetchAzureSubscriptionInfoWorkload)
    {
        _logger = logger;
        _fetchAzureSubscriptionInfoWorkload = fetchAzureSubscriptionInfoWorkload;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        CorrelationId correlationId = CorrelationId.Create();
        WorkloadResult<string> subscription = await _fetchAzureSubscriptionInfoWorkload.ExecuteAsync(correlationId, stoppingToken);
        string test = subscription
            .Match(
                value => 
                {
                    _logger.LogInformation("Subscription found: {Subscription}", value);
                    return value;
                },
                exception =>
                {
                    _logger.LogError("An exception occurred: {Message}", exception.Message);
                    return exception.Message;
                }
            );
    }
}