using VIAEventAssociation.Core.Domain.Aggregates.Guests;
using VIAEventAssociation.Core.Domain.Contracts;
using VIAEventAssociation.Core.Tools.OperationResult;

namespace VIAEventAssociation.Core.Domain.Services;

public class GuestService
{
    private readonly IGuestRepository _guestRepository;

    public GuestService(IGuestRepository guestRepository)
    {
        _guestRepository = guestRepository;
    }

    public Result<Guest> RegisterGuest(string firstName, string lastName, string email)
    {
        if (_guestRepository.EmailExists(email))
        {
            return Error.EmailAlreadyExists;
        }

        var guestResult = Guest.Create(firstName, lastName, email);
    
        if (guestResult.IsFailure)
        {
            return guestResult.Error;
        }

        _guestRepository.AddGuest(guestResult.Payload);
        return guestResult.Payload;
    }

}
