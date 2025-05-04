using Microsoft.AspNetCore.Mvc;
using ViaEventAssociation.Core.Application.Features.Commands.Event;
using ViaEventAssociation.Core.Application.Features.Dispatcher;
using ViaEventAssociation.Presentation.WebAPI.EndPoints.Common;

namespace ViaEventAssociation.Presentation.WebAPI.EndPoints.Events;

public class InviteGuest(ICommandDispatcher dispatcher) :
    ApiEndpoint
    .WithRequest<InviteGuest.InviteGuestRequest>
    .WithResponse<InviteGuest.InviteGuestResponse> 
{
    [HttpPost("events/invite")]
    public override async Task<ActionResult<InviteGuestResponse>> HandleAsync(InviteGuestRequest request) {
        var cmd = InviteGuestCommand.Create(request.EventId, request.GuestId).Payload;
        var result = await dispatcher.DispatchAsync(cmd);
        return result.IsSuccess
            ? Ok(new InviteGuestResponse(cmd.Id.Value, cmd.GuestId.Value))
            : BadRequest(result.Error.Message);
    }

    public record InviteGuestRequest(string EventId, string GuestId);

    public record InviteGuestResponse(string EventId, string GuestId);
}