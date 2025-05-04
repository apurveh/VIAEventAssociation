using Microsoft.AspNetCore.Mvc;
using ViaEventAssociation.Core.Application.Features.Dispatcher;
using ViaEventAssociation.Presentation.WebAPI.EndPoints.Common;

namespace ViaEventAssociation.Presentation.WebAPI.EndPoints.Events;

public class SetReadyEvent(ICommandDispatcher dispatcher) :
    ApiEndpoint
    .WithRequest<SetReadyEvent.SetReadyEventRequest>
    .WithResponse<SetReadyEvent.SetReadyEventResponse> 
{
    [HttpPost("events/set-ready")]
    public override async Task<ActionResult<SetReadyEventResponse>> HandleAsync(SetReadyEventRequest request) {
        var cmd = SetReadyEventCommand.Create(request.Id).Payload;
        var result = await dispatcher.DispatchAsync(cmd);
        return result.IsSuccess
            ? Ok(new SetReadyEventResponse(cmd.Id.Value))
            : BadRequest(result.Error.Message);
    }

    public record SetReadyEventRequest(string Id);

    public record SetReadyEventResponse(string Id);
}