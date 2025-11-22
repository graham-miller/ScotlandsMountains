using ScotlandsMountains.Application.Adapters;
using ScotlandsMountains.Application.Ports;

namespace ScotlandsMountains.Application.UseCases.DobihFiles;

public record GetDobihFileInfoQuery(int Id) : IRequest<Result<DobihFileDto?>>;

internal class GetDobihFileInfoQueryHandler : IRequestHandler<GetDobihFileInfoQuery, Result<DobihFileDto?>>
{
    private readonly IDobihImportService _service;

    public GetDobihFileInfoQueryHandler(IDobihImportService service)
    {
        _service = service;
    }

    public async Task<Result<DobihFileDto?>> HandleAsync(GetDobihFileInfoQuery request, CancellationToken cancellationToken = default)
    {
        var file = await _service.GetDobihFileAsync(request.Id, cancellationToken);

        return file is null
            ? Result<DobihFileDto?>.Success(null)
            : Result<DobihFileDto?>.Success(new DobihFileDto(file));
    }
}
