using Microsoft.AspNetCore.Mvc;
using ViaEventAssociation.Core.Application.CommandDispatching.Commands.Guest;
using ViaEventAssociation.Core.Application.Features.Dispatcher;
using ViaEventAssociation.Presentation.WebAPI.EndPoints.Common;

namespace ViaEventAssociation.Presentation.WebAPI.EndPoints.Guests;

public class AcceptInvitation(ICommandDispatcher dispatcher) :
    ApiEndpoint
    .WithRequest<AcceptInvitation.AcceptInvitationRequest>
    .WithResponse<AcceptInvitation.AcceptInvitationResponse>
{
    [HttpPost("guests/accept-invitation")]
    public override async Task<ActionResult<AcceptInvitationResponse>> HandleAsync(AcceptInvitationRequest request)
    {
        var command = AcceptsInvitationCommand.Create(request.EventId, request.GuestId).Payload;
        var result = await dispatcher.DispatchAsync(command);
        return result.IsSuccess
            ? Ok(new AcceptInvitationResponse(command.Id.Value))
            : BadRequest(result.Error.Message);
    }
    
    public record AcceptInvitationRequest(string EventId, string GuestId);

    public record AcceptInvitationResponse(string Id);
}