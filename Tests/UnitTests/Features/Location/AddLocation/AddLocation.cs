using ViaEventAssociation.Core.Domain.Agregates.Locations;

public class AddLocation {
    // Create location with null id should produce draft status, and the maximum number of guests is 5
    // ID:UC16.S1
    [Fact]
    public void AddLocation_WithValidNameAndMaxGuests_ShouldReturnSuccess() {
        // Arrange

        // Act
        var result = Location.Create();

        // Assert
        Assert.True(result.IsSuccess);
    }

    // Create location with null id should produce draft status, and the maximum number of guests is 5
    // ID:UC16.S2
    [Fact]
    public void AddLocation_WithValidNameAndMaxGuests_ShouldReturnMaxGuestsOf5() {
        // Arrange

        // Act
        var result = Location.Create();

        // Assert
        Assert.Equal(5, result.Payload.MaxNumberOfGuests.Value);
    }

    // Create location with null id should produce draft status, and the maximum number of guests is 5
    // ID:UC16.S3
    [Fact]
    public void AddLocation_WithValidNameAndMaxGuests_ShouldReturnAnEmptyListOfEvents() {
        // Arrange

        // Act
        var result = Location.Create();

        // Assert
        Assert.Empty(result.Payload.Events);
    }
}