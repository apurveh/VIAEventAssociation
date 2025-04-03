using VIAEventAssociation.Core.Application.CommandDispatching.Commands.Event;
using VIAEventAssociation.Core.Tools.OperationResult;

namespace UnitTests.Features.EventTests.MakeEventPrivate;

public class MakeEventPrivateCommandTest
{
    [Fact]
    public void Create_WithValidEventId_ShouldReturnSuccess()
    {
        // Arrange
        var eventId = "EID" + Guid.NewGuid();

        // Act
        var result = MakeEventPrivateCommand.Create(eventId);

        // Assert
        Assert.True(result.IsSuccess);
        Assert.Equal(eventId, result.Payload.Id.Value.ToString());
    }
    
    // UC6.F1 – Invalid command creation
    [Fact]
    public void Create_WithInvalidEventId_ShouldReturnFailure()
    {
        // Arrange
        var eventId = "EDD" + Guid.NewGuid();

        // Act
        var result = MakeEventPrivateCommand.Create(eventId);

        // Assert
        Assert.True(result.IsFailure);
        Assert.Contains(Error.InvalidPrefix, result.Error.GetAllErrors());
    }

}