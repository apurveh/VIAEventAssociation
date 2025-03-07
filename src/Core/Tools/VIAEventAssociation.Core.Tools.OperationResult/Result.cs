namespace VIAEventAssociation.Core.Tools.OperationResult;

public class Result
{
    public static readonly Result Ok = new(true, null);
    
    protected Result(bool isSuccess, Error error) {
        IsSuccess = isSuccess;
        Error = error;
    }
    
    public bool IsSuccess { get; }
    public Error Error { get; }
    public bool IsFailure => !IsSuccess;

    public static Result Fail(Error error) {
        return new Result(false, error);
    }

    public static Result Success() {
        return Ok;
    }

    public static implicit operator Result(Error error) {
        return Fail(error);
    }

    public static implicit operator Result(bool successFlag) {
        return successFlag ? Success() : Fail(Error.Unknown);
    }

    public Result OnSuccess(Action action) {
        if (IsSuccess) {
            action?.Invoke();
        }

        return this;
    }

    public Result OnFailure(Action<Error> action) {
        if (IsFailure) {
            action?.Invoke(this.Error);
        }

        return this;
    }
}

public class Result<T> : Result
{
    private Result(bool isSuccess, T value, Error error) : base(isSuccess, error) {
        Payload = value;
    }

    public T Payload { get; }

    public static Result<T> Ok(T value) {
        return new Result<T>(true, value, null);
    }

    public static Result<T> Fail(Error error) {
        return new Result<T>(false, default(T), error);
    }

    public static Result<T> Success(T value) {
        return Ok(value);
    }

    public static implicit operator Result<T>(T value) {
        return Success(value);
    }
    
    public static implicit operator Result<T>(Error error) {
        return Fail(error);
    }

    public Result<T> OnSuccess(Action<T> action) {
        if (IsSuccess) {
            action?.Invoke(Payload);
        }

        return this;
    }

    public Result<T> OnFailure(Action<Error> action) {
        if (IsFailure) {
            action?.Invoke(this.Error);
        }

        return this;
    }
}