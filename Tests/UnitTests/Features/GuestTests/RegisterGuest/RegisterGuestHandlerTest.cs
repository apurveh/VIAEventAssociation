using UnitTests.Fakes;
using VIAEventAssociation.Core.Application.CommandDispatching.Commands.Guest;
using VIAEventAssociation.Core.Application.Features.Guest;
using VIAEventAssociation.Core.Domain.Aggregates.Guests;
using VIAEventAssociation.Core.Tools.OperationResult;
using Xunit;

namespace UnitTests.Features.GuestTests.RegisterGuest;

public class RegisterGuestHandlerTest
{
    // UC10.S1 - Successfully register a guest
    [Fact]
    public void Handle_ValidCommand_ShouldRegisterGuest()
    {
        // Arrange
        var guestRepo = new FakeGuestRepo();
        var uow = new FakeUoW();

        var command = RegisterGuestCommand.Create("GID" + Guid.NewGuid(), "John", "Doe", "jdoe@via.dk").Payload;
        var handler = new RegisterGuestHandler(guestRepo, uow);

        // Act
        var result = handler.HandleAsync(command).Result;

        // Assert
        Assert.True(result.IsSuccess);
        Assert.Single(guestRepo._guests); // Confirm guest added
    }

    // UC10.F5 - Email already exists
    [Fact]
    public void Handle_EmailAlreadyExists_ShouldReturnFailure()
    {
        // Arrange
        var guestRepo = new FakeGuestRepo();
        guestRepo._guests.Add(Guest.Create("Jane", "Smith", "jdoe@via.dk").Payload);

        var uow = new FakeUoW();
        var command = RegisterGuestCommand.Create("GID" + Guid.NewGuid(), "John", "Doe", "jdoe@via.dk").Payload;
        var handler = new RegisterGuestHandler(guestRepo, uow);

        // Act
        var result = handler.HandleAsync(command).Result;

        // Assert
        Assert.True(result.IsFailure);
        Assert.Equal(Error.EmailAlreadyExists, result.Error);
    }
}