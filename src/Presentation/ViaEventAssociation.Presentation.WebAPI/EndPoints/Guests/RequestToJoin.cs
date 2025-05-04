using Microsoft.AspNetCore.Mvc;
using ViaEventAssociation.Core.Application.CommandDispatching.Commands.Guest;
using ViaEventAssociation.Core.Application.Features.Dispatcher;
using ViaEventAssociation.Presentation.WebAPI.EndPoints.Common;

namespace ViaEventAssociation.Presentation.WebAPI.EndPoints.Guests;

public class RequestToJoin(ICommandDispatcher dispatcher) :
    ApiEndpoint
    .WithRequest<RequestToJoin.RequestToJoinRequest>
    .WithoutResponse 
{
    [HttpPost("guests/request-to-join")]
    public override async Task<ActionResult> HandleAsync(RequestToJoinRequest request) {
        var cmd = RequestToJoinCommand.Create(request.EventId, request.GuestId).Payload;
        var result = await dispatcher.DispatchAsync(cmd);
        return result.IsSuccess
            ? Ok()
            : BadRequest(result.Error.Message);
    }

    public record RequestToJoinRequest(string EventId, string GuestId);
}