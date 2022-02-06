using ScotlandsMountains.Domain.Values;

namespace ScotlandsMountains.Domain;

public class County : Entity
{
    public List<MountainSummary> Mountains { get; set; }
}