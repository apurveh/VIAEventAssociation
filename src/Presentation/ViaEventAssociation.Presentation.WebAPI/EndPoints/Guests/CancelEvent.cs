using Microsoft.AspNetCore.Mvc;
using ViaEventAssociation.Core.Application.CommandDispatching.Commands.Guest;
using ViaEventAssociation.Core.Application.Features.Dispatcher;
using ViaEventAssociation.Presentation.WebAPI.EndPoints.Common;

namespace ViaEventAssociation.Presentation.WebAPI.EndPoints.Guests;

public class CancelEvent(ICommandDispatcher dispatcher) :
    ApiEndpoint
    .WithRequest<CancelEvent.CancelEventRequest>
    .WithoutResponse 
{
    [HttpPost("guests/cancel-event")]
    public override async Task<ActionResult> HandleAsync(CancelEventRequest request) {
        var cmd = CancelEventParticipationCommand.Create(request.EventId, request.GuestId).Payload;
        var result = await dispatcher.DispatchAsync(cmd);
        return result.IsSuccess
            ? Ok()
            : BadRequest(result.Error.Message);
    }

    public record CancelEventRequest(string EventId, string GuestId);
}