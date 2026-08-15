using Co2AnomalyDetection.Application.Abstractions;
using Co2AnomalyDetection.Domain.Entities;
using Co2AnomalyDetection.Domain.Enums;
using Co2AnomalyDetection.Domain.Models;

namespace Co2AnomalyDetection.Application.Detectors;

public sealed class EnergyConsumptionDetector : IAnomalyDetector
{
    private const decimal MediumThreshold = 0.50m;
    private const decimal HighThreshold = 1.00m;

    public IReadOnlyCollection<AnomalyFinding> Detect(
     EmissionRecord current,
     IReadOnlyCollection<EmissionRecord> history)
    {
        if (current.EnergyKwh <= 0)
        {
            return Array.Empty<AnomalyFinding>();
        }

        var validHistory = history
            .Where(x => x.EnergyKwh > 0)
            .ToList();

        if (validHistory.Count < 2)
        {
            return Array.Empty<AnomalyFinding>();
        }

        var historicalAverage = validHistory.Average(x => x.EnergyKwh);

        if (historicalAverage <= 0)
        {
            return Array.Empty<AnomalyFinding>();
        }

        var variation =
            Math.Abs(current.EnergyKwh - historicalAverage)
            / historicalAverage;

        if (variation >= HighThreshold)
        {
            return new[]
            {
            new AnomalyFinding
            {
                Type = AnomalyType.EnergyConsumption,
                Reason =
                    $"Energy consumption differs by {variation:P0} from the historical average.",
                Severity = AnomalySeverity.High
            }
        };
        }

        if (variation >= MediumThreshold)
        {
            return new[]
            {
            new AnomalyFinding
            {
                Type = AnomalyType.EnergyConsumption,
                Reason =
                    $"Energy consumption differs by {variation:P0} from the historical average.",
                Severity = AnomalySeverity.Medium
            }
        };
        }

        return Array.Empty<AnomalyFinding>();
    }
}
