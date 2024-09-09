using AzureWorkloads;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

HostApplicationBuilder builder = Host.CreateApplicationBuilder(args);
builder.Services.AddHostedService<AzureWorkerService>();

IHost host = builder.Build();
await host.RunAsync();