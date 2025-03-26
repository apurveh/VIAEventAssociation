using Moq;
using VIAEventAssociation.Core.Domain.Aggregates.Guests;
using VIAEventAssociation.Core.Domain.Contracts;
using VIAEventAssociation.Core.Domain.Services;
using VIAEventAssociation.Core.Tools.OperationResult;

namespace UnitTests.Features.GuestTests.RegisterGuest;

public class RegisterGuest
{
    //UC10.S1
    [Fact]
    public void RegisterGuest_WhenValidDataProvided_ShouldRegisterGuest()
    {
        // Arrange
        string firstName = "John";
        string lastName = "Doe";
        string email = "333123@via.dk";

        // Act
        var result = Guest.Create(firstName, lastName, email);

        // Assert
        Assert.True(result.IsSuccess);
        Assert.NotNull(result.Payload);
        Assert.Equal(firstName, result.Payload.FirstName.Value);
        Assert.Equal(lastName, result.Payload.LastName.Value);
        Assert.Equal(email, result.Payload.Email.Value);
    }

    //UC10.F1
    [Fact]
    public void RegisterGuest_WhenEmailDomainIncorrect_ShouldReturnFailure()
    {
        //Arrange
        string firstName = "John";
        string lastName = "Doe";
        string email = "johndoe@gmail.com";

        //Act
        var result = Guest.Create(firstName, lastName, email);

        //Assert
        Assert.True(result.IsFailure);
        Assert.Equal(Error.InvalidEmailDomain, result.Error);
    }

    //UC10.F2
    [Fact]
    public void RegisterGuest_WhenEmailFormatIncorrect_ShouldReturnFailure()
    {
        //Arrange
        string firstName = "John";
        string lastName = "Doe";
        string email = "johndoe@via.dk";

        //Act
        var result = Guest.Create(firstName, lastName, email);

        //Assert
        Assert.True(result.IsFailure);
        Assert.Equal(Error.InvalidEmail, result.Error);
    }

    // UC10.F3
    [Theory]
    [InlineData("J", "Doe", "jdo@via.dk")] // Too short first name
    [InlineData("ThisNameIsWayTooLongToBeValidForAGuest", "Doe", "jdo@via.dk")] // Too long first name
    public void RegisterGuest_WhenFirstNameIsInvalid_ShouldReturnFailure(string firstName, string lastName, string email)
    {
        // Act
        var result = Guest.Create(firstName, lastName, email);

        // Assert
        Assert.True(result.IsFailure);
        Assert.Equal(Error.InvalidNameLength(), result.Error);
    }
    
    //
    // UC10.F4
    [Theory]
    [InlineData("John", "D", "jdo@via.dk")]
    [InlineData("John","ThisNameIsWayTooLongToBeValidForAGuest", "jdo@via.dk")]
    public void RegisterGuest_WhenLastNameIsInvalid_ShouldReturnFailure(string firstName, string lastName, string email)
    {
        // Act
        var result = Guest.Create(firstName, lastName, email);

        // Assert
        Assert.True(result.IsFailure);
        Assert.Equal(Error.InvalidNameLength(), result.Error);
    }
    //UC10.F5
    [Fact]
    public void RegisterGuest_WhenEmailAlreadyExists_ShouldReturnFailure()
    {
        // Arrange
        string firstName = "John";
        string lastName = "Doe";
        string email = "john.doe@via.dk";

        var mockRepo = new Mock<IGuestService>();
        mockRepo.Setup(repo => repo.EmailExists(email)).Returns(true); // Simulate email already in use

        var guestService = new GuestService(mockRepo.Object);

        // Act
        var result = guestService.RegisterGuest(firstName, lastName, email);

        // Assert
        Assert.True(result.IsFailure);
        Assert.Equal(Error.EmailAlreadyExists, result.Error);
    }
    
    //UC10.F6
    [Theory]
    [InlineData("John1", "Doe", "jdo@via.dk")]
    [InlineData("John", "Doe1", "jdo@via.dk")]
    public void RegisterGuest_WhenFirstNameOrLastName_ContainInvalidLetters_ShouldReturnFailure(string firstName, string lastName, string email)
    {
        // Act
        var result = Guest.Create(firstName, lastName, email);

        // Assert
        Assert.True(result.IsFailure);
        Assert.Equal(Error.InvalidName, result.Error);
    }
}