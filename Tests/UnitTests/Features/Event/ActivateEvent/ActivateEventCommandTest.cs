using System;
using ViaEventAssociation.Core.Application.Features.Commands.Event;
using Xunit;

namespace UnitTests.Features.Event.ActivateEvent;

public class ActivateEventCommandTest {
    //ID:UC9.S1
    [Fact]
    public void Create_ActivateEventCommand_WithValidData() {
        //Arrange
        var guid = "EID" + Guid.NewGuid();

        //Act
        var result = ActivateEventCommand.Create(guid);

        //Assert
        Assert.True(result.IsSuccess);
        Assert.Equal(guid, result.Payload.Id.Value);
    }

    //ID:UC9.F1
    [Fact]
    public async void Create_WithInvalidData_Failure() {
        //Arrange
        var guid = "EID" + "Invalid";

        //Act
        var result = ActivateEventCommand.Create(guid);

        //Assert
        Assert.True(result.IsFailure);
        Assert.Equal(Error.InvalidLength, result.Error);
    }
}