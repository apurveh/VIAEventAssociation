using ViaEventAssociation.Core.Domain.Agregates.Events;

namespace UnitTests.Features.Guest.RejectInvitation;

public class GuestRejectInvitation {
    //ID: 15 – Guest declines invitation
    // Given an active event, and a registered guest, and the event has an invitation for the guest, when the guest declines the invitation, then the invitation is registered as declined
    // ID:UC15.S1
    [Fact]
    public void GuestDeclinesInvitation_WithValidData_ShouldReturnSuccess() {
        //Arrange
        var guest = GuestFactory
            .InitWithDefaultsValues()
            .Build();

        var @event = EventFactory.Init()
            .WithValidTimeInFuture()
            .WithStatus(EventStatus.Active)
            .WithVisibility(EventVisibility.Public)
            .Build();

        @event.SendInvitation(guest);

        //Act
        var result = guest.DeclineInvitation(@event);

        //Assert
        Assert.True(result.IsSuccess);
    }

    // Given an active event, and a registered guest, and the event has an accepted invitation for the guest, when the guest declines the invitation, then the invitation is registered as declined
    // ID:UC15.S2
    [Fact]
    public void GuestDeclinesInvitation_WithAcceptedInvitation_ShouldReturnSuccess() {
        //Arrange
        var guest = GuestFactory
            .InitWithDefaultsValues()
            .Build();

        var @event = EventFactory.Init()
            .WithValidTimeInFuture()
            .WithStatus(EventStatus.Active)
            .WithVisibility(EventVisibility.Public)
            .Build();

        @event.SendInvitation(guest);
        guest.AcceptInvitation(@event);

        //Act
        var result = guest.DeclineInvitation(@event);

        //Assert
        Assert.True(result.IsSuccess);
    }

    // Given an active event, and a registered guest, and the event has no invitation for the guest, when the guest declines the invitation, then the request is rejected, with a message explaining the guest is not invited to the event
    // ID:UC15.F1
    [Fact]
    public void GuestDeclinesInvitation_WithNoInvitation_ShouldReturnFailure() {
        //Arrange
        var guest = GuestFactory
            .InitWithDefaultsValues()
            .Build();

        var @event = EventFactory.Init()
            .WithValidTimeInFuture()
            .WithStatus(EventStatus.Active)
            .WithVisibility(EventVisibility.Public)
            .Build();

        //Act
        var result = guest.DeclineInvitation(@event);

        //Assert
        Assert.True(result.IsFailure);
        Assert.Equal(Error.InvitationPendingOrAcceptedNotFound, result.Error);
    }

    // Given an active event, and a registered guest, and the event has a pending invitation for the guest, when the guest declines the invitation, then the request is rejected with a message explaining invitations to cancelled events cannot be declined.
    // ID:UC15.F2
    [Fact]
    public void GuestDeclinesInvitation_WithCancelledEvent_ShouldReturnFailure() {
        //Arrange
        var guest = GuestFactory
            .InitWithDefaultsValues()
            .Build();

        var @event = EventFactory.Init()
            .WithValidTimeInFuture()
            .WithStatus(EventStatus.Active)
            .Build();

        @event.SendInvitation(guest);
        @event.CancelEvent();

        //Act
        var result = guest.DeclineInvitation(@event);

        //Assert
        Assert.True(result.IsFailure);
        Assert.Equal(Error.EventStatusIsCanceledAndCannotRejectInvitation, result.Error);
    }

    // Given an active event, and a registered guest, and the event has a pending invitation for the guest, when the guest declines the invitation, then the request is rejected with a message explaining the event cannot yet be declined.
    // ID:UC15.F3
    [Fact]
    public void GuestDeclinesInvitation_WithPendingEvent_ShouldReturnFailure() {
        //Arrange
        var guest = GuestFactory
            .InitWithDefaultsValues()
            .Build();

        var @event = EventFactory.Init()
            .WithValidTimeInFuture()
            .WithStatus(EventStatus.Ready)
            .Build();

        @event.SendInvitation(guest);

        //Act
        var result = guest.DeclineInvitation(@event);

        //Assert
        Assert.True(result.IsFailure);
        Assert.Equal(Error.EventStatusIsReadyAndCannotRejectInvitation, result.Error);
    }
}