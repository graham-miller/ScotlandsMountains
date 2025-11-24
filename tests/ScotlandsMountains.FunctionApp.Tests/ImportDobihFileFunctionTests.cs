using Azure.Messaging.ServiceBus;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;
using NSubstitute;
using NSubstitute.ExceptionExtensions;
using ScotlandsMountains.Application.Ports;
using ScotlandsMountains.Application.UseCases.DobihFiles;
using ScotlandsMountains.Shared;
using System.Text;
using System.Text.Json;

namespace ScotlandsMountains.FunctionApp.Tests;

[TestFixture]
public class ImportDobihFileFunctionTests
{
    private IMediator _mediator;
    private ServiceBusReceivedMessage? _message;
    private ServiceBusMessageActions _messageActions;
    private ImportDobihFileFunction _sut;

    private static readonly FileUploadNotificationMessage TestPayload = new FileUploadNotificationMessage
    {
        ContainerName = "Container name",
        FileName = "File name"
    };

    [SetUp]
    public void SetUp()
    {
        _mediator = Substitute.For<IMediator>();
        _messageActions = Substitute.For<ServiceBusMessageActions>();

        var json = JsonSerializer.Serialize(TestPayload);
        _message = ServiceBusModelFactory.ServiceBusReceivedMessage(
            body: new BinaryData(Encoding.UTF8.GetBytes(json)),
            messageId: "123");

        var loggerFactory = Substitute.For<ILoggerFactory>();
        var logger = Substitute.For<ILogger<ImportDobihFileFunction>>();
        loggerFactory.CreateLogger<ImportDobihFileFunction>().Returns(logger);

        _sut = new ImportDobihFileFunction(_mediator, loggerFactory);
    }

    [Test]
    public async Task Run_ValidMessage_ProcessesCommandAndCompletesMessage()
    {
        // arrange
        _mediator
            .SendAsync(Arg.Any<ImportDobihFileCommand>(), Arg.Any<CancellationToken>())
            .Returns(Task.FromResult(Result.Success(true)));

        // act
        await _sut.Run(_message!, _messageActions);

        // assert
        using (Assert.EnterMultipleScope())
        {
            await _mediator.Received(1).SendAsync(
                Arg.Is<ImportDobihFileCommand>(c =>
                    c.ContainerName == TestPayload.ContainerName &&
                    c.FileName == TestPayload.FileName),
                Arg.Any<CancellationToken>());


            await _messageActions.Received(1).CompleteMessageAsync(_message, Arg.Any<CancellationToken>());
        }
    }

    [Test]
    public async Task Run_MediatorThrowsException_MessageIsNotCompleted()
    {
        // arrange
        _mediator
            .SendAsync(Arg.Any<ImportDobihFileCommand>(), Arg.Any<CancellationToken>())
            .Throws(new ApplicationException());

        // act & assert
        using (Assert.EnterMultipleScope())
        {
            Assert.That(async () => await _sut.Run(_message!, _messageActions),
            Throws.InstanceOf<ApplicationException>());

            await _messageActions.DidNotReceive().CompleteMessageAsync(
                Arg.Any<ServiceBusReceivedMessage>(),
                Arg.Any<CancellationToken>());
        }
    }
}