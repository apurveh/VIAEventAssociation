﻿using VIAEventAssociation.Core.Application.CommandDispatching.Commands.Guest;
using VIAEventAssociation.Core.Tools.OperationResult;

namespace UnitTests.Features.GuestTests.ParticipatePublicEvent;

public class GuestParticipatePublicCommandTest
{
    // UC11.S1
    [Fact]
    public void Create_ValidInput_Success()
    {
        // Arrange
        var eventId = "EID" + Guid.NewGuid();
        var guestId = "GID" + Guid.NewGuid();
        
        // Act
        var result = RequestToJoinCommand.Create(eventId, guestId);
        var command = result.Payload;
        
        // Assert
        Assert.True(result.IsSuccess);
        Assert.Equal(guestId, command.Id.Value);
        Assert.Equal(eventId, command.EventId.Value);
    }
    
    // UC11.F1
    [Fact]
    public void Create_WithInvalidEventId_ShouldReturnError()
    {
        // Arrange
        var eventId = "EDD" + Guid.NewGuid();
        var guestId = "GID" + Guid.NewGuid();
        
        // Act
        var result = RequestToJoinCommand.Create(eventId, guestId);
        
        // Assert
        Assert.True(result.IsFailure);
        Assert.Contains(Error.InvalidPrefix, result.Error.GetAllErrors());
    }
}