using Microsoft.AspNetCore.Mvc;
using ViaEventAssociation.Core.Application.Features.Commands.Event;
using ViaEventAssociation.Core.Application.Features.Dispatcher;
using ViaEventAssociation.Presentation.WebAPI.EndPoints.Common;

namespace ViaEventAssociation.Presentation.WebAPI.EndPoints.Events;

public class MakePublic(ICommandDispatcher dispatcher) :
    ApiEndpoint
    .WithRequest<MakePublic.MakePublicRequest>
    .WithResponse<MakePublic.MakePublicResponse> 
{
    [HttpPost("events/make-public")]
    public override async Task<ActionResult<MakePublicResponse>> HandleAsync(MakePublicRequest request) {
        var cmd = MakePublicCommand.Create(request.Id).Payload;
        var result = await dispatcher.DispatchAsync(cmd);
        return result.IsSuccess
            ? Ok(new MakePublicResponse(cmd.Id.Value))
            : BadRequest(result.Error.Message);
    }

    public record MakePublicRequest(string Id);

    public record MakePublicResponse(string Id);
}