using AsyncWorkloads.Results;
using AzureWorkloads.Workloads.CheckAzLogin;
using AzureWorkloads.Workloads.FetchAzureSubscriptionInfo;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace AzureWorkloads;

public class AzureWorkerService : BackgroundService
{
    private readonly ILogger<AzureWorkerService> _logger;

    public AzureWorkerService(ILogger<AzureWorkerService> logger)
    {
        _logger = logger;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        CheckAzLoginWorkload checkAzLoginWorkload = new();
        FetchAzureSubscriptionInfoWorkloadPrerequisite fetchAzureSubscriptionInfoWorkloadPrerequisite = new(checkAzLoginWorkload);
        FetchAzureSubscriptionInfoWorkload fetchAzureSubscriptionInfoWorkload = new(fetchAzureSubscriptionInfoWorkloadPrerequisite);
        Result<string> subscription = await fetchAzureSubscriptionInfoWorkload.ExecuteAsync(stoppingToken);
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