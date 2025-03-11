using UnitTests.Features.EventTests;
using VIAEventAssociation.Core.Domain.Aggregates.Entities;
using VIAEventAssociation.Core.Domain.Aggregates.Events;

namespace UnitTests.Features.GuestTests.AcceptInvitation;

public class AcceptInvitation
{
    // UC14.S1
    [Theory]
    [InlineData(10)]
    [InlineData(50)]
    public void AcceptInvitation_WithValidEventAndGuestAndPendingInvitation_ShouldChangeInvitationStatus(int maxNumberOfGuests)
    {
        // Arrange
        var guest = GuestFactory
            .InitWithDefaultsValues()
            .Build();

        var @event = EventFactory
            .Init()
            .WithValidTimeInFuture()
            .WithStatus(EventStatus.Active)
            .WithVisibility(EventVisibility.Public)
            .WithMaxNumberOfGuests(maxNumberOfGuests)
            .WithValidConfirmedAttendees(maxNumberOfGuests - 1)
            .Build();

        @event.SendInvitation(guest);
        
        // Act
        var result = guest.AcceptInvitation(@event);
        
        // Assert
        Assert.True(result.IsSuccess);
        Assert.Equal(ParticipationStatus.Accepted, guest.Participations.First().ParticipationStatus);
    }
}