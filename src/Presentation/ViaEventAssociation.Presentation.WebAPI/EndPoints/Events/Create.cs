using Microsoft.AspNetCore.Mvc;
using ViaEventAssociation.Core.Application.Features.Commands.Event;
using ViaEventAssociation.Core.Application.Features.Dispatcher;
using ViaEventAssociation.Presentation.WebAPI.EndPoints.Common;

namespace ViaEventAssociation.Presentation.WebAPI.EndPoints.Events;

public class Create(ICommandDispatcher dispatcher) :
    ApiEndpoint
    .WithoutRequest
    .WithResponse<Create.CreateEventResponse>
{
    [HttpPost("events/create")]
    public override async Task<ActionResult<CreateEventResponse>> HandleAsync()
    {
        var command = CreateEventCommand.Create().Payload;
        var result = await dispatcher.DispatchAsync(command);
        return result.IsSuccess
            ? Ok(new CreateEventResponse(command.Id.Value))
            : BadRequest(result.Error.Message);
    }
    
    public record CreateEventResponse(string Id);
}