using VIAEventAssociation.Core.Application.CommandDispatching;
using VIAEventAssociation.Core.Application.CommandDispatching.Commands.Guest;
using VIAEventAssociation.Core.Domain.Aggregates.Guests;
using VIAEventAssociation.Core.Domain.Common.UnitOfWork;
using VIAEventAssociation.Core.Tools.OperationResult;

namespace VIAEventAssociation.Core.Application.Features.Guest;

public class RegisterGuestHandler(
    IGuestRepository guestRepository,
    IUnitOfWork unitOfWork
) : ICommandHandler<RegisterGuestCommand, GuestId>
{
    public async Task<Result> HandleAsync(RegisterGuestCommand command)
    {
        // Check for duplicate email
        var emailExists = await guestRepository.EmailExists(command.Email);
        if (emailExists)
            return Error.EmailAlreadyExists;

        // Create the Guest using domain logic
        var result = Domain.Aggregates.Guests.Guest.Create(command.FirstName, command.LastName, command.Email);
        if (result.IsFailure)
            return result.Error;

        var guest = result.Payload;

        // Persist the guest
        await guestRepository.AddAsync(guest);
        await unitOfWork.SaveChangesAsync();

        return Result.Success();
    }
}