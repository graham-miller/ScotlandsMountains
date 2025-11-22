namespace ScotlandsMountains.Application.Adapters;

public class Result<T>
{
    internal static Result<T> Success(T value) => new(true, null, value);

    internal static Result<T> Failure(string errorMessage) => new(false, errorMessage, default!);
    
    private Result(bool isSuccess, string? errorMessage, T value)
    {
        Value = value;
        IsSuccess = isSuccess;
        ErrorMessage = errorMessage;
    }

    public bool IsSuccess { get; }

    public bool IsFailure => !IsSuccess;

    public string? ErrorMessage { get; }

    public T Value { get; }
}
