using VIAEventAssociation.Core.Domain.Aggregates.Guests;
using VIAEventAssociation.Core.Domain.Contracts;
using VIAEventAssociation.Core.Tools.OperationResult;

namespace VIAEventAssociation.Core.Domain.Services;

public class GuestService
{
    private readonly IGuestService _guestService;

    public GuestService(IGuestService guestService)
    {
        _guestService = guestService;
    }

    public Result<Guest> RegisterGuest(string firstName, string lastName, string email)
    {
        if (_guestService.EmailExists(email))
        {
            return Error.EmailAlreadyExists;
        }

        var guestResult = Guest.Create(firstName, lastName, email);
    
        if (guestResult.IsFailure)
        {
            return guestResult.Error;
        }

        _guestService.AddGuest(guestResult.Payload);
        return guestResult.Payload;
    }

}
