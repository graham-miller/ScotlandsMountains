using NSubstitute;
using ScotlandsMountains.Application.UseCases.DobihFiles;
using ScotlandsMountains.Application.Ports;
using ScotlandsMountains.Domain.Entities;
using ScotlandsMountains.Shared;

namespace ScotlandsMountains.Application.Tests.UseCases.DobihFiles;

[TestFixture]
public class GetDobihFileInfoQueryHandlerTests
{
    private IDobihImportService _service;
    private GetDobihFileInfoQueryHandler _sut; // System Under Test

    [SetUp]
    public void SetUp()
    {
        // arrange
        _service = Substitute.For<IDobihImportService>();
        _sut = new GetDobihFileInfoQueryHandler(_service);
    }

    [Test]
    public async Task HandleAsync_FileFound_ReturnsSuccessResultWithDto()
    {
        // arrange
        const int queryId = 101;
        const string containerName = "Container name";
        const string fileName = "File name";
        var query = new GetDobihFileInfoQuery(queryId);
        var dobihFile = new DobihFile(containerName, fileName);

        _service
            .GetDobihFileAsync(queryId, Arg.Any<CancellationToken>())
            .Returns(dobihFile);

        // act
        var result = await _sut.HandleAsync(query);

        // assert
        using (Assert.EnterMultipleScope())
        {
            Assert.That(result.IsSuccess, Is.True);
            Assert.That(result.Value, Is.Not.Null);
        }
    }

    [Test]
    public async Task HandleAsync_FileDoesNotExist_ReturnsNotFoundFailure()
    {
        // arrange
        var query = new GetDobihFileInfoQuery(1);

        // Mock the service to return null
        _service.GetDobihFileAsync(Arg.Any<int>(), Arg.Any<CancellationToken>())
            .Returns((DobihFile) null);

        // act
        var result = await _sut.HandleAsync(query);

        // assert
        using (Assert.EnterMultipleScope())
        {
            Assert.That(result.IsFailure, Is.True);
            Assert.That(result.Error.Type, Is.EqualTo(Errors.NotFound));
            Assert.That(result.Value, Is.Null);
        }
    }

    [Test]
    public async Task HandleAsync_ServiceCalledWithCorrectId_ServiceReceivedCall()
    {
        // arrange
        const int queryId = 303;
        var query = new GetDobihFileInfoQuery(queryId);

        // Mock the service to return a file (outcome is less important than the call verification)
        var dobihFile = new DobihFile("Container name", "File name");

        _service
            .GetDobihFileAsync(queryId, Arg.Any<CancellationToken>())
            .Returns(dobihFile);

        // act
        await _sut.HandleAsync(query);

        // assert
        await _service.Received(1).GetDobihFileAsync(
            Arg.Is<int>(id => id == queryId),
            Arg.Any<CancellationToken>());
    }
}