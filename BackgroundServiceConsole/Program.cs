using AzureWorkloads;
using AzureWorkloads.Workloads.CheckAzLogin;
using AzureWorkloads.Workloads.FetchAzureSubscriptionInfo;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

HostApplicationBuilder builder = Host.CreateApplicationBuilder(args);
builder.Services
    .AddHostedService<AzureWorkerService>()
    .AddSingleton<CheckAzLoginWorkload>()
    .AddSingleton<FetchAzureSubscriptionInfoWorkloadPrerequisite>()
    .AddSingleton<FetchAzureSubscriptionInfoWorkload>()
    .AddLogging(builder => builder.AddConsole());

IHost host = builder.Build();
await host.RunAsync();