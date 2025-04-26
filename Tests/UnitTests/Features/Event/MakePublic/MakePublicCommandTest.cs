using ViaEventAssociation.Core.Application.Features.Commands.Event;

namespace UnitTests.Features.Event.MakePrivate;

public class MakePublicCommandTest {
    //ID:UC5.S1
    [Fact]
    public void Create_MakePublicCommand_WithValidData() {
        //Arrange
        var guid = "EID" + Guid.NewGuid();

        //Act
        var result = MakePublicCommand.Create(guid);

        //Assert
        Assert.True(result.IsSuccess);
        Assert.Equal(guid, result.Payload.Id.Value);
    }

    //ID:UC5.F1
    [Fact]
    public void Create_MakePublicCommand_WithEmptyId() {
        //Arrange
        var guid = "";

        //Act
        var result = MakePublicCommand.Create(guid);

        //Assert
        Assert.True(result.IsFailure);
        Assert.Contains(Error.BlankString.Message, result.Error.Message);
    }
}