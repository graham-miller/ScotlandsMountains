using NSubstitute;
using ScotlandsMountains.Application.UseCases.DobihFiles;
using ScotlandsMountains.Application.Ports;
using ScotlandsMountains.Application.Adapters;
using ScotlandsMountains.Domain.Entities;
using ScotlandsMountains.Shared;

namespace ScotlandsMountains.Application.Tests.UseCases.DobihFiles;

[TestFixture]
public class UploadDobihFileCommandHandlerTests
{
    private IDobihImportService _dobihImportService;
    private IFileStorageService _fileStorageService;
    private IFileUploadNotificationService _fileUploadNotificationService;
    private UploadDobihFileCommandHandler _sut;

    private static readonly Stream MockContentStream = new MemoryStream([1, 2, 3]);
    private static readonly DobihFile MockDomainFile = new DobihFile("Container name", "File name");

    private static readonly Uri MockFileUri = new Uri("https://storage.example.com/dobih-files/test-file-guid");

    [SetUp]
    public void SetUp()
    {
        _dobihImportService = Substitute.For<IDobihImportService>();
        _fileStorageService = Substitute.For<IFileStorageService>();
        _fileUploadNotificationService = Substitute.For<IFileUploadNotificationService>();

        _dobihImportService
            .AcceptUploadAsync(Arg.Any<string>(), Arg.Any<string>(), Arg.Any<CancellationToken>())
            .Returns(MockDomainFile);

        _fileStorageService
            .UploadFileAsync(Arg.Any<string>(), Arg.Any<string>(), Arg.Any<Stream>(), Arg.Any<CancellationToken>())
            .Returns(MockFileUri);

        _sut = new UploadDobihFileCommandHandler(
            _dobihImportService,
            _fileStorageService,
            _fileUploadNotificationService);
    }

    [Test]
    public async Task HandleAsync_SuccessfulUpload_OrchestratesServicesAndReturnsSuccess()
    {
        // arrange
        var command = new UploadDobihFileCommand(MockContentStream);

        // act
        var result = await _sut.HandleAsync(command);

        // assert
        using (Assert.EnterMultipleScope())
        {
            Assert.That(result.IsSuccess, Is.True);
            Assert.That(result.Value, Is.InstanceOf<DobihFileDto>());

            const string expectedContainerName = "dobih-files";
            await _dobihImportService.Received(1).AcceptUploadAsync(
                Arg.Is(expectedContainerName), Arg.Any<string>(), Arg.Any<CancellationToken>());

            await _fileStorageService.Received(1).UploadFileAsync(
                Arg.Is(expectedContainerName), Arg.Any<string>(), Arg.Is<Stream>(s => s == MockContentStream), Arg.Any<CancellationToken>());

            await _fileUploadNotificationService.Received(1).PublishFileUploadedNotificationAsync(
                Arg.Is(expectedContainerName), Arg.Any<string>(), Arg.Any<CancellationToken>());
        }
    }

    [Test]
    public async Task HandleAsync_AcceptUploadFails_ReturnsBadRequest()
    {
        // arrange
        var command = new UploadDobihFileCommand(MockContentStream);

        _dobihImportService
            .AcceptUploadAsync(Arg.Any<string>(), Arg.Any<string>(), Arg.Any<CancellationToken>())
            .Returns(Task.FromException<DobihFile>(new InvalidOperationException()));

        // act
        var result = await _sut.HandleAsync(command);

        // assert
        using (Assert.EnterMultipleScope())
        {
            Assert.That(result.IsFailure, Is.True);
            Assert.That(result.Error.Type, Is.EqualTo(Errors.BadRequest));

            await _fileStorageService.DidNotReceive().UploadFileAsync(
                Arg.Any<string>(), Arg.Any<string>(), Arg.Any<Stream>(), Arg.Any<CancellationToken>());
            
            await _fileUploadNotificationService.DidNotReceive().PublishFileUploadedNotificationAsync(
                Arg.Any<string>(), Arg.Any<string>(), Arg.Any<CancellationToken>());
        }
    }

    [Test]
    public async Task HandleAsync_UploadFileFails_ReturnsUnknownError()
    {
        // arrange
        var command = new UploadDobihFileCommand(MockContentStream);

        _fileStorageService
            .UploadFileAsync(Arg.Any<string>(), Arg.Any<string>(), Arg.Any<Stream>(), Arg.Any<CancellationToken>())
            .Returns(Task.FromException<Uri>(new TimeoutException()));

        // act
        var result = await _sut.HandleAsync(command);

        // assert
        using (Assert.EnterMultipleScope())
        {
            Assert.That(result.IsFailure, Is.True);
            Assert.That(result.Error.Type, Is.EqualTo(Errors.Unknown));

            // Verify services called: AcceptUpload must be called, but notification should not be.
            await _dobihImportService.Received(1).AcceptUploadAsync(
                Arg.Any<string>(), Arg.Any<string>(), Arg.Any<CancellationToken>());

            await _fileUploadNotificationService.DidNotReceive().PublishFileUploadedNotificationAsync(
                Arg.Any<string>(), Arg.Any<string>(), Arg.Any<CancellationToken>());
        }
    }

    [Test]
    public async Task HandleAsync_NotificationPublishingFails_ReturnsUnknownError()
    {
        // arrange
        var command = new UploadDobihFileCommand(MockContentStream);

        // Setup Publish notification to throw an exception
        _fileUploadNotificationService
            .PublishFileUploadedNotificationAsync(Arg.Any<string>(), Arg.Any<string>(), Arg.Any<CancellationToken>())
            .Returns(Task.FromException(new Exception()));

        // act
        var result = await _sut.HandleAsync(command);

        // assert
        using (Assert.EnterMultipleScope())
        {
            Assert.That(result.IsFailure, Is.True);
            Assert.That(result.Error.Type, Is.EqualTo(Errors.Unknown));

            await _dobihImportService.Received(1).AcceptUploadAsync(
                Arg.Any<string>(), Arg.Any<string>(), Arg.Any<CancellationToken>());
            await _fileStorageService.Received(1).UploadFileAsync(
                Arg.Any<string>(), Arg.Any<string>(), Arg.Any<Stream>(), Arg.Any<CancellationToken>());
        }
    }
}