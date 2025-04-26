using ViaEventAssociation.Core.Domain.Agregates.Guests;

namespace ViaEventAssociation.Core.Domain.Entities.Invitation;

public class Invitation : Participation {
    private Invitation(ParticipationId participationId, Event @event, Guest guest,
        ParticipationStatus participationStatus) : base(participationId, @event, guest, ParticipationType.Invitation,
        participationStatus) { }

    private Invitation() : this(default!, default!, default!, default!) { } // Required by EF Core

    public static Result<Invitation> SendInvitation(Event @event, Guest guest) {
        var errors = new HashSet<Error>();

        var participationIdResult = ParticipationId.GenerateId()
            .OnFailure(error => errors.Add(error));

        var invitation = new Invitation(participationIdResult.Payload, @event, guest, ParticipationStatus.Pending);

        var response = guest.Serve(invitation)
            .OnFailure(error => errors.Add(error));

        if (errors.Any()) return Error.Add(errors);

        return response.IsSuccess ? invitation : response.Error;
    }

    public Result AcceptsInvitation() {
        var response = Event.ValidateInvitationResponse(this)
            .OnSuccess(() => ParticipationStatus = ParticipationStatus.Accepted);
        return response.IsSuccess ? Result.Ok : response.Error;
    }

    public Result DeclineInvitation() {
        var response = Event.ValidateInvitationDecline(this)
            .OnSuccess(() => ParticipationStatus = ParticipationStatus.Declined);
        return response.IsSuccess ? Result.Ok : response.Error;
    }
}