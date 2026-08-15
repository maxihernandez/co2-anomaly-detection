namespace Co2AnomalyDetection.Domain.Models;

public sealed class AnomalyResult
{
    public int Id { get; init; }

    public bool RequiresReview => Findings.Count > 0;

    public IReadOnlyCollection<AnomalyFinding> Findings { get; init; }
        = Array.Empty<AnomalyFinding>();
}