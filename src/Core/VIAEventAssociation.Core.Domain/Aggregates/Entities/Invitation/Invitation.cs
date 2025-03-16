using VIAEventAssociation.Core.Domain.Aggregates.Events;
using VIAEventAssociation.Core.Domain.Aggregates.Guests;
using VIAEventAssociation.Core.Tools.OperationResult;

namespace VIAEventAssociation.Core.Domain.Aggregates.Entities.Invitation;

public class Invitation : Participation
{
    private Invitation(ParticipationId id, Event @event, Guest guest, ParticipationStatus participationStatus) : base(id, @event, guest, ParticipationType.Invitation, participationStatus) { }

    public static Result<Invitation> SendInvitation(Event @event, Guest guest)
    {
        var errors = new HashSet<Error>();
        
        var participationIdResult = ParticipationId.GenerateId()
            .OnFailure(error => errors.Add(error));

        var invitation = new Invitation(participationIdResult.Payload, @event, @guest, ParticipationStatus.Pending);

        var response = guest.Serve(invitation)
            .OnFailure(error => errors.Add(error));

        if (errors.Any())
            return Error.Add(errors);
        
        return response.IsSuccess ? invitation : response.Error;
    }
    
    public Result AcceptInvitation()
    {
        var response = Event.ValidateInvitationResponse(this)
            .OnSuccess(() => ParticipationStatus = ParticipationStatus.Accepted);
        return response.IsSuccess ? Result.Ok : response.Error;
    }
}