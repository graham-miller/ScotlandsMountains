using ScotlandsMountains.Application.Adapters;
using ScotlandsMountains.Domain.Entities;

namespace ScotlandsMountains.Application.Tests.Adapters;

[TestFixture]
public class DobihFileDtoTests
{
    [Test]
    public void Constructor_FullyProcessedFile_PropertiesAreCorrectlyMapped()
    {
        // arrange
        const string expectedName = "DoBIH file name";

        var dobihFile = new DobihFile("Container name", "File name");
        dobihFile.StartProcessing();
        dobihFile.CompleteProcessing(expectedName);

        // act
        var dto = new DobihFileDto(dobihFile);

        // assert
        using (Assert.EnterMultipleScope())
        {
            Assert.That(dto.Name, Is.EqualTo(expectedName));
            Assert.That(dto.Status, Is.EqualTo(DobihFileStatus.Completed.ToString()));
            Assert.That(dto.UploadedAt, Is.EqualTo(DateTime.Now).Within(5).Seconds);
            Assert.That(dto.StartedProcessingAt, Is.EqualTo(DateTime.Now).Within(5).Seconds);
            Assert.That(dto.CompletedProcessingAt, Is.EqualTo(DateTime.Now).Within(5).Seconds);

            Assert.That(dto.UploadedAt, Is.LessThan(dto.StartedProcessingAt!));
            Assert.That(dto.StartedProcessingAt, Is.LessThan(dto.CompletedProcessingAt!));
        }
    }
}
