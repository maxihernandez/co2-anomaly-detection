using Co2AnomalyDetection.Application.Detectors;
using Co2AnomalyDetection.Domain.Entities;
using Co2AnomalyDetection.Domain.Enums;

namespace Co2AnomalyDetection.Tests.Detectors;

public class EnergyConsumptionDetectorTests
{
    [Fact]
    public void Detect_ShouldReturnHighSeverity_WhenConsumptionIsSignificantlyHigherThanHistory()
    {
        // Arrange
        var detector = new EnergyConsumptionDetector();

        var history = new List<EmissionRecord>
        {
            new() { Id = 1, Site = "Madrid", Month = "2026-01", EnergyKwh = 12000, Co2Kg = 2800 },
            new() { Id = 2, Site = "Madrid", Month = "2026-02", EnergyKwh = 12500, Co2Kg = 2900 },
            new() { Id = 3, Site = "Madrid", Month = "2026-03", EnergyKwh = 12800, Co2Kg = 2950 }
        };

        var current = new EmissionRecord
        {
            Id = 4,
            Site = "Madrid",
            Month = "2026-04",
            EnergyKwh = 79000,
            Co2Kg = 18200
        };

        // Act
        var result = detector.Detect(current, history);

        // Assert
        var finding = Assert.Single(result);

        Assert.Equal(AnomalyType.EnergyConsumption, finding.Type);
        Assert.Equal(AnomalySeverity.High, finding.Severity);
    }

    [Fact]
    public void Detect_ShouldReturnNoFindings_WhenConsumptionIsWithinExpectedRange()
    {
        // Arrange
        var detector = new EnergyConsumptionDetector();

        var history = new List<EmissionRecord>
    {
        new() { Id = 1, Site = "Madrid", Month = "2026-01", EnergyKwh = 12000, Co2Kg = 2800 },
        new() { Id = 2, Site = "Madrid", Month = "2026-02", EnergyKwh = 12500, Co2Kg = 2900 }
    };

        var current = new EmissionRecord
        {
            Id = 3,
            Site = "Madrid",
            Month = "2026-03",
            EnergyKwh = 12800,
            Co2Kg = 2950
        };

        // Act
        var result = detector.Detect(current, history);

        // Assert
        Assert.Empty(result);
    }

    [Fact]
    public void Detect_ShouldReturnNoFindings_WhenThereIsNotEnoughHistory()
    {
        // Arrange
        var detector = new EnergyConsumptionDetector();

        var history = new List<EmissionRecord>
    {
        new() { Id = 1, Site = "Madrid", Month = "2026-01", EnergyKwh = 12000, Co2Kg = 2800 }
    };

        var current = new EmissionRecord
        {
            Id = 2,
            Site = "Madrid",
            Month = "2026-02",
            EnergyKwh = 30000,
            Co2Kg = 7000
        };

        // Act
        var result = detector.Detect(current, history);

        // Assert
        Assert.Empty(result);
    }
}