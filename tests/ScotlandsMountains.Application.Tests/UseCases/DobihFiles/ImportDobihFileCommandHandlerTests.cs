using NSubstitute;
using ScotlandsMountains.Application.Ports;
using ScotlandsMountains.Application.UseCases.DobihFiles;
using ScotlandsMountains.Application.UseCases.DobihFiles.Parsing;
using System.Text;
using ScotlandsMountains.Domain.Entities;

namespace ScotlandsMountains.Application.Tests.UseCases.DobihFiles;

[TestFixture]
public class ImportDobihFileCommandHandlerTests
{
    private IDobihImportService _dobihImportService;
    private IFileStorageService _fileStorageService;
    private IDobihFileReader _reader;
    private IDobihRecordsParserFactory _parserFactory;
    private IDobihRecordsParser _parser;
    private ImportDobihFileCommandHandler _sut;

    private const string Container = "uploads";
    private const string FileName = "data.csv";
    private const string DobihFileName = "dobih_v1.txt";
    private static readonly Stream MockStream = new MemoryStream(Encoding.UTF8.GetBytes("File content"));
    private static readonly DobihRecords MockRecords = new DobihRecords(DobihFileName, new List<DobihRecord>());
    private static readonly DobihRecordsParser.Output MockOutput = new DobihRecordsParser.Output([], [], [], [], [], []);

    [SetUp]
    public void SetUp()
    {
        _dobihImportService = Substitute.For<IDobihImportService>();
        _fileStorageService = Substitute.For<IFileStorageService>();
        _reader = Substitute.For<IDobihFileReader>();
        _parserFactory = Substitute.For<IDobihRecordsParserFactory>();
        _parser = Substitute.For<IDobihRecordsParser>();

        _parserFactory.Build().Returns(_parser);

        _fileStorageService
            .DownloadFileAsync(Container, FileName, Arg.Any<CancellationToken>())
            .Returns(MockStream);

        _reader.Read(MockStream).Returns(MockRecords);

        _parser
            .Parse(MockRecords)
            .Returns(MockOutput);

        _sut = new ImportDobihFileCommandHandler(
            _dobihImportService,
            _fileStorageService,
            _reader,
            _parserFactory);
    }

    [Test]
    public async Task HandleAsync_SuccessfulImport_OrchestratesDependenciesAndReturnsSuccess()
    {
        // arrange
        var command = new ImportDobihFileCommand(Container, FileName);
        var token = new CancellationToken();

        // act
        var result = await _sut.HandleAsync(command, token);

        // assert
        using (Assert.EnterMultipleScope())
        {
            Assert.That(result.IsSuccess, Is.True, "Expected successful result.");
            Assert.That(result.Value, Is.True, "Expected successful boolean value.");

            await _dobihImportService.Received(1).StartProcessingAsync(
                Container, FileName, Arg.Is<CancellationToken>(t => t == token));

            await _fileStorageService.Received(1).DownloadFileAsync(
                Container, FileName, Arg.Is<CancellationToken>(t => t == token));

            _reader.Received(1).Read(MockStream);

            _parserFactory.Received(1).Build();
            _parser.Received(1).Parse(MockRecords);

            await _dobihImportService.Received(1).CompleteProcessingAsync(
                Container,
                FileName,
                DobihFileName,
                Arg.Is<List<Region>>(x => ReferenceEquals(x, MockOutput.Regions)),
                Arg.Is<List<Map>>(x => ReferenceEquals(x, MockOutput.Maps)),
                Arg.Is<List<Classification>>(x => ReferenceEquals(x, MockOutput.Classifications)),
                Arg.Is<List<County>>(x => ReferenceEquals(x, MockOutput.Counties)),
                Arg.Is<List<Country>>(x => ReferenceEquals(x, MockOutput.Countries)),
                Arg.Is<List<Mountain>>(x => ReferenceEquals(x, MockOutput.Mountains)),
                Arg.Is<CancellationToken>(t => t == token));
        }
    }

    [Test]
    public void HandleAsync_DownloadFails_ExceptionPropagatesAndCompleteIsNotCalled()
    {
        // arrange
        var command = new ImportDobihFileCommand(Container, FileName);
        var expectedException = new IOException();

        // Setup download to throw an exception
        _fileStorageService
            .DownloadFileAsync(Arg.Any<string>(), Arg.Any<string>(), Arg.Any<CancellationToken>())
            .Returns(Task.FromException<Stream>(expectedException));

        // act & assert
        Assert.That(
            async () => await _sut.HandleAsync(command),
            Throws.InstanceOf<IOException>());

        _dobihImportService.Received(1).StartProcessingAsync(Arg.Any<string>(), Arg.Any<string>(), Arg.Any<CancellationToken>());

        _dobihImportService.DidNotReceive().CompleteProcessingAsync(
            Arg.Any<string>(), Arg.Any<string>(), Arg.Any<string>(),
            Arg.Any<List<Region>>(), Arg.Any<List<Map>>(), Arg.Any<List<Classification>>(),
            Arg.Any<List<County>>(), Arg.Any<List<Country>>(), Arg.Any<List<Mountain>>(),
            Arg.Any<CancellationToken>());
    }
}