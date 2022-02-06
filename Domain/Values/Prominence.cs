namespace ScotlandsMountains.Domain.Values;

public class Prominence : Height
{
    public string MeasuredFrom { get; set; }

    public Height MeasuredFromHeight { get; set; }
}