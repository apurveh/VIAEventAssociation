using ViaEventAssociation.Core.QueryContracts.Contracts;

namespace ViaEventAssociation.Core.QueryContracts.Queries;

public class ViewSingleEvent
{
    public record Query(string EventId) : IQuery<Answer>;

    public record Answer(
        string EventTitle,
        string EventDescription,
        string LocationName,
        string EventStartTime,
        string EventEndTime,
        string Visibility,
        int NumberOfAttendees,
        int MaxAttendees,
        List<Guest> Guests,
        int CurrentGuestPage,
        int TotalGuestPages);

    public record Guest(
        string ProfilePictureUrl,
        string Name);
}