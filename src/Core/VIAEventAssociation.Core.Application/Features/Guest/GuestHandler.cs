using VIAEventAssociation.Core.Application.CommandDispatching;
using VIAEventAssociation.Core.Application.CommandDispatching.Commands;
using VIAEventAssociation.Core.Domain.Aggregates.Guests;
using VIAEventAssociation.Core.Domain.Common.UnitOfWork;
using VIAEventAssociation.Core.Tools.OperationResult;

namespace VIAEventAssociation.Core.Application.Features.Guest;

public abstract class GuestHandler<TCommand>
    (IGuestRepository repository, IUnitOfWork unitOfWork) : ICommandHandler<TCommand, GuestId>
    where TCommand : ICommand<GuestId>
{
    protected readonly IGuestRepository Repository = repository;
    protected readonly IUnitOfWork UnitOfWork = unitOfWork;

    public async Task<Result> HandleAsync(TCommand command)
    {
        var result = await Repository.GetByIdAsync(command.Id);

        if (result.IsFailure)
            return result.Error;

        var guest = result?.Payload;

        if (guest is null)
            return Error.GuestNotFound;

        var action = await PerformAction(guest, command);
        if (action.IsFailure)
            return action.Error;

        await UnitOfWork.SaveChangesAsync();

        return Result.Success();
    }
    
    protected abstract Task<Result> PerformAction(Domain.Aggregates.Guests.Guest guest, TCommand command);
}