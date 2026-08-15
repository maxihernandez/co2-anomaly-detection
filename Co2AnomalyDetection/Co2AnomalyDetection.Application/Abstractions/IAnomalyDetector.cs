using Co2AnomalyDetection.Domain.Entities;
using Co2AnomalyDetection.Domain.Models;

namespace Co2AnomalyDetection.Application.Abstractions;

public interface IAnomalyDetector
{
    IReadOnlyCollection<AnomalyFinding> Detect(
        EmissionRecord current,
        IReadOnlyCollection<EmissionRecord> history);
}