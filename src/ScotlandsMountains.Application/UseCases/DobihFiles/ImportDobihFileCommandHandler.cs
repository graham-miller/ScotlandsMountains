using ScotlandsMountains.Application.Adapters;
using ScotlandsMountains.Application.Ports;
using ScotlandsMountains.Application.UseCases.DobihFiles.Parsing;

namespace ScotlandsMountains.Application.UseCases.DobihFiles;

public record ImportDobihFileCommand(string ContainerName, string FileName) : IRequest<Result<bool>>;

internal class ImportDobihFileCommandHandler : IRequestHandler<ImportDobihFileCommand, Result<bool>>
{
    private readonly IDobihImportService _dobihImportService;
    private readonly IFileStorageService _fileStorageService;
    private readonly IDobihFileReader _reader;
    private readonly IDobihRecordsParserFactory _parserFactory;

    public ImportDobihFileCommandHandler(
        IDobihImportService dobihImportService,
        IFileStorageService fileStorageService,
        IDobihFileReader reader,
        IDobihRecordsParserFactory parserFactory)
    {
        _dobihImportService = dobihImportService;
        _fileStorageService = fileStorageService;
        _reader = reader;
        _parserFactory = parserFactory;
    }

    public async Task<Result<bool>> HandleAsync(ImportDobihFileCommand request, CancellationToken cancellationToken = default)
    {
        await _dobihImportService.StartProcessingAsync(request.ContainerName, request.FileName, cancellationToken);

        using var stream = await _fileStorageService.DownloadFileAsync(request.ContainerName, request.FileName, cancellationToken);
        var records = _reader.Read(stream);
        var output = _parserFactory.Build().Parse(records);

        await _dobihImportService.CompleteProcessingAsync(
            request.ContainerName, request.FileName, records.FileName,
            output.Regions, output.Maps, output.Classifications, output.Counties, output.Countries, output.Mountains,
            cancellationToken);

        return Result<bool>.Success(true);
    }
}
