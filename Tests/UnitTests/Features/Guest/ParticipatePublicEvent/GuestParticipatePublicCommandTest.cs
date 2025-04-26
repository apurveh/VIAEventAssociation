using ViaEventAssociation.Core.Application.CommandDispatching.Commands.Guest;

namespace UnitTests.Features.Guest.GuestRequestToJoin;

public class GuestParticipatePublicCommandTest {
    // ID:UC11.S1
    [Fact]
    public void Create_ValidInput_Success() {
        // Arrange
        var eventId = "EID" + Guid.NewGuid();
        var guestId = "GID" + Guid.NewGuid();


        // Act
        var result = RequestToJoinCommand.Create(eventId, guestId);
        var cmd = result.Payload;

        // Assert
        Assert.True(result.IsSuccess);
        Assert.Equal(guestId, cmd.Id.Value);
        Assert.Equal(eventId, cmd.EventId.Value);
    }

    // ID:UC11.F1
    [Fact]
    public void Create_WithInvalidEventId_ShouldReturnError() {
        // Arrange
        var eventId = "EDD" + Guid.NewGuid();
        var guestId = "GDD" + Guid.NewGuid();

        // Act
        var result = RequestToJoinCommand.Create(eventId, guestId);

        // Assert
        Assert.True(result.IsFailure);
        Assert.Contains(Error.InvalidPrefix, result.Error.GetAllErrors());
    }
}