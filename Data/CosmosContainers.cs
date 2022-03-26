namespace ScotlandsMountains.Data;

public interface ICosmosContainers
{
    Container GetMountainsContainer();
    Container GetMountainGroupsContainer();
}

public class CosmosContainers : CosmosFacade, ICosmosContainers
{
    public CosmosContainers(CosmosConfig config)
        : base(config)
    { }

    public Container GetMountainsContainer()
    {
        var database = GetClient().GetDatabase(Config.DatabaseId);
        var container = database.GetContainer(Config.MountainsContainerId);

        return container;
    }

    public Container GetMountainGroupsContainer()
    {
        var database = GetClient().GetDatabase(Config.DatabaseId);
        var container = database.GetContainer(Config.MountainGroupsContainerId);

        return container;
    }
}