using AsyncWorkloads.Results;
using AsyncWorkloads.Workloads;
using AzureWorkloads.Workloads.Experimental;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace AzureWorkloads;

public class AzureWorkerService : BackgroundService
{          
    private readonly ILogger<AzureWorkerService> _logger;
    private readonly LogEvenRepetitionsWorkload _logEvenRepetitionsWorkload;

    public AzureWorkerService(
        ILogger<AzureWorkerService> logger,
        LogEvenRepetitionsWorkload logEvenRepetitionsWorkload)
    {
        _logger = logger;
        _logEvenRepetitionsWorkload = logEvenRepetitionsWorkload;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        _logger.LogInformation("Starting the worker.");
        CorrelationId correlationId = CorrelationId.Create();
        WorkloadResult<string> logEvenRepetitionsResult = await _logEvenRepetitionsWorkload.ExecuteAsync(correlationId, stoppingToken);
        logEvenRepetitionsResult
            .Match(
                value =>
                {
                    Console.WriteLine(value);
                    return value;
                },
                exception =>
                {
                    Console.WriteLine(exception.Message);
                    return exception.Message;
                }
            );
        _logger.LogInformation("Worker finished.");
    }
}