using ViaEventAssociation.Core.QueryContracts.Contracts;

namespace ViaEventAssociation.Core.QueryContracts.Queries;

public class ViewUnpublishedEvents
{
    public record Query : IQuery<Answer>;

    public record Answer(
        List<EventItem> Drafts,
        List<EventItem> Ready,
        List<EventItem> Cancelled);

    public record EventItem(
        string EventId,
        string Title);
}