namespace ScotlandsMountains.Import.Files;

internal static class FileStreams
{
    public static Stream HillCsvZip => GetFileStream("hillcsv.zip");

    private static Stream GetFileStream(string fileName)
    {
        return typeof(FileStreams).Assembly.GetManifestResourceStream($"{typeof(FileStreams).Namespace}.{fileName}")!;
    }
}