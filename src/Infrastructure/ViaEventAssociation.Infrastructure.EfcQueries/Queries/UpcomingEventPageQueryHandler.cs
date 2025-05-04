using Microsoft.EntityFrameworkCore;
using ViaEventAssociation.Core.QueryContracts.Contracts;
using ViaEventAssociation.Core.QueryContracts.Queries;

namespace ViaEventAssociation.Infrastructure.EfcQueries.Queries;

public class UpcomingEventPageQueryHandler(DbproductionContext context) : IQueryHandler<UpcomingEventPage.Query, UpcomingEventPage.Answer>
{
    private const int PageSize = 3;

    public async Task<UpcomingEventPage.Answer> HandleAsync(UpcomingEventPage.Query query)
    {
        // Filter events by title and ensure they are in the future, then order by start time
        var filteredEvents = context.Events
            .Where(e => e.Title.Contains(query.TitleContains) && e.EventStart > DateTime.Now)
            .OrderBy(e => e.EventStart);

        // Count the total events to calculate total pages
        var totalEventsCount = await filteredEvents.CountAsync();
        var totalPages = (int)Math.Ceiling(totalEventsCount / (double)PageSize);

        // Fetch the specific page of events
        var events = await filteredEvents
            .Skip(PageSize * (query.Page - 1))
            .Take(PageSize)
            .Select(e => new UpcomingEventPage.Event(
                e.Title,
                e.EventStart.Value.ToString("yyyy-MM-dd"),
                e.EventStart.Value.ToString("HH:mm"),
                e.EventEnd.Value.ToString("HH:mm"),
                e.Description.Length > 100 ? e.Description.Substring(0, 100) : e.Description,
                e.Participations.Count(p => p.ParticipationStatus == 1),
                e.NumberOfGuests,
                e.Visibility
            )).ToListAsync();

        // Construct the answer with the list of events and the total number of pages
        return new UpcomingEventPage.Answer(
            Events: events,
            Pages: totalPages
        );
    }
}