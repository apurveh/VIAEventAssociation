using Microsoft.EntityFrameworkCore;
using ViaEventAssociation.Core.QueryContracts.Contracts;
using ViaEventAssociation.Core.QueryContracts.Queries;

namespace ViaEventAssociation.Infrastructure.EfcQueries.Queries;

public class ProfilePageQueryHandler(DbproductionContext context) : IQueryHandler<GuestProfilePage.Query, GuestProfilePage.Answer>
{
    public async Task<Result<GuestProfilePage.Answer>> HandleAsync(GuestProfilePage.Query query)
    {
        var guest = await context.Guests
            .Where(g => g.Id == query.UId)
            .Select(g => new {
                g.Id,
                Name = g.FirstName + " " + g.LastName,
                g.Email,
                UpcomingEvents = g.Participations
                    .Where(p => p.Event.EventStart >= DateTime.Now)
                    .Select(p => new GuestProfilePage.UpcomingEvent(
                        p.Event.Title,
                        p.Event.NumberOfGuests,
                        p.Event.EventStart.Value.ToString("yyyy-MM-dd"),
                        p.Event.EventStart.Value.ToString("HH:mm")
                    )).ToList(),
                PastEvents = g.Participations
                    .Where(p => p.Event.EventEnd < DateTime.Now)
                    .Select(p => new GuestProfilePage.PastEvent(p.Event.Title)).ToList(),
                PendingInvitationsCount = g.Participations.Count(p => p.ParticipationStatus == 1)
            })
            .FirstOrDefaultAsync();

        if (guest == null) {
            throw new InvalidOperationException("Guest not found");
        }

        return new GuestProfilePage.Answer(
            guest.Id,
            guest.Name,
            guest.Email,
            guest.UpcomingEvents.Count,
            guest.UpcomingEvents,
            guest.PastEvents,
            guest.PendingInvitationsCount
        );
    }
}