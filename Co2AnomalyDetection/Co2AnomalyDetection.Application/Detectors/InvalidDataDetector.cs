using Co2AnomalyDetection.Application.Abstractions;
using Co2AnomalyDetection.Domain.Entities;
using Co2AnomalyDetection.Domain.Enums;
using Co2AnomalyDetection.Domain.Models;

namespace Co2AnomalyDetection.Application.Detectors;

public sealed class InvalidDataDetector : IAnomalyDetector
{
    public IReadOnlyCollection<AnomalyFinding> Detect(
        EmissionRecord current,
        IReadOnlyCollection<EmissionRecord> history)
    {
        var findings = new List<AnomalyFinding>();

        if (current.EnergyKwh <= 0)
        {
            findings.Add(new AnomalyFinding
            {
                Type = AnomalyType.InvalidData,
                Reason = "Energy consumption must be greater than zero.",
                Severity = AnomalySeverity.High
            });
        }

        if (current.Co2Kg <= 0)
        {
            findings.Add(new AnomalyFinding
            {
                Type = AnomalyType.InvalidData,
                Reason = "CO2 emissions must be greater than zero.",
                Severity = AnomalySeverity.High
            });
        }

        return findings;
    }
}