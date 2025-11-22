using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NSubstitute;
using ScotlandsMountains.Api.Controllers;
using ScotlandsMountains.Application.Adapters;
using ScotlandsMountains.Application.Ports;
using ScotlandsMountains.Application.UseCases.DobihFiles;
using System.Net;

namespace ScotlandsMountains.Api.Tests.Controllers;

[TestFixture]
public class DobihFilesControllerTests
{
    [TestFixture]
    public class GetTests
    {
        protected IMediator _mediator;
        protected DobihFilesController _sut;

        [SetUp]
        public void SetUp()
        {
            _mediator = Substitute.For<IMediator>();
            _sut = new DobihFilesController(_mediator);
        }

        [Test]
        public async Task ValidId_ReturnsOkWithDobihFileModel()
        {
            // arrange
            const int fileId = 1;
            var dto = new DobihFileDto(fileId, "name", "status", DateTime.UtcNow, DateTime.UtcNow.AddSeconds(1), DateTime.UtcNow.AddSeconds(2));
            var successResult = Result<DobihFileDto?>.Success(dto);

            _mediator
                .SendAsync(Arg.Any<GetDobihFileInfoQuery>(), Arg.Any<CancellationToken>())
                .Returns(successResult);

            // act
            var actual = await _sut.Get(fileId) as OkObjectResult;

            // assert
            using (Assert.EnterMultipleScope())
            {
                Assert.That(actual!.StatusCode, Is.EqualTo((int)HttpStatusCode.OK));
                Assert.That(actual.Value, Is.InstanceOf<DobihFileModel>());
                Assert.That((actual.Value as DobihFileModel)!.Id, Is.EqualTo(fileId));
            }
        }

        [Test]
        public async Task MediatorReturnsFailure_ReturnsBadRequest()
        {
            // arrange
            _mediator
                .SendAsync(Arg.Any<GetDobihFileInfoQuery>(), Arg.Any<CancellationToken>())
                .Returns(Result<DobihFileDto?>.Failure("Database error"));

            // act
            var actual = await _sut.Get(1) as BadRequestResult;

            // assert
            Assert.That(actual!.StatusCode, Is.EqualTo((int)HttpStatusCode.BadRequest));
        }

        [Test]
        public async Task MediatorReturnsSuccessWithNullValue_ReturnsNotFound()
        {
            // arrange
            _mediator
                .SendAsync(Arg.Any<GetDobihFileInfoQuery>(), Arg.Any<CancellationToken>())
                .Returns(Result<DobihFileDto?>.Success(null));

            // act
            var actual = await _sut.Get(1) as NotFoundResult;

            // assert
            Assert.That(actual!.StatusCode, Is.EqualTo((int)HttpStatusCode.NotFound));
        }
    }

    [TestFixture]
    public class UploadFileTests
    {
        protected IMediator _mediator;
        protected DobihFilesController _sut;
        [SetUp]
        public void SetUp()
        {
            _mediator = Substitute.For<IMediator>();
            _sut = new DobihFilesController(_mediator);
        }

        [Test]
        public async Task NullFile_ReturnsBadRequest()
        {
            // arrange & act
            var actual = await _sut.Upload(null) as BadRequestResult;

            // assert
            using (Assert.EnterMultipleScope())
            {
                Assert.That(actual!.StatusCode, Is.EqualTo((int)HttpStatusCode.BadRequest));
                await _mediator.DidNotReceive().SendAsync(Arg.Any<UploadDobihFileCommand>(), Arg.Any<CancellationToken>());
            }
        }

        [Test]
        public async Task EmptyFile_ReturnsBadRequest()
        {
            // arrange
            var mockFile = Substitute.For<IFormFile>();
            mockFile.Length.Returns(0);

            // act
            var actual = await _sut.Upload(mockFile) as BadRequestResult;

            // assert
            using (Assert.EnterMultipleScope())
            {
                Assert.That(actual!.StatusCode, Is.EqualTo((int)HttpStatusCode.BadRequest));
                await _mediator.DidNotReceive().SendAsync(Arg.Any<UploadDobihFileCommand>(), Arg.Any<CancellationToken>());
            }
        }

        [Test]
        public async Task ValidFile_ReturnsAcceptedAtRoute()
        {
            // arrange
            const int newFileId = 1;
            var dto = new DobihFileDto(newFileId, "name", "status", DateTime.UtcNow, DateTime.UtcNow.AddSeconds(1), DateTime.UtcNow.AddSeconds(2));
            var successResult = Result<DobihFileDto>.Success(dto);

            var mockFile = Substitute.For<IFormFile>();
            mockFile.Length.Returns(100);
            mockFile.OpenReadStream().Returns(new MemoryStream([1, 2, 3]));

            _mediator
                .SendAsync(Arg.Any<UploadDobihFileCommand>(), Arg.Any<CancellationToken>())
                .Returns(successResult);

            // act
            var actual = await _sut.Upload(mockFile) as AcceptedAtRouteResult;

            // assert
            using (Assert.EnterMultipleScope())
            {
                Assert.That(actual!.StatusCode, Is.EqualTo((int)HttpStatusCode.Accepted));
                Assert.That(actual.RouteName, Is.EqualTo(nameof(DobihFilesController.Get)));
                Assert.That(actual.RouteValues, Is.Not.Null);
                Assert.That(actual.RouteValues!.ContainsKey("id"), Is.True);
                Assert.That(actual.RouteValues["id"], Is.EqualTo(newFileId));
                Assert.That(actual.Value, Is.InstanceOf<DobihFileModel>());
                
                var actualModel = actual.Value as DobihFileModel;
                Assert.That(actualModel!.Id, Is.EqualTo(dto.Id));
                Assert.That(actualModel.Name, Is.EqualTo(dto.Name));
                Assert.That(actualModel.Status, Is.EqualTo(dto.Status));
                Assert.That(actualModel.UploadedAt, Is.EqualTo(dto.UploadedAt));
                Assert.That(actualModel.StartedProcessingAt, Is.EqualTo(dto.StartedProcessingAt));
                Assert.That(actualModel.CompletedProcessingAt, Is.EqualTo(dto.CompletedProcessingAt));

                mockFile.Received(1).OpenReadStream();
            }
        }

        [Test]
        public async Task MediatorReturnsFailure_ReturnsBadRequestWithMessage()
        {
            // arrange
            const string errorMessage = "Error message";
            var failureResult = Result<DobihFileDto>.Failure(errorMessage);

            var mockFile = Substitute.For<IFormFile>();
            mockFile.Length.Returns(100);
            mockFile.OpenReadStream().Returns(new MemoryStream([1, 2, 3]));

            _mediator
                .SendAsync(Arg.Any<UploadDobihFileCommand>(), Arg.Any<CancellationToken>())
                .Returns(failureResult);

            // act
            var actual = await _sut.Upload(mockFile) as BadRequestObjectResult;

            // assert
            using (Assert.EnterMultipleScope())
            {
                Assert.That(actual!.StatusCode, Is.EqualTo((int)HttpStatusCode.BadRequest));
                Assert.That(actual.Value, Is.EqualTo(errorMessage));
            }
        }
    }
}