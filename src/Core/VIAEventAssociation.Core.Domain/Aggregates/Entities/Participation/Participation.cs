using ViaEventAssociation.Core.Domain.Agregates.Guests;
using ViaEventAssociation.Core.Domain.Common.Bases;
using ViaEventAssociation.Core.Domain.Entities.Invitation;

namespace ViaEventAssociation.Core.Domain.Entities;

public abstract class Participation : Entity<ParticipationId> {
    protected Participation(ParticipationId participationId, Event @event, Guest guest,
        ParticipationType participationType, ParticipationStatus participationStatus) : base(participationId) {
        Event = @event;
        Guest = guest;
        ParticipationType = participationType;
        ParticipationStatus = participationStatus;
    }

    public Guest Guest { get; private set; }
    public Event Event { get; private set; }
    public ParticipationType ParticipationType { get; private set; }
    public ParticipationStatus ParticipationStatus { get; protected set; }


    public Result CancelParticipation() {
        if (Event.isEventPast())
            return Error.EventIsPast;

        ParticipationStatus = ParticipationStatus.Canceled;
        return Result.Ok;
    }


    public override string ToString() {
        return
            $"{nameof(Participation)}: {nameof(Id)}: {Id}, {nameof(Guest)}: {Guest}, {nameof(Event)}: {Event}, {nameof(ParticipationType)}: {ParticipationType}";
    }
}