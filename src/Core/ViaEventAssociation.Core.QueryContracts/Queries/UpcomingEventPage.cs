using ViaEventAssociation.Core.QueryContracts.Contracts;

namespace ViaEventAssociation.Core.QueryContracts.Queries;

public class UpcomingEventPage
{
    public record Query(string TitleContains, int Page) : IQuery<Answer>;

    public record Answer(
        List<Event> Events,
        int Pages);

    public record Event(
        string Title,
        string Date,
        string StartTime,
        string EndTime,
        string Description,
        int NumberOfConfirmedGuests,
        int NumberOfTotalQuota,
        string EventStatus);
}