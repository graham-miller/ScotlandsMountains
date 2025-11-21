using ScotlandsMountains.Domain.Entities;

namespace ScotlandsMountains.Application.Adapters;

public class DobihFileDto
{
    public DobihFileDto(DobihFile file)
    {
        Id = file.Id;
        Name = file.DobihName;
        Status = file.Status.ToString();
        UploadedAt = file.UploadedAt;
        StartedProcessingAt = file.StartedProcessingAt;
        CompletedProcessingAt = file.CompletedProcessingAt;
    }

    public int Id { get; private set; }
    
    public string? Name { get; private set; }

    public string Status { get; private set; }

    public DateTime UploadedAt { get; private set; }

    public DateTime? StartedProcessingAt { get; private set; }

    public DateTime? CompletedProcessingAt { get; private set; }
}
