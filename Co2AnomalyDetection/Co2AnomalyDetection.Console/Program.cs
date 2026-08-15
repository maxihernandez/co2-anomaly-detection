using Co2AnomalyDetection.Application.Abstractions;
using Co2AnomalyDetection.Application.Detectors;
using Co2AnomalyDetection.Application.Services;
using Co2AnomalyDetection.Domain.Entities;
using System.Text.Json;
using System.Text.Json.Serialization;

try
{
    var filePath = Path.Combine(AppContext.BaseDirectory, "data.json");

    if (!File.Exists(filePath))
    {
        Console.Error.WriteLine("Input file 'data.json' was not found.");
        return;
    }

    var json = File.ReadAllText(filePath);

    var records = JsonSerializer.Deserialize<List<EmissionRecord>>(
        json,
        new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        });

    if (records is null || records.Count == 0)
    {
        Console.Error.WriteLine("No emission records were found.");
        return;
    }

    IReadOnlyCollection<IAnomalyDetector> detectors =
    [
        new InvalidDataDetector(),
        new EnergyConsumptionDetector(),
        new EmissionFactorDetector()
    ];

    var service = new AnomalyDetectionService(detectors);

    var results = service.Analyze(records);

    var outputOptions = new JsonSerializerOptions
    {
        WriteIndented = true,
        Converters =
        {
            new JsonStringEnumConverter()
        }
    };

    Console.WriteLine(
        JsonSerializer.Serialize(results, outputOptions));
}
catch (JsonException ex)
{
    Console.Error.WriteLine($"Invalid JSON input: {ex.Message}");
}
catch (IOException ex)
{
    Console.Error.WriteLine($"Error reading input file: {ex.Message}");
}
catch (Exception ex)
{
    Console.Error.WriteLine($"Unexpected error: {ex.Message}");
}