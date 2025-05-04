using Microsoft.AspNetCore.Mvc;
using ViaEventAssociation.Core.QueryContracts.Queries;
using ViaEventAssociation.Core.QueryContracts.QueryDispatching;
using ViaEventAssociation.Core.Tools.ObjectMapper;
using ViaEventAssociation.Presentation.WebAPI.EndPoints.Common;

namespace ViaEventAssociation.Presentation.WebAPI.EndPoints.Queries;

public class ViewUnpublishEventEndpoint(IQueryDispatcher dispatcher, IMapper mapper)
    : ApiEndpoint
        .WithRequest<ViewUnpublishEventEndpoint.Request>
        .WithResponse<ViewUnpublishEventEndpoint.ViewUnpublishEvent> 
{
    [HttpGet("events/unpublish/{Id}")]
    public override async Task<ActionResult<ViewUnpublishEvent>> HandleAsync([FromRoute] Request request) {
        var query = mapper.Map<ViewUnpublishedEvents.Query>(request);
        var answer = await dispatcher.DispatchAsync(query);
        var response = mapper.Map<ViewUnpublishEvent>(answer);
        return Ok(response);
    }

    public record Request([FromRoute] string Id);

    public record ViewUnpublishEvent(
        string EventTitle,
        string EventDescription,
        string LocationName,
        DateTime EventStartTime,
        DateTime EventEndTime);
}