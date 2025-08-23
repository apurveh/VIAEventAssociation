using Microsoft.EntityFrameworkCore;
using ViaEventAssociation.Core.QueryContracts.Contracts;
using ViaEventAssociation.Core.QueryContracts.Queries;

namespace ViaEventAssociation.Infrastructure.EfcQueries.Queries;

public class ViewSingleEventQueryHandler(DbproductionContext context) : IQueryHandler<ViewSingleEvent.Query, ViewSingleEvent.Answer>
{
    private const int GuestsPerPage = 6;

    public async Task<Result<ViewSingleEvent.Answer>> HandleAsync(ViewSingleEvent.Query query)
    {
        var eventWithAttendees = await context.Events
            .Where(e => e.Id == query.EventId)
            .Select(e => new {
                e.Title,
                e.Description,
                e.EventStart,
                e.EventEnd,
                e.NumberOfGuests,
                e.Visibility,
                Guests = e.Participations
                    .Select(p => new ViewSingleEvent.Guest(
                        "Unknown",
                        p.Guest.FirstName + " " + p.Guest.LastName
                    ))
                    .ToList(),
                MaxAttendees = e.NumberOfGuests
            })
            .FirstOrDefaultAsync();

        if (eventWithAttendees == null) {
            throw new InvalidOperationException("Event not found");
        }

        return new ViewSingleEvent.Answer(
            EventTitle: eventWithAttendees.Title,
            EventDescription: eventWithAttendees.Description,
            LocationName: "Unknown",
            EventStartTime: eventWithAttendees.EventStart.Value.ToString("HH:mm"),
            EventEndTime: eventWithAttendees.EventEnd.Value.ToString("HH:mm"),
            Visibility: eventWithAttendees.Visibility,
            NumberOfAttendees: eventWithAttendees.Guests.Count,
            MaxAttendees: eventWithAttendees.MaxAttendees,
            Guests: eventWithAttendees.Guests,
            CurrentGuestPage: GuestsPerPage,
            TotalGuestPages: (int)Math.Ceiling((double)eventWithAttendees.Guests.Count / GuestsPerPage)
        );
    }
}