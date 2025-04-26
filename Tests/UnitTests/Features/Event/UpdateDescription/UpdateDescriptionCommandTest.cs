using ViaEventAssociation.Core.Application.Features.Commands.Event;

namespace UnitTests.Features.Event.UpdateDescription;

public class UpdateDescriptionCommandTest {
    //ID:UC3.S1
    [Fact]
    public void Create_UpdateDescriptionCommand_WithValidData() {
        //Arrange
        var guid = "EID" + Guid.NewGuid();
        var description = "Description";

        //Act
        var result = UpdateDescriptionCommand.Create(guid, description);

        //Assert
        Assert.True(result.IsSuccess);
        Assert.Equal(guid, result.Payload.Id.Value);
    }

    //ID:UC3.F1
    [Fact]
    public void Create_UpdateDescriptionCommand_WithEmptyId() {
        //Arrange
        var guid = "";
        var description = "Description";

        //Act
        var result = UpdateDescriptionCommand.Create(guid, description);

        //Assert
        Assert.True(result.IsFailure);
        Assert.Contains(Error.BlankString.Message, result.Error.Message);
    }
}