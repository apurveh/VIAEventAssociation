using Microsoft.AspNetCore.Mvc;
using ViaEventAssociation.Core.Application.Features.Commands.Event;
using ViaEventAssociation.Core.Application.Features.Dispatcher;
using ViaEventAssociation.Presentation.WebAPI.EndPoints.Common;

namespace ViaEventAssociation.Presentation.WebAPI.EndPoints.Events;

public class MakePrivate(ICommandDispatcher dispatcher) :
    ApiEndpoint
    .WithRequest<MakePrivate.MakePrivateRequest>
    .WithResponse<MakePrivate.MakePrivateResponse> 
{
    [HttpPost("events/make-private")]
    public override async Task<ActionResult<MakePrivateResponse>> HandleAsync(MakePrivateRequest request) {
        var cmd = MakePrivateCommand.Create(request.Id).Payload;
        var result = await dispatcher.DispatchAsync(cmd);
        return result.IsSuccess
            ? Ok(new MakePrivateResponse(cmd.Id.Value))
            : BadRequest(result.Error.Message);
    }

    public record MakePrivateRequest(string Id);

    public record MakePrivateResponse(string Id);
}