using Co2AnomalyDetection.Application.Detectors;
using Co2AnomalyDetection.Domain.Entities;
using Co2AnomalyDetection.Domain.Enums;

namespace Co2AnomalyDetection.Tests.Detectors;

public class EmissionFactorDetectorTests
{
    [Fact]
    public void Detect_ShouldReturnHighSeverity_WhenEmissionFactorIsSignificantlyHigherThanHistory()
    {
        // Arrange
        var detector = new EmissionFactorDetector();

        var history = new List<EmissionRecord>
        {
            new() { Id = 5, Site = "Barcelona", Month = "2026-01", EnergyKwh = 8500, Co2Kg = 1950 },
            new() { Id = 6, Site = "Barcelona", Month = "2026-02", EnergyKwh = 8700, Co2Kg = 2000 }
        };

        var current = new EmissionRecord
        {
            Id = 8,
            Site = "Barcelona",
            Month = "2026-04",
            EnergyKwh = 8900,
            Co2Kg = 8500
        };

        // Act
        var result = detector.Detect(current, history);

        // Assert
        var finding = Assert.Single(result);

        Assert.Equal(AnomalyType.EmissionFactor, finding.Type);
        Assert.Equal(AnomalySeverity.High, finding.Severity);
    }
}