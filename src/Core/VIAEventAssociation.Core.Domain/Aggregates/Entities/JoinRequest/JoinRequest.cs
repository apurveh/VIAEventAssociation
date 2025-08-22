using ViaEventAssociation.Core.Domain.Aggregates.Events;
using ViaEventAssociation.Core.Domain.Aggregates.Guests;
using ViaEventAssociation.Core.Domain.Entities;
using ViaEventAssociation.Core.Domain.Entities.Invitation;

namespace ViaEventAssociation.Core.Domain.Aggregates.Entities.JoinRequest;

public class JoinRequest : Participation.Participation {
    private JoinRequest(ParticipationId participationId, Event @event, Guest guest, string? reason,
        ParticipationStatus participationStatus) : base(participationId, @event, guest, ParticipationType.JoinRequest,
        participationStatus) {
        Reason = reason;
    }

    private JoinRequest() : this(default!, default!, default!, default!, default!) { } // Required by EF Core

    internal string? Reason { get; private set; }

    public static Result<JoinRequest> SendJoinRequest(Event @event, Guest guest, string reason = null) {
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

        return participation.ParticipationStatus == ParticipationStatus.Declined
            ? Error.EventIsPrivate
            : participation;
    }

    public Result AcceptJoinRequest() {
        ParticipationStatus = ParticipationStatus.Accepted;
        return Result.Ok;
    }

    public Result DeclineJoinRequest() {
        ParticipationStatus = ParticipationStatus.Declined;
        return Result.Ok;
    }
}