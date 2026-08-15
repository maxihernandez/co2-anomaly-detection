using Co2AnomalyDetection.Application.Abstractions;
using Co2AnomalyDetection.Domain.Entities;
using Co2AnomalyDetection.Domain.Enums;
using Co2AnomalyDetection.Domain.Models;

namespace Co2AnomalyDetection.Application.Detectors;

public sealed class EmissionFactorDetector : IAnomalyDetector
{
    private const decimal MediumThreshold = 0.50m;
    private const decimal HighThreshold = 1.00m;

    public IReadOnlyCollection<AnomalyFinding> Detect(
        EmissionRecord current,
        IReadOnlyCollection<EmissionRecord> history)
    {
        if (current.EnergyKwh <= 0 || current.Co2Kg <= 0)
        {
            return Array.Empty<AnomalyFinding>();
        }

        var validHistory = history
            .Where(x => x.EnergyKwh > 0 && x.Co2Kg > 0)
            .ToList();

        if (validHistory.Count < 2)
        {
            return Array.Empty<AnomalyFinding>();
        }

        var historicalEmissionFactors = validHistory
            .Select(x => x.Co2Kg / x.EnergyKwh)
            .ToList();

        var historicalAverageFactor = historicalEmissionFactors.Average();

        if (historicalAverageFactor <= 0)
        {
            return Array.Empty<AnomalyFinding>();
        }

        var currentEmissionFactor = current.Co2Kg / current.EnergyKwh;

        var variation =
            Math.Abs(currentEmissionFactor - historicalAverageFactor)
            / historicalAverageFactor;

        if (variation >= HighThreshold)
        {
            return new[]
            {
                new AnomalyFinding
                {
                    Type = AnomalyType.EmissionFactor,
                    Reason =
                        $"CO2-to-energy ratio differs by {variation:P0} from the historical average.",
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
                    Type = AnomalyType.EmissionFactor,
                    Reason =
                        $"CO2-to-energy ratio differs by {variation:P0} from the historical average.",
                    Severity = AnomalySeverity.Medium
                }
            };
        }

        return Array.Empty<AnomalyFinding>();
    }
}