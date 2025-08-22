using ViaEventAssociation.Core.Application.CommandDispatching;
using ViaEventAssociation.Core.Application.CommandDispatching.Commands;
using ViaEventAssociation.Core.Application.Features.Commands.Guest;
using ViaEventAssociation.Core.Domain;
using ViaEventAssociation.Core.Domain.Agregates.Guests;
using ViaEventAssociation.Core.Domain.Contracts;

public class RegisterGuestHandler(IGuestRepository guestRepository, IUnitOfWork unitOfWork) : ICommandHandler<Command<GuestId>>, ICommandHandler<RegisterGuestCommand>
{
    public async Task<Result> HandleAsync(Command<GuestId> command) {
        var result = await guestRepository.GetByIdAsync(command.Id);

        if (result.IsSuccess)
            return Error.GuestAlreadyRegistered;

        if (command is RegisterGuestCommand registerGuestCommand) {
            var guest = Guest.Create(registerGuestCommand.FirstName, registerGuestCommand.LastName, registerGuestCommand.Email);

            if (guest.IsFailure)
                return guest.Error;
            

            await guestRepository.AddAsync(guest.Payload);
            await unitOfWork.SaveChangesAsync();

            return Result.Success();
        }

        return Error.InvalidCommand;
    }
} 