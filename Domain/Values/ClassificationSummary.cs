namespace ScotlandsMountains.Domain.Values;

public class ClassificationSummary : EntitySummary
{
    public ClassificationSummary() { }

    public ClassificationSummary(Classification classification)
        : base(classification)
    {
        DisplayOrder = classification.DisplayOrder;
    }

    public int DisplayOrder { get; set; }
}