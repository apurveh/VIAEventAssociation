using ViaEventAssociation.Core.Application.Features.Commands.Event;

namespace UnitTests.Features.Event.InviteGuest;

public class InviteGuessCommandTest {
    // ID:UC13.S1
    [Fact]
    public void Create_ValidInput_Success() {
        // Arrange
        var eventId = "EID" + Guid.NewGuid();
        var guestId = "GID" + Guid.NewGuid();

        // Act
        var result = InviteGuestCommand.Create(eventId, guestId);
        var cmd = result.Payload;

        // Assert
        Assert.True(result.IsSuccess);
        Assert.Equal(guestId, cmd.GuestId.Value);
        Assert.Equal(eventId, cmd.Id.Value);
    }

    // ID:UC13.F1
    [Fact]
    public void Create_WithInvalidEventId_ShouldReturnError() {
        // Arrange
        var eventId = "EDD" + Guid.NewGuid();
        var guestId = "GDD" + Guid.NewGuid();

        // Act
        var result = InviteGuestCommand.Create(eventId, guestId);

        // Assert
        Assert.True(result.IsFailure);
        Assert.Contains(Error.InvalidPrefix, result.Error.GetAllErrors());
    }
}