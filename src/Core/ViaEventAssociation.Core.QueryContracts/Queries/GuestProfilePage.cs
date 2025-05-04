using ViaEventAssociation.Core.QueryContracts.Contracts;

namespace ViaEventAssociation.Core.QueryContracts.Queries;

public class GuestProfilePage
{
    public record Query(string UId) : IQuery<Answer>;

    public record Answer(
        string Name,
        string Email,
        int UpcomingEventsCount,
        List<UpcomingEvent> OverviewUpcomingEvents,
        List<PastEvent> OverviewPastEvents,
        int PendingInvitationsCount);

    public record UpcomingEvent(
        string Title,
        int NumberOfGuests,
        string Date,
        string StartTime);

    public record PastEvent(
        string Title);
}