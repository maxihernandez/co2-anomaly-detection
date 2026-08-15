using Co2AnomalyDetection.Application.Abstractions;
using Co2AnomalyDetection.Domain.Entities;
using Co2AnomalyDetection.Domain.Models;

namespace Co2AnomalyDetection.Application.Services;

public sealed class AnomalyDetectionService
{
    private readonly IReadOnlyCollection<IAnomalyDetector> _detectors;

    public AnomalyDetectionService(
        IReadOnlyCollection<IAnomalyDetector> detectors)
    {
        _detectors = detectors;
    }

    public IReadOnlyCollection<AnomalyResult> Analyze(
        IReadOnlyCollection<EmissionRecord> records)
    {
        var results = new List<AnomalyResult>();

        var orderedRecords = records
            .OrderBy(x => x.Site)
            .ThenBy(x => x.Month)
            .ToList();

        foreach (var current in orderedRecords)
        {
            var history = orderedRecords
                .Where(x =>
                    x.Site == current.Site &&
                    string.Compare(
                        x.Month,
                        current.Month,
                        StringComparison.Ordinal) < 0)
                .ToList();

            var findings = _detectors
                .SelectMany(detector =>
                    detector.Detect(current, history))
                .ToList();

            results.Add(new AnomalyResult
            {
                Id = current.Id,
                Findings = findings
            });
        }

        return results
        .OrderBy(x => x.Id)
        .ToList();
    }
}