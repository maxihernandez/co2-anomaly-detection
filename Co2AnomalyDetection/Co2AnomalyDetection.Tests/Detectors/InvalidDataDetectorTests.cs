using Co2AnomalyDetection.Application.Detectors;
using Co2AnomalyDetection.Domain.Entities;
using Co2AnomalyDetection.Domain.Enums;

namespace Co2AnomalyDetection.Tests.Detectors;

public class InvalidDataDetectorTests
{
    [Fact]
    public void Detect_ShouldReturnTwoFindings_WhenEnergyAndCo2AreNegative()
    {
        // Arrange
        var detector = new InvalidDataDetector();

        var current = new EmissionRecord
        {
            Id = 7,
            Site = "Barcelona",
            Month = "2026-03",
            EnergyKwh = -900,
            Co2Kg = -210
        };

        // Act
        var result = detector.Detect(
            current,
            Array.Empty<EmissionRecord>());

        // Assert
        Assert.Equal(2, result.Count);

        Assert.All(result, finding =>
        {
            Assert.Equal(AnomalyType.InvalidData, finding.Type);
            Assert.Equal(AnomalySeverity.High, finding.Severity);
        });
    }
}