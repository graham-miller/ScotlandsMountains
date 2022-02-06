using ScotlandsMountains.Domain.Values;

namespace ScotlandsMountains.Domain;

public class Section : Entity
{
    public string Code { get; set; }

    public List<MountainSummary> Mountains { get; set; }
}