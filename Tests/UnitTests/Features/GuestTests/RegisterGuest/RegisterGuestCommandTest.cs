using VIAEventAssociation.Core.Application.CommandDispatching.Commands.Guest;
using VIAEventAssociation.Core.Tools.OperationResult;
using Xunit;

namespace UnitTests.Features.GuestTests.RegisterGuest;

public class RegisterGuestCommandTest
{
    // UC10 Sunny
    [Fact]
    public void Create_WithValidData_ShouldReturnSuccess()
    {
        // Arrange
        var guestId = "GID" + Guid.NewGuid();
        var firstName = "John";
        var lastName = "Doe";
        var email = "jdoe@via.dk";

        // Act
        var result = RegisterGuestCommand.Create(guestId, firstName, lastName, email);

        // Assert
        Assert.True(result.IsSuccess);
        Assert.Equal(guestId, result.Payload.Id.Value.ToString());
        Assert.Equal(firstName, result.Payload.FirstName);
        Assert.Equal(lastName, result.Payload.LastName);
        Assert.Equal(email, result.Payload.Email);
    }

    // UC10 Rainy
    [Fact]
    public void Create_WithBlankFields_ShouldReturnFailure()
    {
        // Act
        var result = RegisterGuestCommand.Create("", "", "", "");

        // Assert
        Assert.True(result.IsFailure);
        Assert.Contains(Error.BlankString, result.Error.GetAllErrors());
    }
}