using ViaEventAssociation.Core.Application.Features.Commands.Event;

namespace UnitTests.Features.Event.UpdateTitle;

public class UpdateTitleCommandTest {
    // ID:UC2.S1
    [Fact]
    public void Create_ValidInput_Success() {
        // Arrange
        var guid = "EID" + Guid.NewGuid();
        var title = "New Title";


        // Act
        var result = UpdateTitleCommand.Create(guid, title);
        var cmd = result.Payload;

        // Assert
        Assert.True(result.IsSuccess);
        Assert.Equal(guid, cmd.Id.Value);
        Assert.Equal(title, cmd.Title.Value);
        Assert.Equal(39, cmd.Id.Value.Length);
    }

    // ID:UC2.F1
    [Fact]
    public void Create_EmptyId_Failure() {
        // Arrange
        var guid = "";
        var title = "New Title";

        // Act
        var result = UpdateTitleCommand.Create(guid, title);

        // Assert
        Assert.True(result.IsFailure);
        Assert.Contains(Error.BlankString.Message, result.Error.Message);
    }
}