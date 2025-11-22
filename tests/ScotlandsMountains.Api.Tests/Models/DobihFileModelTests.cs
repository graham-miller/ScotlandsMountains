using ScotlandsMountains.Application.Adapters;

namespace ScotlandsMountains.Api.Tests.Models;

[TestFixture]
public class DobihFileModelTests
{
    [Test]
    public void DobihFileModel_Constructor_SetsPropertiesCorrectly()
    {
        // arrange
        var dto = new DobihFileDto(1, "name", "status", DateTime.UtcNow, DateTime.UtcNow.AddSeconds(1), DateTime.UtcNow.AddSeconds(2));

        // act
        var sut = new DobihFileModel(dto);
        
        // assert
        using (Assert.EnterMultipleScope())
        {
            Assert.That(sut.Name, Is.EqualTo(dto.Name));
            Assert.That(sut.Status, Is.EqualTo(dto.Status));
            Assert.That(sut.UploadedAt, Is.EqualTo(dto.UploadedAt));
            Assert.That(sut.StartedProcessingAt, Is.EqualTo(dto.StartedProcessingAt));
            Assert.That(sut.CompletedProcessingAt, Is.EqualTo(dto.CompletedProcessingAt));
        }
    }
}
