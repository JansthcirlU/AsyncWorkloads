using AzureWorkloads;
using AzureWorkloads.Workloads.Experimental;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

HostApplicationBuilder builder = Host.CreateApplicationBuilder(args);
builder.Services
    .AddHostedService<AzureWorkerService>()
    .AddExperimentalWorkloads()
    .AddLogging(builder => builder.AddConsole());

IHost host = builder.Build();
await host.RunAsync();