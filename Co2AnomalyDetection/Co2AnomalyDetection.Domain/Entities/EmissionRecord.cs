namespace Co2AnomalyDetection.Domain.Entities;

public sealed class EmissionRecord
{
    public int Id { get; init; }

    public required string Site { get; init; }

    public required string Month { get; init; }

    public decimal EnergyKwh { get; init; }

    public decimal Co2Kg { get; init; }
}