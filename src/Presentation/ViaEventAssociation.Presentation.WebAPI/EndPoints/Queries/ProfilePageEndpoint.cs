using Microsoft.AspNetCore.Mvc;
using ViaEventAssociation.Core.QueryContracts.Queries;
using ViaEventAssociation.Core.QueryContracts.QueryDispatching;
using ViaEventAssociation.Core.Tools.ObjectMapper;
using ViaEventAssociation.Presentation.WebAPI.EndPoints.Common;

namespace ViaEventAssociation.Presentation.WebAPI.EndPoints.Queries;

public class GuestPersonalPageEndpoint(IQueryDispatcher dispatcher, IMapper mapper) :
    ApiEndpoint
    .WithRequest<GuestPersonalPageEndpoint.Request>
    .WithResponse<GuestPersonalPageEndpoint.Response>
{
    [HttpGet("guests/{Id}")]
    public override async Task<ActionResult<Response>> HandleAsync([FromRoute] Request request)
    {
        var query = mapper.Map<GuestProfilePage.Query>(request);
        var answer = await dispatcher.DispatchAsync(query);
        var response = mapper.Map<Response>(answer);
        return Ok(response);
    }

    public record Request([FromRoute] string Id);

    public record Response(
        string GuestName,
        string GuestDescription,
        string GuestEmail,
        string GuestPhoneNumber);
}