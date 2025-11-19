namespace ScotlandsMountains.Domain.Values;

public record struct Height
{
    public Height(decimal metres)
    {
        if (metres < 0)
        {
            throw new ArgumentOutOfRangeException(nameof(metres), "Height cannot be negative.");
        }
        Metres = metres;
    }

    public decimal Metres { get; }
    
    public static implicit operator decimal(Height height) => height.Metres;
    
    public static implicit operator Height(decimal metres) => new(metres);

    public override string ToString() => $"{Metres:0.0}m";
}
