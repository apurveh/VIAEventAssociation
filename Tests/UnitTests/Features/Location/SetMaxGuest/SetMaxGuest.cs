namespace UnitTests.Features.Location.SetMaxGuest;

public class SetMaxNumberOfGuestsTests {
    // Given an existing location, when owner sets the maximum number of guests, then the maximum number of guests is set to the selected value
    // ID: UC18.S1
    [Theory]
    [InlineData(5)]
    [InlineData(10)]
    [InlineData(25)]
    [InlineData(50)]
    public void SetMaxNumberOfGuests_ValidMaxGuests_MaxGuestsSet(int maxGuests) {
        // Arrange
        var location = LocationFactory.Init()
            .Build();

        // Act
        var result = location.SetMaxNumberOfGuests(maxGuests);

        // Assert
        Assert.True(result.IsSuccess);
        Assert.Equal(maxGuests, location.MaxNumberOfGuests.Value);
    }

    // Given an existing location, when owner sets the number of maximum guests to number < minimum required, then a failure message is provided explaining the minimum number of guests
    // ID:UC18.F1
    [Fact]
    public void SetMaxNumberOfGuests_MaxGuestsLessThanMinimum_FailureMessageReturned() {
        // Arrange
        var location = LocationFactory.Init().Build();
        var minMaxGuests = CONST.MIN_NUMBER_OF_GUESTS - 1; // Assuming there's a minimum constraint

        // Act
        var result = location.SetMaxNumberOfGuests(minMaxGuests);

        // Assert
        Assert.True(result.IsFailure);
        Assert.Contains(Error.TooFewGuests(CONST.MIN_NUMBER_OF_GUESTS).Message, result.Error.Message);
    }

    // Given an existing location, when owner sets the number of maximum guests to a number > maximum allowed, then a failure message is provided explaining the maximum number of guests
    // ID:UC18.F2
    [Fact]
    public void SetMaxNumberOfGuests_MaxGuestsMoreThanMaximum_FailureMessageReturned() {
        // Arrange
        var location = LocationFactory.Init().Build();
        var maxMaxGuests = CONST.MAX_NUMBER_OF_GUESTS + 1; // Assuming there's a maximum constraint

        // Act
        var result = location.SetMaxNumberOfGuests(maxMaxGuests);

        // Assert
        Assert.True(result.IsFailure);
        Assert.Contains(Error.TooManyGuests(CONST.MAX_NUMBER_OF_GUESTS).Message, result.Error.Message);
    }
}