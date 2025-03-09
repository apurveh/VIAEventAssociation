using VIAEventAssociation.Core.Domain.Aggregates.Events;
using VIAEventAssociation.Core.Domain.Aggregates.Guests;
using VIAEventAssociation.Core.Domain.Common.Bases;
using VIAEventAssociation.Core.Tools.OperationResult;

namespace VIAEventAssociation.Core.Domain.Aggregates.Entities;

public abstract class Participation : Entity<ParticipationId>
{
    protected Participation(ParticipationId id, Event @event, Guest guest, ParticipationType participationType, ParticipationStatus participationStatus) : base(id)
    {
        Event = @event;
        Guest = guest;
        ParticipationStatus = participationStatus;
        ParticipationType = participationType;
    }
    
    public Event Event { get; }
    public Guest Guest { get; }
    public ParticipationStatus ParticipationStatus { get; protected set; }
    public ParticipationType ParticipationType { get; }

    public Result CancelParticipation()
    {
        if (Event.IsEventPast())
            return Error.EventIsPast;

        ParticipationStatus = ParticipationStatus.Canceled;
        return Result.Ok;
    }

    public override string ToString()
    {
        return
            $"{nameof(Participation)}: {nameof(Id)}: {Id}, {nameof(Guest)}: {Guest}, {nameof(Event)}: {Event}, {nameof(ParticipationType)}: {ParticipationType}";
    }
}