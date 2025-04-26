// using ViaEventAssociation.Core.Tools.OperationResult;
//
// namespace UnitTests.OperationResult;
//
// public class OperationResultUnitTests {
//     [Fact]
//     public void Success_Should_Set_IsSuccess_To_True()
//     {
//         var result = Result.Success();
//
//         Assert.True(result.IsSuccess);
//     }
//
//     [Fact]
//     public void Fail_Should_Set_IsSuccess_To_False()
//     {
//         var error = Error.BadRequest;
//         var result = Result.Fail(error);
//
//         Assert.False(result.IsSuccess);
//         Assert.Equal(error, result.Error);
//     }
//
//     [Fact]
//     public void ResultT_Success_Should_Set_Value()
//     {
//         var expectedValue = "Test";
//         var result = Result<string>.Success(expectedValue);
//
//         Assert.True(result.IsSuccess);
//         Assert.Equal(expectedValue, result.Value);
//     }
//
//     [Fact]
//     public void ResultT_Fail_Should_Not_Set_Value()
//     {
//         var error = Error.BadRequest;
//         var result = Result<string>.Fail(error);
//
//         Assert.False(result.IsSuccess);
//         Assert.Equal(error, result.Error);
//     }
//
//     [Fact]
//     public void Implicit_Conversion_From_Value_To_ResultT_Should_Succeed()
//     {
//         string testValue = "Hello";
//         Result<string> result = testValue; // Implicit conversion
//
//         Assert.True(result.IsSuccess);
//         Assert.Equal(testValue, result.Value);
//     }
//
//     [Fact]
//     public void Implicit_Conversion_From_Error_To_Result_Should_Fail()
//     {
//         Result result = Error.NotFound; // Implicit conversion
//
//         Assert.False(result.IsSuccess);
//         Assert.Equal(Error.NotFound.Code, result.Error.Code);
//     }
//
//     [Fact]
//     public void Result_Failure_Should_Contain_Error_Info()
//     {
//         var error = Error.NotFound;
//         var result = Result.Fail(error);
//
//         Assert.False(result.IsSuccess);
//         Assert.Equal(error, result.Error);
//     }
//
//     [Fact]
//     public void ResultT_Failure_Should_Contain_Error_Info()
//     {
//         var error = Error.InvalidEmail;
//         var result = Result<string>.Fail(error);
//
//         Assert.False(result.IsSuccess);
//         Assert.Equal(error, result.Error);
//     }
//
//     [Fact]
//     public void ResultT_Explicit_Success_Should_Contain_Value()
//     {
//         var result = Result<int>.Success(123);
//
//         Assert.True(result.IsSuccess);
//         Assert.Equal(123, result.Value);
//     }
//
//     [Fact]
//     public void MultipleErrors_Should_Be_Handled()
//     {
//         var errors = Error.MultipleErrors(Error.ErrorCode.BadRequest, Error.ErrorCode.NotFound);
//         Assert.Contains(errors, e => e.Code == (int)Error.ErrorCode.BadRequest);
//         Assert.Contains(errors, e => e.Code == (int)Error.ErrorCode.NotFound);
//     }
//
//     // Boundary tests
//     [Fact]
//     public void ResultT_With_Null_Value_Should_Still_Succeed()
//     {
//         var result = Result<string>.Success(null);
//
//         Assert.True(result.IsSuccess);
//         Console.WriteLine(result);
//         Assert.Null(result.Value);
//     }
//
//     [Fact]
//     public void ResultT_With_Null_Error_Should_Still_Succeed()
//     {
//         var result = Result<string>.Fail(null);
//
//         Assert.False(result.IsSuccess);
//         Assert.Null(result.Error);
//     }
//
//     [Fact]
//     public void Error_should_contain_message()
//     {
//         var error = Error.BadRequest;
//         Assert.Equal("The request could not be understood by the server due to malformed syntax.", error.Message);
//     }
//
//     // Testing Implicit Operators
//
//     [Fact]
//     public void Implicit_Operator_Value_To_ResultT_Should_Return_Success_With_Value()
//     {
//         string testValue = "Test";
//         Result<string> result = testValue; // Using implicit operator
//
//         Assert.True(result.IsSuccess);
//         Assert.Equal(testValue, result.Value);
//     }
//
//     [Fact]
//     public void Implicit_Operator_Error_To_ResultT_Should_Return_Failure_With_Error()
//     {
//         Result<string> result = Error.BadRequest; // Using implicit operator
//
//         Assert.False(result.IsSuccess);
//         Assert.Equal(Error.BadRequest.Code, result.Error.Code);
//     }
//
//     // Testing Exception Handling
//
//     [Fact]
//     public void Exception_Should_Create_Error_With_InternalServerError_Code()
//     {
//         var exception = new Exception("Unexpected error occurred.");
//         var error = Error.Exception(exception);
//
//         Assert.Equal((int)Error.ErrorCode.InternalServerError, error.Code);
//         Assert.Equal(exception.Message, error.Message);
//     }
//
//     [Fact]
//     public void OnSuccess_Should_Invoke_Action_If_Success()
//     {
//         var result = Result.Success();
//         var actionInvoked = false;
//         result.OnSuccess(() => actionInvoked = true);
//         Assert.True(actionInvoked);
//     }
//
//     [Fact]
//     public void OnSuccess_Should_Not_Invoke_Action_If_Failure()
//     {
//         var result = Result.Fail(Error.BadRequest);
//         var actionInvoked = false;
//         result.OnSuccess(() => actionInvoked = true);
//         Assert.False(actionInvoked);
//     }
//
//     [Fact]
//     public void OnFailure_Should_Invoke_Action_If_Failure()
//     {
//         var result = Result.Fail(Error.BadRequest);
//         var actionInvoked = false;
//         result.OnFailure(() => actionInvoked = true);
//         Assert.True(actionInvoked);
//     }
//
// }

