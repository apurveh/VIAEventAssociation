using Microsoft.EntityFrameworkCore;
using VIAEventAssociation.Core.Domain.Aggregates.Entities;
using VIAEventAssociation.Core.Domain.Aggregates.Entities.Invitation;
using VIAEventAssociation.Core.Domain.Aggregates.Events;
using VIAEventAssociation.Core.Domain.Aggregates.Guests;
using VIAEventAssociation.Core.Domain.Common;
using VIAEventAssociation.Core.Tools.OperationResult;

namespace VIAEventAssociation.Infrastructure.SqliteDmPersistence.Repositories
{
    public class EventRepository : Repository<Event, EventId>, IEventRepository
    {
        public EventRepository(SqliteDmContext context) : base(context) { }

        public async Task<IEnumerable<Event>> GetPublicActiveEventsAsync()
        {
            return await _dbSet
                .Where(e => e.EventVisibility == EventVisibility.Public && e.EventStatus == EventStatus.Active)
                .ToListAsync();
        }

        public async Task<Event?> GetEventWithParticipantsAsync(EventId id)
        {
            return await _dbSet
                .Include(e => e.Participations)
                .FirstOrDefaultAsync(e => e.Id == id);
        }

        public async Task<bool> IsGuestAlreadyParticipating(EventId eventId, GuestId guestId)
        {
            var evt = await _dbSet
                .Include(e => e.Participations)
                .FirstOrDefaultAsync(e => e.Id == eventId);

            return evt?.Participations.Any(p =>
                p.Guest.Id == guestId && p.ParticipationStatus == ParticipationStatus.Accepted) ?? false;
        }

        public async Task<bool> IsGuestAlreadyInvited(EventId eventId, GuestId guestId)
        {
            var evt = await _dbSet
                .Include(e => e.Participations)
                .FirstOrDefaultAsync(e => e.Id == eventId);

            return evt?.Participations.Any(p =>
                p.Guest.Id == guestId &&
                p is Invitation &&
                p.ParticipationStatus == ParticipationStatus.Pending) ?? false;
        }

        public async Task<int> GetConfirmedParticipantCountAsync(EventId eventId)
        {
            var evt = await _dbSet
                .Include(e => e.Participations)
                .FirstOrDefaultAsync(e => e.Id == eventId);

            return evt?.Participations.Count(p => p.ParticipationStatus == ParticipationStatus.Accepted) ?? 0;
        }

        public async Task<IEnumerable<Event>> GetUpcomingEventsAsync()
        {
            return await _dbSet
                .Where(e => !e.IsEventPast())
                .ToListAsync();
        }
    }
}
