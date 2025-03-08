using VIAEventAssociation.Core.Domain.Aggregates.Events;
using VIAEventAssociation.Core.Domain.Aggregates.Guests;
using VIAEventAssociation.Core.Tools.OperationResult;

namespace VIAEventAssociation.Core.Domain.Aggregates.Entities;

public class JoinRequest : Participation
{
    private JoinRequest(ParticipationId participationId, Event @event, Guest guest, string? reason,
        ParticipationStatus participationStatus) : base(participationId, @event, guest, ParticipationType.JoinRequest,
        participationStatus)
    {
        Reason = reason;
    }
    
    public string? Reason { get; private set; }

    public static Result<JoinRequest> SendJoinRequest(Event @event, Guest guest, string reason = null!)
    {
        var errors = new HashSet<Error>();

        var participationIdResult = ParticipationId.GenerateId()
            .OnFailure(error => errors.Add(error));

        var participation = new JoinRequest(participationIdResult.Payload, @event, guest, reason,
            ParticipationStatus.Pending);
        
        @event.RequestToJoin(participation)
            .OnFailure(error => errors.Add(error))
            .OnSuccess(status => participation.ParticipationStatus = status);

        if (errors.Any())
            return Error.Add(errors);

        return participation.ParticipationStatus == ParticipationStatus.Declined ? Error.EventIsPrivate : participation;
    }
}