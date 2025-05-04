using Microsoft.AspNetCore.Mvc;
using ViaEventAssociation.Core.Application.CommandDispatching.Commands.Guest;
using ViaEventAssociation.Core.Application.Features.Dispatcher;
using ViaEventAssociation.Presentation.WebAPI.EndPoints.Common;

namespace ViaEventAssociation.Presentation.WebAPI.EndPoints.Guests;

public class RejectInvitation(ICommandDispatcher dispatcher) :
    ApiEndpoint
    .WithRequest<RejectInvitation.RejectInvitationRequest>
    .WithoutResponse 
{
    [HttpPost("guests/reject")]
    public override async Task<ActionResult> HandleAsync(RejectInvitationRequest request) {
        var cmd = RejectInvitationCommand.Create(request.EventId, request.GuestId).Payload;
        var result = await dispatcher.DispatchAsync(cmd);
        return result.IsSuccess
            ? Ok()
            : BadRequest(result.Error.Message);
    }

    public record RejectInvitationRequest(string EventId, string GuestId);
}