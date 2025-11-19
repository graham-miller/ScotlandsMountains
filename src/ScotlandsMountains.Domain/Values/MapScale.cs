namespace ScotlandsMountains.Domain.Values;

public record struct MapScale
{
    public MapScale(decimal value)
    {
        if (value <= 0m || value >= 1m)
        {
            throw new ArgumentOutOfRangeException(nameof(value), "Scale must be between 0 and 1.");
        }
        Value = value;
    }

    public decimal Value { get; }
    
    public static implicit operator decimal(MapScale value) => value.Value;
    
    public static implicit operator MapScale(decimal value) => new(value);

    public override string ToString() => $"1:{Value:0.0}";
}
