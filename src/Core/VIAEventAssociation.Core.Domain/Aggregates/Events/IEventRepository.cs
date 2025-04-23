using VIAEventAssociation.Core.Domain.Aggregates.Guests;
using VIAEventAssociation.Core.Domain.Common;

namespace VIAEventAssociation.Core.Domain.Aggregates.Events;

public interface IEventRepository : IRepository<Event, EventId>
{
    Task<IEnumerable<Event>> GetPublicActiveEventsAsync();

    Task<Event?> GetEventWithParticipantsAsync(EventId id);

    Task<bool> IsGuestAlreadyParticipating(EventId eventId, GuestId guestId);

    Task<bool> IsGuestAlreadyInvited(EventId eventId, GuestId guestId);

    Task<int> GetConfirmedParticipantCountAsync(EventId eventId);

    Task<IEnumerable<Event>> GetUpcomingEventsAsync();
}
