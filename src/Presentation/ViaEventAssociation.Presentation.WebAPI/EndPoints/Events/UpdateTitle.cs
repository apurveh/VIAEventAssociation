using Microsoft.AspNetCore.Mvc;
using ViaEventAssociation.Core.Application.Features.Commands.Event;
using ViaEventAssociation.Core.Application.Features.Dispatcher;
using ViaEventAssociation.Presentation.WebAPI.EndPoints.Common;

namespace ViaEventAssociation.Presentation.WebAPI.EndPoints.Events;

public class UpdateTitle(ICommandDispatcher dispatcher)
    : ApiEndpoint
        .WithRequest<UpdateTitle.UpdateTitleRequest>
        .WithoutResponse 
{
    [HttpPost("events/{Id}/update-title")]
    public override async Task<ActionResult> HandleAsync([FromRoute] UpdateTitleRequest request) {
        var cmdResult = UpdateTitleCommand.Create(request.Id, request.RequestBody.Title);
        if (cmdResult.IsFailure) {
            return BadRequest(cmdResult.Error);
        }

        var result = await dispatcher.DispatchAsync(cmdResult.Payload);
        return result.IsSuccess
            ? Ok()
            : BadRequest(result.Error);
    }

    public class UpdateTitleRequest {
        [FromRoute] public string Id { get; set; }
        [FromBody] public Body RequestBody { get; set; }

        public record Body(string Title);
    }
}