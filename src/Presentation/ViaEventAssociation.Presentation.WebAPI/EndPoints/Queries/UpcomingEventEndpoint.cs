using Microsoft.AspNetCore.Mvc;
using ViaEventAssociation.Core.QueryContracts.Queries;
using ViaEventAssociation.Core.QueryContracts.QueryDispatching;
using ViaEventAssociation.Core.Tools.ObjectMapper;
using ViaEventAssociation.Presentation.WebAPI.EndPoints.Common;

namespace ViaEventAssociation.Presentation.WebAPI.EndPoints.Queries;

public class UpcomingEventEndpoint(IQueryDispatcher dispatcher, IMapper mapper)
    : ApiEndpoint
        .WithRequest<UpcomingEventEndpoint.Request>
        .WithResponse<UpcomingEventEndpoint.UpcomingEventResponse> 
{
    [HttpGet("events/upcoming")]
    public override async Task<ActionResult<UpcomingEventResponse>> HandleAsync([FromRoute] Request request) {
        var query = mapper.Map<GuestProfilePage.Query>(request);
        var answer = await dispatcher.DispatchAsync(query);
        var response = mapper.Map<UpcomingEventResponse>(answer);
        return Ok(response);
    }

    public record Request();

    public record UpcomingEventResponse(
        string EventTitle,
        string EventDescription,
        string LocationName,
        DateTime EventStartTime,
        DateTime EventEndTime);
}