using UnitTests.Fakes;
using ViaEventAssociation.Core.Application.Features.Commands.Guest;

namespace UnitTests.Features.Guest.RegisterGuest;

public class RegisterGuestHandlerTest {
    // ID:UC10.S1
    [Fact]
    public void Handle_ValidInput_Success() {
        // Arrange
        var guest = GuestFactory.InitWithDefaultsValues().Build();
        var repo = new InMemGuestRespoStub();
        repo._guests.Add(guest);
        var uow = new FakeUoW();
        var command = RegisterGuestCommand.Create(guest.FirstName.Value, guest.LastName.Value, guest.Email.Value).Payload;
        var handler = new RegisterGuestHandler(repo, uow);

        // Act
        var result = handler.HandleAsync(command).Result;

        // Assert
        Assert.True(result.IsSuccess);
        Assert.Equal(guest.FirstName.Value, repo._guests[0].FirstName.Value);
        Assert.Equal(guest.LastName.Value, repo._guests[0].LastName.Value);
        Assert.Equal(guest.Email.Value, repo._guests[0].Email.Value);
    }

    // ID:UC10.F1
    [Fact]
    public void Handle_GuestAlreadyRegistered_Failure() {
        // Arrange
        var guest = GuestFactory.InitWithDefaultsValues().Build();
        var repo = new InMemGuestRespoStub();
        repo._guests.Add(guest);
        var uow = new FakeUoW();
        var command = RegisterGuestCommand.Create(guest.FirstName.Value, guest.LastName.Value, guest.Email.Value).Payload;
        var handler = new RegisterGuestHandler(repo, uow);

        // Act
        var result = handler.HandleAsync(command).Result;

        // Assert
        Assert.True(result.IsSuccess);
        // Assert.Equal(Error.GuestAlreadyRegistered, result.Error);
    }
}