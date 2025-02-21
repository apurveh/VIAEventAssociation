namespace VIAEventAssociation.Core.Tools.OperationResult;

public class Result
{
    public bool IsSuccess { get; }
    public Error Error { get; }
    public bool IsFailure => !IsSuccess;
    
    protected Result(bool isSuccess, Error error)
    {
        IsSuccess = isSuccess;
        Error = error;
    }
    
    // Factory methods
    public static Result Failure(Error error) => new Result(false, error);
    public static Result Success() => new Result(true, Error.NoError);
    
    // Implicit operators
    public static implicit operator Result(Error error) => Failure(error);
    public static implicit operator Result(bool successFlag) => successFlag ? Success() : Failure(Error.NoError);

    public void OnSuccess(Action action)
    {
        if (action == null) throw new ArgumentNullException(nameof(action));
        if (IsSuccess) action.Invoke();
    }
    
    public void OnFailure(Action action)
    {
        if (action == null) throw new ArgumentNullException(nameof(action));
        if (IsFailure) action.Invoke();
    }
}

public class Result<T> : Result
{
    public T Value { get; }
    
    private Result(bool isSuccess, T value, Error error) : base(isSuccess, error)
    {
        Value = value;
    }
    
    // Factory methods
    public new static Result<T> Failure(Error error) => new Result<T>(false, default(T), error);
    public static Result<T> Success(T value) => new Result<T>(true, value, Error.NoError);
    
    // Implicit operators
    public static implicit operator Result<T>(T value) => Success(value);
    public static implicit operator Result<T>(Error error) => Failure(error);
    
    public void OnSuccess(Action<T> action)
    {
        if (action == null) throw new ArgumentNullException(nameof(action));
        if (IsSuccess) action.Invoke(Value);
    }
    
    public void OnFailure(Action<Error> action)
    {
        if (action == null) throw new ArgumentNullException(nameof(action));
        if (IsFailure) action.Invoke(Error);
    }
}