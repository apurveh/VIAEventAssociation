using Microsoft.AspNetCore.Mvc;
using ViaEventAssociation.Core.Application.Features.Commands.Guest;
using ViaEventAssociation.Core.Application.Features.Dispatcher;
using ViaEventAssociation.Presentation.WebAPI.EndPoints.Common;

namespace ViaEventAssociation.Presentation.WebAPI.EndPoints.Guests;

public class RegisterGuest(ICommandDispatcher dispatcher) :
    ApiEndpoint
    .WithRequest<RegisterGuest.RegisterGuestRequest>
    .WithoutResponse 
{
    [HttpPost("guests/register")]
    public override async Task<ActionResult> HandleAsync(RegisterGuestRequest request) {
        var cmd = RegisterGuestCommand.Create(request.FirstName, request.LastName, request.Email).Payload;
        var result = await dispatcher.DispatchAsync(cmd);
        return result.IsSuccess
            ? Ok()
            : BadRequest(result.Error.Message);
    }

    public record RegisterGuestRequest(string FirstName, string LastName, string Email);
}