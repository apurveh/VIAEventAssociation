// Assuming use of xUnit for testing

namespace UnitTests.Features.Location.UpdatesName;

public class UpdateNameTests {
    // Update name of an existing location with a name length between minimum and maximum characters
    // Assuming similar constraints to the event title for demonstration
    //ID:UC17.S1
    [Theory]
    [InlineData("Beautiful Venue")] // Middle limit of the name length
    [InlineData("Loc")] // Minimum limit of the name length
    [InlineData("Spacious and Elegant Event Space")] // Maximum limit of the name length
    public void UpdateName_ValidLengthName_NameUpdated(string name) {
        // Arrange
        var location = LocationFactory.Init() // Assuming you have a similar factory setup for locations
            .Build();

        // Act
        var result = location.UpdateName(name);

        // Assert
        Assert.True(result.IsSuccess);
        Assert.Equal(name, location.Name.Value);
    }

    // Update name of an existing location with a name length of 0 characters
    //ID:UC17.F1
    [Fact]
    public void UpdateName_NameLength0Characters_FailureMessageReturned() {
        // Arrange
        var location = LocationFactory.Init().Build();

        // Act
        var result = location.UpdateName("");

        // Assert
        Assert.True(result.IsFailure);
        Assert.Contains(Error.BlankString.Message, result.Error.Message); // Assuming BlankString is an error type in your domain
    }

    // Update name of an existing location with a name length less than minimum required characters
    // ID:UC17.F2
    [Theory]
    [InlineData("No")]
    [InlineData("A")]
    public void UpdateName_NameLengthLessThanMinimum_FailureMessageReturned(string name) {
        // Arrange
        var location = LocationFactory.Init().Build();

        // Act
        var result = location.UpdateName(name);

        // Assert
        Assert.True(result.IsFailure);
        Assert.Contains(Error.TooShortName(CONST.MIN_NAME_LENGTH).Message, result.Error.Message); // Assuming your errors and constants
    }

    // Update name of an existing location with a name length more than maximum allowed characters
    // Assuming max length for location name is similar to the event title length
    // ID:UC17.F3
    [Theory]
    [InlineData("This location name is definitely way too long for any practical use, and should not be accepted by the system for any reason at all, ever, period. It's just too long.")]
    [InlineData("This name is beyond the set limit and should not be accepted by the system for any reason at all, ever, period. It's just too long.")]
    public void UpdateName_NameLengthMoreThanMaximum_FailureMessageReturned(string name) {
        // Arrange
        var location = LocationFactory.Init().Build();

        // Act
        var result = location.UpdateName(name);

        // Assert
        Assert.True(result.IsFailure);
        Assert.Contains(Error.TooLongName(CONST.MAX_NAME_LENGTH), result.Error.GetAllErrors()); // Assuming your errors and constants
    }

    // Update name of an existing location with a null name
    // ID:UC17.F4
    [Fact]
    public void UpdateName_NameNull_FailureMessageReturned() {
        // Arrange
        var location = LocationFactory.Init().Build();

        // Act
        var result = location.UpdateName(null);

        // Assert
        Assert.True(result.IsFailure);
        Assert.Contains(Error.NullString.Message, result.Error.Message); // Assuming NullString is an error type in your domain
    }
}