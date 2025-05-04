using Microsoft.AspNetCore.Mvc;
using ViaEventAssociation.Core.Application.Features.Dispatcher;
using ViaEventAssociation.Presentation.WebAPI.EndPoints.Common;

namespace ViaEventAssociation.Presentation.WebAPI.EndPoints.Events;

public class SetMaxGuest(ICommandDispatcher dispatcher) :
    ApiEndpoint
    .WithRequest<SetMaxGuest.SetMaxGuestRequest>
    .WithResponse<SetMaxGuest.SetMaxGuestResponse> 
{
    [HttpPost("events/set-max-guest")]
    public override async Task<ActionResult<SetMaxGuestResponse>> HandleAsync(SetMaxGuestRequest request) {
        var cmd = SetMaxGuestCommand.Create(request.Id, request.MaxGuest).Payload;
        var result = await dispatcher.DispatchAsync(cmd);
        return result.IsSuccess
            ? Ok(new SetMaxGuestResponse(cmd.Id.Value))
            : BadRequest(result.Error.Message);
    }

    public record SetMaxGuestRequest(string Id, string MaxGuest);

    public record SetMaxGuestResponse(string Id);
}