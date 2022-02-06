using ScotlandsMountains.Domain.Values;

namespace ScotlandsMountains.Domain;

public class Classification : Entity
{
    public string SingularName { get; set; }

    public int DisplayOrder { get; set; }

    public string Description { get; set; }

    public List<MountainSummary> Mountains { get; set; }
}