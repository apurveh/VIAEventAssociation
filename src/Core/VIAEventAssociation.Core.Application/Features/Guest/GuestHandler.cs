using ViaEventAssociation.Core.Application.CommandDispatching;
using ViaEventAssociation.Core.Application.CommandDispatching.Commands;
using ViaEventAssociation.Core.Domain;
using ViaEventAssociation.Core.Domain.Agregates.Guests;

namespace ViaEventAssociation.Core.Application.Features.Guest;

public abstract class GuestHandler(IGuestRepository repository, IUnitOfWork unitOfWork) : ICommandHandler<Command<GuestId>> {
    public async Task<Result> HandleAsync(Command<GuestId> command) {
        var result = await repository.GetByIdAsync(command.Id);

        if (result.IsFailure)
            return result.Error;

        var guest = result?.Payload;

        if (guest is null)
            return Error.GuestIsNotFound;

        var action = await PerformAction(guest, command);
        if (action.IsFailure)
            return action.Error;

        await unitOfWork.SaveChangesAsync();

        return Result.Success();
    }

    protected abstract Task<Result> PerformAction(Domain.Agregates.Guests.Guest guest, Command<GuestId> command);
}