using VIAEventAssociation.Core.Application.CommandDispatching.Commands.Event;
using VIAEventAssociation.Core.Tools.OperationResult;

namespace UnitTests.Features.EventTests.ActivateEvent;

public class ActivateEventCommandTest
{
    // UC8 Sunny
    [Fact]
    public void Create_WithValidEventId_ShouldReturnSuccess()
    {
        // Arrange
        var eventId = "EID" + Guid.NewGuid();

        // Act
        var result = ActivateEventCommand.Create(eventId);

        // Assert
        Assert.True(result.IsSuccess);
        Assert.Equal(eventId, result.Payload.Id.Value.ToString());
    }
    
    // UC8 Rainy
    [Fact]
    public void Create_WithInvalidEventId_ShouldReturnFailure()
    {
        // Arrange
        var eventId = "XID" + Guid.NewGuid();

        // Act
        var result = ActivateEventCommand.Create(eventId);

        // Assert
        Assert.True(result.IsFailure);
        Assert.Contains(Error.InvalidPrefix, result.Error.GetAllErrors());
    }
}