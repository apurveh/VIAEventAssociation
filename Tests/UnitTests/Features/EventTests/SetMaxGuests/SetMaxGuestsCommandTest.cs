using VIAEventAssociation.Core.Application.CommandDispatching.Commands.Event;
using VIAEventAssociation.Core.Domain.Common.Values;
using VIAEventAssociation.Core.Tools.OperationResult;

namespace UnitTests.Features.EventTests.SetMaxGuests;

public class SetMaxGuestsCommandTest
{
    // UC7 Sunny
    [Fact]
    public void Create_WithValidInput_ShouldReturnSuccess()
    {
        // Arrange
        var eventId = "EID" + Guid.NewGuid();
        var guestCount = 25;

        // Act
        var result = SetMaxGuestsCommand.Create(eventId, guestCount);

        // Assert
        Assert.True(result.IsSuccess);
        Assert.Equal(eventId, result.Payload.Id.Value.ToString());
        Assert.Equal(guestCount, result.Payload.MaxGuests);
    }

    // UC7 Rainy
    [Fact]
    public void Create_WithTooFewGuests_ShouldReturnFailure()
    {
        // Arrange
        var eventId = "EID" + Guid.NewGuid();
        var guestCount = 1;

        // Act
        var result = SetMaxGuestsCommand.Create(eventId, guestCount);

        // Assert
        Assert.False(result.IsSuccess);
        Assert.Contains(Error.TooFewGuests(CONST.MIN_NUMBER_OF_GUESTS), result.Error.GetAllErrors());
    }
}