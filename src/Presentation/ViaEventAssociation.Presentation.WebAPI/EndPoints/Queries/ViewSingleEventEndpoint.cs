using Microsoft.AspNetCore.Mvc;
using ViaEventAssociation.Core.QueryContracts.Queries;
using ViaEventAssociation.Core.QueryContracts.QueryDispatching;
using ViaEventAssociation.Core.Tools.ObjectMapper;
using ViaEventAssociation.Presentation.WebAPI.EndPoints.Common;

namespace ViaEventAssociation.Presentation.WebAPI.EndPoints.Queries;

public class ViewSingleEventEndpoint(IQueryDispatcher dispatcher, IMapper mapper)
    : ApiEndpoint
        .WithRequest<ViewSingleEventEndpoint.Request>
        .WithResponse<ViewSingleEventEndpoint.ViewSingleEventResponse> 
{
    [HttpGet("events/{Id}")]
    public override async Task<ActionResult<ViewSingleEventResponse>> HandleAsync([FromRoute] Request request) {
        var query = mapper.Map<ViewSingleEvent.Query>(request);
        var answer = await dispatcher.DispatchAsync(query);
        var response = mapper.Map<ViewSingleEventResponse>(answer);
        return Ok(response);
    }

    public record Request([FromRoute] string Id);

    public record ViewSingleEventResponse(
        string EventTitle,
        string EventDescription,
        string LocationName,
        DateTime EventStartTime,
        DateTime EventEndTime);
}