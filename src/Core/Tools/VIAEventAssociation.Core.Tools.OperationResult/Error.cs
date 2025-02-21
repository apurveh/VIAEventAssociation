namespace VIAEventAssociation.Core.Tools.OperationResult;

public class Error
{
    public int Code { get; }
    public string Message { get; }
    
    private Error(int code, string message)
    {
        Code = code;
        Message = message;
    }
    
    public enum ErrorCode
    { 
        NoError = 0,
        
        // Http error codes
        BadRequest = 400,
        Unauthorized = 401,
        Forbidden = 403,
        NotFound = 404,
        InternalServerError = 500,
        NotImplemented = 501,
        
        // Custom error codes
        InvalidEmail = 1001
    }

    // List of multiple errors if occurred
    public static List<Error> MultipleErrors(params ErrorCode[] codes) =>
        codes.Select(code => new Error((int)code, GetMessage(code))).ToList();

    // If no code exists in dictionary it will return the default message "An unexpected error occurred."
    private static string GetMessage(ErrorCode code) =>
        Messages.GetValueOrDefault(code, "An unexpected error occurred.");
    
    public static readonly Dictionary<ErrorCode, string> Messages = new Dictionary<ErrorCode, string>
    {
        { ErrorCode.NoError, "No error" },
        
        // Http error messages
        { ErrorCode.BadRequest, "Bad request" },
        { ErrorCode.Unauthorized, "Unauthorized" },
        { ErrorCode.Forbidden, "Forbidden" },
        { ErrorCode.NotFound, "Not found" },
        { ErrorCode.InternalServerError, "Internal server error" },
        { ErrorCode.NotImplemented, "Not implemented" },
        
        // Custom error messages
        { ErrorCode.InvalidEmail, "Invalid email format" }
    };
    
    public static Error NoError => new Error((int) ErrorCode.NoError, GetMessage(ErrorCode.NoError));
    public static Error BadRequest => new Error((int) ErrorCode.BadRequest, GetMessage(ErrorCode.BadRequest));
    public static Error Unauthorized => new Error((int) ErrorCode.Unauthorized, GetMessage(ErrorCode.Unauthorized));   
    public static Error Forbidden => new Error((int) ErrorCode.Forbidden, GetMessage(ErrorCode.Forbidden));   
    public static Error NotFound => new Error((int) ErrorCode.NotFound, GetMessage(ErrorCode.NotFound));   
    public static Error InternalServerError => new Error((int) ErrorCode.InternalServerError, GetMessage(ErrorCode.InternalServerError));   
    public static Error NotImplemented => new Error((int) ErrorCode.NotImplemented, GetMessage(ErrorCode.NotImplemented));   
    public static Error InvalidEmail => new Error((int) ErrorCode.InvalidEmail, GetMessage(ErrorCode.InvalidEmail));   
    public static Error Exception(Exception exception) => new Error((int) ErrorCode.InternalServerError, exception.Message);
}