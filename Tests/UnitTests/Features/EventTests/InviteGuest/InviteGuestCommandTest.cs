using VIAEventAssociation.Core.Application.CommandDispatching.Commands.Event;
using VIAEventAssociation.Core.Tools.OperationResult;

namespace UnitTests.Features.EventTests.InviteGuest;

public class InviteGuestCommandTest
{
    // UC13.S1
    [Fact]
    public void Create_ValidInput_Success()
    {
        // Arrange
        var eventId = "EID" + Guid.NewGuid();
        var guestId = "GID" + Guid.NewGuid();
        
        // Act
        var result = InviteGuestCommand.Create(eventId, guestId);
        var command = result.Payload;
        
        // Assert
        Assert.True(result.IsSuccess);
        Assert.Equal(eventId, command.Id.Value);
        Assert.Equal(guestId, command.GuestId.Value);
    }
    
    // UC13.F1
    [Fact]
    public void Create_WithInvalidEventId_ShouldReturnError()
    {
        // Arrange
        var eventId = "EDD" + Guid.NewGuid();
        var guestId = "GID" + Guid.NewGuid();
        
        // Act
        var result = InviteGuestCommand.Create(eventId, guestId);
        
        // Assert
        Assert.True(result.IsFailure);
        Assert.Contains(Error.InvalidPrefix, result.Error.GetAllErrors());
    }
}