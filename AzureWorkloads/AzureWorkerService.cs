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

    public AzureWorkerService(ILogger<AzureWorkerService> logger)
    {
        _logger = logger;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        // Enable logging
        using ILoggerFactory factory = LoggerFactory.Create(builder => builder.AddConsole());
        ILogger<CheckAzLoginWorkload> checkAzLoginLogger = factory.CreateLogger<CheckAzLoginWorkload>();
        ILogger<FetchAzureSubscriptionInfoWorkload> fetchSubscriptionInfoLogger = factory.CreateLogger<FetchAzureSubscriptionInfoWorkload>();
        
        CorrelationId correlationId = CorrelationId.Create();
        CheckAzLoginWorkload checkAzLoginWorkload = new(checkAzLoginLogger);
        FetchAzureSubscriptionInfoWorkload fetchAzureSubscriptionInfoWorkload = new(fetchSubscriptionInfoLogger, checkAzLoginWorkload);
        WorkloadResult<string> subscription = await fetchAzureSubscriptionInfoWorkload.ExecuteAsync(correlationId, stoppingToken);
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