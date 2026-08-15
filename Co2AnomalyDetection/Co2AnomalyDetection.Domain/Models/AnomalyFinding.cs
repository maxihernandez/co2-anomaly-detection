using Co2AnomalyDetection.Domain.Enums;

namespace Co2AnomalyDetection.Domain.Models;

public sealed class AnomalyFinding
{
    public required AnomalyType Type { get; init; }

    public required string Reason { get; init; }

    public required AnomalySeverity Severity { get; init; }
}