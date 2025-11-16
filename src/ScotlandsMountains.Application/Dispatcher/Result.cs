namespace ScotlandsMountains.Application.Dispatcher;

internal class Result
{
    public static Result Success() => new Result(true, null);

    public static Result Failure(string errorMessage) => new(false, errorMessage);

    protected Result(bool isSuccess, string? errorMessage)
    {
        IsSuccess = isSuccess;
        ErrorMessage = errorMessage;
    }

    public bool IsSuccess { get; }

    public string? ErrorMessage { get; }
}

internal class Result<T> : Result
{
    public static Result<T> Success(T value) => new Result<T>(true, null, value);
 
    public static new Result<T> Failure(string errorMessage) => new Result<T>(false, errorMessage, default!);
    
    private Result(bool isSuccess, string? errorMessage, T value)
        : base(isSuccess, errorMessage)
    {
        Value = value;
    }
    
    public T Value { get; }
}