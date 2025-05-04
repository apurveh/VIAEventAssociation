using Microsoft.AspNetCore.Mvc;
using ViaEventAssociation.Core.Application.Features.Commands.Event;
using ViaEventAssociation.Core.Application.Features.Dispatcher;
using ViaEventAssociation.Presentation.WebAPI.EndPoints.Common;

namespace ViaEventAssociation.Presentation.WebAPI.EndPoints.Events;

public class UpdateTimeInterval(ICommandDispatcher dispatcher) :
    ApiEndpoint
    .WithRequest<UpdateTimeInterval.UpdateTimeIntervalRequest>
    .WithResponse<UpdateTimeInterval.UpdateTimeIntervalResponse> 
{
    [HttpPost("events/update-time-interval")]
    public override async Task<ActionResult<UpdateTimeIntervalResponse>> HandleAsync(UpdateTimeIntervalRequest request) 
    {
        var cmd = UpdateTimeIntervalCommand.Create(request.Id, request.Start, request.End).Payload;
        var result = await dispatcher.DispatchAsync(cmd);
        return result.IsSuccess
            ? Ok(new UpdateTimeIntervalResponse(cmd.Id.Value))
            : BadRequest(result.Error.Message);
    }

    public record UpdateTimeIntervalRequest(string Id, string Start, string End);

    public record UpdateTimeIntervalResponse(string Id);
}