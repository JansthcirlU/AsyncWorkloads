using Microsoft.Extensions.DependencyInjection;

namespace AzureWorkloads.Workloads.Experimental;

public static class ExperimentalWorkloadsDependencyInjection
{
    public static IServiceCollection AddExperimentalWorkloads(this IServiceCollection services)
        => services
            .AddScoped<GetAnimalNameWorkload>()
            .AddScoped<GetNumberWorkload>()
            .AddScoped<LogAnimalToFileWorkload>()
            .AddScoped<RepeatAnimalNameWorkload>()
            .AddScoped<CheckIfNumberIsEvenWorkload>()
            .AddScoped<LogEvenRepetitionsWorkload>();
}