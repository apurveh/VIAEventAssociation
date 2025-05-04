using Microsoft.AspNetCore.Mvc;
using ViaEventAssociation.Core.Application.Features.Commands.Event;
using ViaEventAssociation.Core.Application.Features.Dispatcher;
using ViaEventAssociation.Presentation.WebAPI.EndPoints.Common;

namespace ViaEventAssociation.Presentation.WebAPI.EndPoints.Events;

public class EventDescription(ICommandDispatcher dispatcher) :
    ApiEndpoint
    .WithRequest<EventDescription.EventDescriptionRequest>
    .WithResponse<EventDescription.EventDescriptionResponse> 
{
    [HttpPost("events/description")]
    public override async Task<ActionResult<EventDescriptionResponse>> HandleAsync(EventDescriptionRequest request) {
        var cmd = UpdateDescriptionCommand.Create(request.Id, request.Description).Payload;
        var result = await dispatcher.DispatchAsync(cmd);
        return result.IsSuccess
            ? Ok(new EventDescriptionResponse(cmd.Id.Value))
            : BadRequest(result.Error.Message);
    }

    public record EventDescriptionRequest(string Id, string Description);

    public record EventDescriptionResponse(string Id);
}