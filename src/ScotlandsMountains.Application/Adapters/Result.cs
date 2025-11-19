namespace ScotlandsMountains.Application.Adapters;

public class Result
{
    internal static Result Success() => new Result(true, null);

    internal static Result Failure(string errorMessage) => new(false, errorMessage);

    protected Result(bool isSuccess, string? errorMessage)
    {
        IsSuccess = isSuccess;
        ErrorMessage = errorMessage;
    }

    public bool IsSuccess { get; }

    public string? ErrorMessage { get; }
}

public class Result<T> : Result
{
    internal static Result<T> Success(T value) => new Result<T>(true, null, value);

    internal static new Result<T> Failure(string errorMessage) => new Result<T>(false, errorMessage, default!);
    
    private Result(bool isSuccess, string? errorMessage, T value)
        : base(isSuccess, errorMessage)
    {
        Value = value;
    }
    
    public T Value { get; }
}