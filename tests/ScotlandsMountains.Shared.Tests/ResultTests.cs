namespace ScotlandsMountains.Shared.Tests;

[TestFixture]
public class ResultTests
{
    [TestFixture]
    public class SuccessAndFailureStaticMethodTests
    {
        private const int TestValue = 100;
        private static readonly Error TestError = Errors.NotFound;

        [Test]
        public void Success_Bool_CreatesSuccessfulBoolResultWithValueTrue()
        {
            // arrange & act
            var result = Result.Success();

            // assert
            using (Assert.EnterMultipleScope())
            {
                Assert.That(result.IsSuccess, Is.True);
                Assert.That(result.IsFailure, Is.False);
                Assert.That(result.Value, Is.True);
            }
        }

        [Test]
        public void SuccessT_WithValue_CreatesSuccessfulResultWithCorrectValue()
        {
            // arrange & act
            var result = Result.Success(TestValue);

            // assert
            using (Assert.EnterMultipleScope())
            {
                Assert.That(result.IsSuccess, Is.True);
                Assert.That(result.IsFailure, Is.False);
                Assert.That(result.Value, Is.EqualTo(TestValue));
            }
        }

        [Test]
        public void SuccessT_WithDefaultValueWhenAllowed_CreatesSuccessfulResult()
        {
            // arrange
            const bool defaultValue = default;

            // act
            var result = Result.Success(defaultValue);

            // assert
            using (Assert.EnterMultipleScope())
            {
                Assert.That(result.IsSuccess, Is.True);
                Assert.That(result.Value, Is.False);
            }
        }

        [Test]
        public void Failure_Bool_CreatesFailedBoolResultWithError()
        {
            // arrange & act
            var result = Result.Failure(TestError);

            // assert
            using (Assert.EnterMultipleScope())
            {
                Assert.That(result.IsSuccess, Is.False);
                Assert.That(result.IsFailure, Is.True);
                Assert.That(result.Value, Is.False);
                Assert.That(result.Error, Is.EqualTo(TestError));
            }
        }

        [Test]
        public void FailureT_WithError_CreatesFailedResultWithDefaultValue()
        {
            // arrange & act
            var result = Result.Failure<int>(TestError);

            // assert
            using (Assert.EnterMultipleScope())
            {
                Assert.That(result.IsSuccess, Is.False);
                Assert.That(result.IsFailure, Is.True);
                Assert.That(result.Value, Is.EqualTo(default(int)));
                Assert.That(result.Error, Is.EqualTo(TestError));
            }
        }
    }

    [TestFixture]
    public class ResultTConstructorAndMethodsTests
    {
        private static readonly Error TestError = Errors.BadRequest;

        [Test]
        public void Constructor_SuccessWithNonBoolNonVoidDefaultValue_ThrowsArgumentException()
        {
            // arrange
            int defaultValue = default;

            // act & assert
            Assert.That(() => new Result<int>(defaultValue, Errors.None, true),
                Throws.ArgumentException
                    .With.Message.Contains("A success result must have a non-default value.")
                    .And.Property("ParamName").EqualTo("value"));
        }

        [Test]
        public void Constructor_SuccessWithError_ThrowsArgumentException()
        {
            // arrange
            const int value = 5;

            // act & assert
            Assert.That(() => new Result<int>(value, TestError, true),
                Throws.ArgumentException
                    .With.Message.Contains("A success result cannot have an error message.")
                    .And.Property("ParamName").EqualTo("error"));
        }

        [Test]
        public void Constructor_FailureWithoutError_ThrowsArgumentException()
        {
            // arrange
            const int value = 5;

            // act & assert
            Assert.That(() => new Result<int>(value, Errors.None, false),
                Throws.ArgumentException
                    .With.Message.Contains("A failure result must have a non-empty error message.")
                    .And.Property("ParamName").EqualTo("error"));
        }

        [Test]
        public void Constructor_ValidSuccess_DoesNotThrow()
        {
            // arrange
            const int value = 5;

            // act & assert
            Assert.That(() => new Result<int>(value, Errors.None, true), Throws.Nothing);
        }

        [Test]
        public void Constructor_ValidFailure_DoesNotThrow()
        {
            // arrange
            int value = default;

            // act & assert
            Assert.That(() => new Result<int>(value, TestError, false), Throws.Nothing);
        }

        [Test]
        public void GetValueOrThrow_OnSuccess_ReturnsValue()
        {
            // arrange
            const string expectedValue = "Success!";
            var successResult = Result.Success(expectedValue);

            // act
            var actualValue = successResult.GetValueOrThrow();

            // assert
            Assert.That(actualValue, Is.EqualTo(expectedValue));
        }

        [Test]
        public void GetValueOrThrow_OnFailure_ThrowsInvalidOperationExceptionWithMessageContainingError()
        {
            // arrange
            var failureResult = Result.Failure<string>(Errors.Unknown);

            // act & assert
            Assert.That(failureResult.GetValueOrThrow,
                Throws.InvalidOperationException
                    .With.Message.Contains("Cannot access value as the operation failed."));
        }

        [Test]
        public void ImplicitOperator_FromValueToResult_CreatesSuccessResult()
        {
            // arrange
            const int value = 77;

            // act
            Result<int> result = value;

            // assert
            using (Assert.EnterMultipleScope())
            {
                Assert.That(result.IsSuccess, Is.True);
                Assert.That(result.Value, Is.EqualTo(value));
            }
        }
    }
}