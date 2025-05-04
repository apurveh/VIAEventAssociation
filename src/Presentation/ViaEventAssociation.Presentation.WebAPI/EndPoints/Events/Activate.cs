using Microsoft.AspNetCore.Mvc;
using ViaEventAssociation.Core.Application.Features.Commands.Event;
using ViaEventAssociation.Core.Application.Features.Dispatcher;
using ViaEventAssociation.Presentation.WebAPI.EndPoints.Common;

namespace ViaEventAssociation.Presentation.WebAPI.EndPoints.Events;

public class Activate(ICommandDispatcher dispatcher) : 
    ApiEndpoint
    .WithRequest<Activate.ActivateEventRequest>
    .WithResponse<Activate.ActivateEventResponse>
{
    [HttpPost("events/activate")]
    public override async Task<ActionResult<ActivateEventResponse>> HandleAsync(ActivateEventRequest request)
    {
        var command = ActivateEventCommand.Create(request.Id).Payload;
        var result = await dispatcher.DispatchAsync(command);
        return result.IsSuccess
            ? Ok(new ActivateEventResponse(command.Id.Value))
            : BadRequest(result.Error.Message);
    }

    public record ActivateEventRequest(string Id);

    public record ActivateEventResponse(string Id);
}