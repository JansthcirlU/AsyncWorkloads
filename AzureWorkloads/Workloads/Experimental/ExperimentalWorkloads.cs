using AsyncWorkloads.Results;
using AsyncWorkloads.Workloads;
using Microsoft.Extensions.Logging;

namespace AzureWorkloads.Workloads.Experimental;

public sealed class GetAnimalNameWorkload : AsyncWorkload<string>
{
    public GetAnimalNameWorkload(ILogger<AsyncWorkload<string>> logger) : base("Get animal name", logger)
    {
    }

    protected override Task<WorkloadResult<string>> ExecuteWorkAsync(CorrelationId correlationId, CancellationToken cancellationToken)
    {
        List<string> animals = [
            "Monkey",
            "Dog",
            "Cat",
            "Elephant"
        ];
        string choice = animals[Random.Shared.Next(animals.Count)];
        WorkloadResult<string> result = Success(choice, correlationId);
        return Task.FromResult(result);
    }
}

public sealed class GetNumberWorkload : AsyncWorkload<int>
{
    public GetNumberWorkload(ILogger<AsyncWorkload<int>> logger) : base("Get number", logger)
    {
    }

    protected override Task<WorkloadResult<int>> ExecuteWorkAsync(CorrelationId correlationId, CancellationToken cancellationToken)
    {
        int number = Random.Shared.Next(1, 11);
        WorkloadResult<int> result = Success(number, correlationId);
        return Task.FromResult(result);
    }
}

public sealed class LogAnimalToFileWorkload : AsyncWorkload<string>
{
    private readonly GetAnimalNameWorkload _getAnimalNameWorkload;

    public LogAnimalToFileWorkload(
        ILogger<AsyncWorkload<string>> logger,
        GetAnimalNameWorkload getAnimalNameWorkload) : base("Log animal to file", logger)
    {
        _getAnimalNameWorkload = getAnimalNameWorkload;
    }

    protected override async Task<WorkloadResult<string>> ExecuteWorkAsync(CorrelationId correlationId, CancellationToken cancellationToken)
    {
        WorkloadResult<string> animalNameResult = await _getAnimalNameWorkload.ExecuteAsync(correlationId, cancellationToken);
        string animalName = animalNameResult.Match(
            animalName => animalName,
            exception => throw exception
        );
        await File.WriteAllTextAsync("animal-name.txt", animalName, cancellationToken);
        return animalNameResult;
    }
}

public sealed class RepeatAnimalNameWorkload : AsyncWorkload<string>
{
    private readonly GetAnimalNameWorkload _getAnimalNameWorkload;
    private readonly GetNumberWorkload _getNumberWorkload;

    public RepeatAnimalNameWorkload(
        ILogger<AsyncWorkload<string>> logger,
        GetAnimalNameWorkload getAnimalNameWorkload,
        GetNumberWorkload getNumberWorkload) : base("Repeat animal name", logger)
    {
        _getAnimalNameWorkload = getAnimalNameWorkload;
        _getNumberWorkload = getNumberWorkload;
    }

    protected override async Task<WorkloadResult<string>> ExecuteWorkAsync(CorrelationId correlationId, CancellationToken cancellationToken)
    {
        WorkloadResult<string> animalNameResult = await _getAnimalNameWorkload.ExecuteAsync(correlationId, cancellationToken);
        WorkloadResult<int> repetitionsResult = await _getNumberWorkload.ExecuteAsync(correlationId, cancellationToken);
        WorkloadResult<string> repeatedAnimalNameResult = animalNameResult
            .CombineWith(
                repetitionsResult,
                (name, repetitions) => string.Join(", ", Enumerable.Range(0, repetitions).Select(_ => name)),
                WorkloadId,
                correlationId);
        return repeatedAnimalNameResult;
    }
}

public sealed class CheckIfNumberIsEvenWorkload : AsyncWorkload<bool>
{
    private readonly GetNumberWorkload _getNumberWorkload;

    public CheckIfNumberIsEvenWorkload(
        ILogger<AsyncWorkload<bool>> logger,
        GetNumberWorkload getNumberWorkload) : base("Check if number is even", logger)
    {
        _getNumberWorkload = getNumberWorkload;
    }

    protected override async Task<WorkloadResult<bool>> ExecuteWorkAsync(CorrelationId correlationId, CancellationToken cancellationToken)
    {
        WorkloadResult<int> numberResult = await _getNumberWorkload.ExecuteAsync(correlationId, cancellationToken);
        WorkloadResult<bool> isEvenResult = numberResult
            .Bind(number => Success(number % 2 == 0, correlationId),
            WorkloadId,
            correlationId);
        return isEvenResult;
    }
}

public sealed class LogEvenRepetitionsWorkload : AsyncWorkload<string>
{
    private readonly RepeatAnimalNameWorkload _repeatAnimalNameWorkload;
    private readonly CheckIfNumberIsEvenWorkload _checkIfNumberIsEvenWorkload;

    public LogEvenRepetitionsWorkload(
        ILogger<AsyncWorkload<string>> logger,
        RepeatAnimalNameWorkload repeatAnimalNameWorkload,
        CheckIfNumberIsEvenWorkload checkIfNumberIsEvenWorkload) : base("Log even repetitions", logger)
    {
        _repeatAnimalNameWorkload = repeatAnimalNameWorkload;
        _checkIfNumberIsEvenWorkload = checkIfNumberIsEvenWorkload;
    }

    protected override async Task<WorkloadResult<string>> ExecuteWorkAsync(CorrelationId correlationId, CancellationToken cancellationToken)
    {
        WorkloadResult<string> animalNamesResult = await _repeatAnimalNameWorkload.ExecuteAsync(correlationId, cancellationToken);
        if (!animalNamesResult.IsSuccess) return Failure<string>(animalNamesResult.GetExceptionOrDefault()!, correlationId);

        WorkloadResult<bool> checkIfNumberIsEvenResult = await _checkIfNumberIsEvenWorkload.ExecuteAsync(correlationId, cancellationToken);
        if (!checkIfNumberIsEvenResult.IsSuccess) return Failure<string>(checkIfNumberIsEvenResult.GetExceptionOrDefault()!, correlationId);
        
        string animalNames = animalNamesResult.GetValueOrDefault()!;
        bool isEven = checkIfNumberIsEvenResult.GetValueOrDefault()!;
        await File.WriteAllTextAsync($"animal-names-{(isEven ? "even" : "odd")}.txt", animalNames, cancellationToken);
        return animalNamesResult;
    }
}