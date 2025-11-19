namespace ScotlandsMountains.Domain.Values;

public readonly record struct DobihId
{
    public DobihId(int value)
    {
        if (value < 1)
        {
            throw new ArgumentOutOfRangeException(nameof(value), "DobihId must be greater than 0.");
        }
        Value = value;
    }

    public int Value { get; }
    
    public static implicit operator int(DobihId dobihId) => dobihId.Value;
    
    public static implicit operator DobihId(int value) => new(value);

    public override string ToString() => Value.ToString();
}
