public class Result {
    public static readonly Result Ok = new(true, null);

    protected Result(bool isSuccess, Error error) {
        IsSuccess = isSuccess;
        Error = error;
    }

    public bool IsSuccess { get; }
    public Error Error { get; }
    public bool IsFailure => !IsSuccess;

    // Overloaded factory method for failure with multiple errors
    public static Result Fail(Error error) {
        return new Result(false, error);
    }

    // Factory method for success
    public static Result Success() {
        return Ok;
    }

    // Implicit operator for converting ErrorCollection to a Result
    public static implicit operator Result(Error error) {
        return Fail(error);
    }

    // Implicit operator for converting a bool (success flag) to a Result
    public static implicit operator Result(bool successFlag) {
        return successFlag ? Success() : Fail(Error.Unknown);
    }

    public Result OnSuccess(Action action) {
        if (IsSuccess) {
            action?.Invoke();
        }

        return this; // Return current Result for chaining
    }

    public Result OnFailure(Action<Error> action) {
        if (IsFailure) {
            action?.Invoke(this.Error);
        }

        return this; // Return current Result for chaining
    }
}

public class Result<T> : Result {
    private Result(bool isSuccess, T value, Error error) : base(isSuccess, error) {
        Payload = value;
    }

    public T Payload { get; }

    public static Result<T> Ok(T value) {
        return new Result<T>(true, value, null); // Clearly indicates a successful result with a value
    }

    // Overloaded factory method for failure with multiple errors
    public static Result<T> Fail(Error error) {
        return new Result<T>(false, default(T), error);
    }

    // Factory method for success with generic type
    public static Result<T> Success(T value) {
        return Ok(value);
    }

    // Implicit operator for converting a value to a Result<T>
    public static implicit operator Result<T>(T value) {
        return Success(value);
    }


    // Implicit operator for converting ErrorCollection to a Result<T>
    public static implicit operator Result<T>(Error error) {
        return Fail(error);
    }

    public Result<T> OnSuccess(Action<T> action) {
        if (IsSuccess) {
            action?.Invoke(Payload);
        }

        return this; // Return current Result<T> for chaining
    }

    public Result<T> OnFailure(Action<Error> action) {
        if (IsFailure) {
            action?.Invoke(this.Error);
        }

        return this; // Return current Result<T> for chaining
    }
}