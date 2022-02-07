namespace ScotlandsMountains.Domain;

public class Classification : MountainGroup
{
    public string SingularName { get; set; }

    public int DisplayOrder { get; set; }

    public string Description { get; set; }
}