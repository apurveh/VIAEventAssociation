using Microsoft.EntityFrameworkCore;
using ViaEventAssociation.Core.QueryContracts.Contracts;
using ViaEventAssociation.Core.QueryContracts.Queries;

namespace ViaEventAssociation.Infrastructure.EfcQueries.Queries;

public class ViewUnpublishedEventsQueryHandler(DbproductionContext context) : IQueryHandler<ViewUnpublishedEvents.Query, ViewUnpublishedEvents.Answer>
{
    public async Task<Result<ViewUnpublishedEvents.Answer>> HandleAsync(ViewUnpublishedEvents.Query query)
    {
        // Use IQueryable to build up the query.
        IQueryable<Event> draftQuery = context.Events.Where(e => e.Status == "draft");
        IQueryable<Event> readyQuery = context.Events.Where(e => e.Status == "ready");
        IQueryable<Event> cancelledQuery = context.Events.Where(e => e.Status == "cancelled");

        // Execute the queries and convert to lists.
        var draftEvents = await draftQuery
            .Select(e => new ViewUnpublishedEvents.EventItem(e.Id, e.Title))
            .ToListAsync();

        var readyEvents = await readyQuery
            .Select(e => new ViewUnpublishedEvents.EventItem(e.Id, e.Title))
            .ToListAsync();

        var cancelledEvents = await cancelledQuery
            .Select(e => new ViewUnpublishedEvents.EventItem(e.Id, e.Title))
            .ToListAsync();

        return new ViewUnpublishedEvents.Answer(
            Drafts: draftEvents,
            Ready: readyEvents,
            Cancelled: cancelledEvents
        );
    }
}