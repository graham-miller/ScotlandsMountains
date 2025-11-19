namespace ScotlandsMountains.Domain.Values;

public record struct GridRef
{
    public GridRef(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
        {
            throw new ArgumentException("Grid reference cannot be null or empty.", nameof(value));
        }
        Value = value.ToUpperInvariant();
    }

    public string Value { get; }
    
    public static implicit operator string(GridRef gridRef) => gridRef.Value;
    
    public static implicit operator GridRef(string value) => new(value);
    
    public override string ToString() => Value;
}
