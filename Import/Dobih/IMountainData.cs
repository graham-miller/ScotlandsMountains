namespace ScotlandsMountains.Import.Dobih;

public interface IMountainData
{
    List<Mountain> Mountains { get; }
    List<Classification> Classifications { get; }
    List<Section> Sections { get; }
    List<County> Counties { get; }
    List<Map> Maps { get; }
}