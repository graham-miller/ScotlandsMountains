namespace ScotlandsMountains.Shared;

public static class Result
{
    public static Result<bool> Success() => new(true, Errors.None, true);

    public static Result<T> Success<T>(T value) => new(value, Errors.None, true);

    public static Result<T> Failure<T>(Error error) => new(default, error, false);

    public static Result<bool> Failure(Error error) => new(false, error, false);
}

public class Result<T>
{
    internal Result(T value, Error error, bool isSuccess)
    {
        if (isSuccess && EqualityComparer<T>.Default.Equals(value, default) && typeof(T) != typeof(void) && typeof(T) != typeof(bool))
            throw new ArgumentException("A success result must have a non-default value.", nameof(value));

        if (isSuccess && error != Errors.None)
            throw new ArgumentException("A success result cannot have an error message.", nameof(error));

        if (!isSuccess && error == Errors.None)
            throw new ArgumentException("A failure result must have a non-empty error message.", nameof(error));

        IsSuccess = isSuccess;
        Error = error;
        Value = value;
    }

    public bool IsSuccess { get; }

    public bool IsFailure => !IsSuccess;

    public T Value { get; }

    public Error Error { get; }

    public T GetValueOrThrow()
    {
        if (IsFailure)
            throw new InvalidOperationException($"Cannot access Value. The operation failed: {Error}");
        
        return Value;
    }

    public static implicit operator Result<T>(T value) => Result.Success(value);
}

public enum Errors { None, NotFound, BadRequest, Unknown }

public record Error(Errors Type)
{
    public static implicit operator Error(Errors type) => new(type);
}
