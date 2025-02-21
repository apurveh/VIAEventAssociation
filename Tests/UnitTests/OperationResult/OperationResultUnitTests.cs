using VIAEventAssociation.Core.Tools.OperationResult;

namespace UnitTests.OperationResult;

public class OperationResultUnitTests
{
    [Fact]
    public void Failure_Should_Set_IsSuccess_To_False()
    {
        var error = Error.BadRequest;
        var result = Result.Failure(error);
        
        Assert.False(result.IsSuccess);
    }
    
    [Fact]
    public void Result_Failure_Should_Contain_Error_Info()
    {
        var error = Error.NotFound;
        var result = Result.Failure(error);

        Assert.Equal(error, result.Error);
    }
    
    [Fact]
    public void Success_Should_Set_IsSuccess_To_True()
    {
        var result = Result.Success();
        
        Assert.True(result.IsSuccess);
    }
    
    [Fact]
    public void Result_Success_Should_Contain_NoError_Code()
    {
        var error = Error.NoError;
        var result = Result.Success();

        Assert.Equal(error.Code, result.Error.Code);
    }
    
    [Fact]
    public void Implicit_Conversion_From_Error_To_Result_Should_Fail_And_Contain_Error_Info()
    {
        var error = Error.NotFound;
        Result result = error;

        Assert.False(result.IsSuccess);
        Assert.Equal(error, result.Error);
    }
    
    [Fact]
    public void Implicit_Conversion_From_SuccessFlag_To_Result_Should_Fail_And_Contain_NoError_Code()
    {
        var successFlag = false;
        Result result = successFlag;

        Assert.False(result.IsSuccess);
        Assert.Equal(Error.NoError.Code, result.Error.Code);
    }
    
    [Fact]
    public void Implicit_Conversion_From_SuccessFlag_To_Result_Should_Succeed_And_Contain_NoError_Code()
    {
        var successFlag = true;
        Result result = successFlag;

        Assert.True(result.IsSuccess);
        Assert.Equal(Error.NoError.Code, result.Error.Code);
    }
    
    [Fact]
    public void ResultT_Failure_Should_Not_Set_Value()
    {
        var error = Error.Unauthorized;
        var result = Result<string>.Failure(error);
        
        Assert.Null(result.Value);
    }
    
    [Fact]
    public void ResultT_Failure_Should_Set_IsSuccess_To_False()
    {
        var error = Error.Unauthorized;
        var result = Result<string>.Failure(error);
        
        Assert.False(result.IsSuccess);
    }
    
    [Fact]
    public void ResultT_Failure_Should_Contain_Error_Info()
    {
        var error = Error.Unauthorized;
        var result = Result<string>.Failure(error);
        
        Assert.Equal(error, result.Error);
    }
    
    [Fact]
    public void ResultT_Success_Should_Set_Value()
    {
        var expectedResult = "Test";
        var result = Result<string>.Success(expectedResult);
        
        Assert.Equal(expectedResult, result.Value);
    }
    
    [Fact]
    public void ResultT_Success_Should_Set_IsSuccess_To_True()
    {
        var expectedResult = "Test";
        var result = Result<string>.Success(expectedResult);
        
        Assert.True(result.IsSuccess);
    }
    
    [Fact]
    public void ResultT_Success_Should_Set_NoError_Code()
    {
        var expectedResult = "Test";
        var result = Result<string>.Success(expectedResult);
        
        Assert.Equal(Error.NoError.Code, result.Error.Code);
    }

    [Fact]
    public void Implicit_Conversion_From_Value_To_ResultT_Should_Succeed()
    {
        string testValue = "Hello World!";
        Result<string> result = testValue;

        Assert.True(result.IsSuccess);
        Assert.Equal(testValue, result.Value);
        Assert.Equal(Error.NoError.Code, result.Error.Code);
    }
    
    [Fact]
    public void Implicit_Conversion_From_Error_To_ResultT_Should_Fail()
    {
        var error = Error.InternalServerError;
        Result<string> result = error;

        Assert.False(result.IsSuccess);
        Assert.Null(result.Value);
        Assert.Equal(error, result.Error);
    }
    
    [Fact]
    public void MultipleErrors_Should_Be_Handled()
    {
        var errors = Error.MultipleErrors(Error.ErrorCode.BadRequest, Error.ErrorCode.NotFound);
        Assert.Contains(errors, e => e.Code == (int)Error.ErrorCode.BadRequest);
        Assert.Contains(errors, e => e.Code == (int)Error.ErrorCode.NotFound);
    }
    
    [Fact]
    public void ResultT_With_Null_Value_Should_Still_Succeed()
    {
        var result = Result<string>.Success(null);

        Assert.True(result.IsSuccess);
        Assert.Null(result.Value);
    }
    
    [Fact]
    public void ResultT_With_Null_Error_Should_Still_Succeed()
    {
        var result = Result<string>.Failure(null);

        Assert.False(result.IsSuccess);
        Assert.Null(result.Error);
    }
    
    [Fact]
    public void Error_Should_Contain_Message()
    {
        var error = Error.BadRequest;
        Assert.Equal("Bad request", error.Message);
    }
    
    [Fact]
    public void Exception_Should_Create_Error_With_InternalServerError_Code()
    {
        var exception = new Exception("Unexpected error occurred.");
        var error = Error.Exception(exception);

        Assert.Equal((int)Error.ErrorCode.InternalServerError, error.Code);
        Assert.Equal(exception.Message, error.Message);
    }
    
    [Fact]
    public void OnSuccess_Should_Invoke_Action_If_Success()
    {
        var result = Result.Success();
        var actionInvoked = false;
        result.OnSuccess(() => actionInvoked = true);
        Assert.True(actionInvoked);
    }
    
    [Fact]
    public void OnSuccess_Should_Not_Invoke_Action_If_Failure()
    {
        var result = Result.Failure(Error.BadRequest);
        var actionInvoked = false;
        result.OnSuccess(() => actionInvoked = true);
        Assert.False(actionInvoked);
    }

    [Fact]
    public void OnFailure_Should_Invoke_Action_If_Failure()
    {
        var result = Result.Failure(Error.BadRequest);
        var actionInvoked = false;
        result.OnFailure(() => actionInvoked = true);
        Assert.True(actionInvoked);
    }
    
    [Fact]
    public void OnSuccess_Should_Invoke_ActionT_If_Success()
    {
        var result = Result<int>.Success(200);
        var actionInvoked = false;
        
        result.OnSuccess(value =>
        {
            actionInvoked = true;
            Assert.Equal(200, value);
        });
        
        Assert.True(actionInvoked);
    }
    
    [Fact]
    public void OnFailure_Should_Invoke_ActionError_If_Failure()
    {
        var error = Error.InternalServerError;
        var result = Result<int>.Failure(error);
        var actionInvoked = false;
        
        result.OnFailure(err =>
        {
            actionInvoked = true;
            Assert.Equal(error, err);
        });
        
        Assert.True(actionInvoked);
    }
}