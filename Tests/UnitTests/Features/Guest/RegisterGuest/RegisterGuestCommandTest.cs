using ViaEventAssociation.Core.Application.Features.Commands.Guest;

namespace UnitTests.Features.Guest.RegisterGuest;

public class RegisterGuestCommandTest {
    // ID:UC10.S1
    [Fact]
    public void Create_ValidInput_Success() {
        // Arrange
        var firstName = "John";
        var lastName = "Doe";
        var email = "JDH@via.dk";

        // Act
        var result = RegisterGuestCommand.Create(firstName, lastName, email);
        var cmd = result.Payload;

        // Assert
        Assert.True(result.IsSuccess);
        Assert.Equal(firstName, cmd.FirstName.Value);
        Assert.Equal(lastName, cmd.LastName.Value);
        Assert.Equal(email, cmd.Email.Value);
    }

    // ID:UC10.F1
    [Fact]
    public void Create_WithInvalidEmailDomain_ShouldReturnError() {
        // Arrange
        var firstName = "John";
        var lastName = "Doe";
        var email = "jJDH@google.com";

        // Act
        var result = RegisterGuestCommand.Create(firstName, lastName, email);

        // Assert
        Assert.True(result.IsFailure);
        Assert.Contains(Error.InvalidEmailDomain, result.Error.GetAllErrors());
    }
}