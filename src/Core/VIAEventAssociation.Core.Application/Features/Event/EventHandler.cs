using ViaEventAssociation.Core.Application.CommandDispatching;
using ViaEventAssociation.Core.Application.CommandDispatching.Commands;
using ViaEventAssociation.Core.Domain;
using ViaEventAssociation.Core.Domain.Aggregates.Events;
using ViaEventAssociation.Core.Domain.Agregates.Events;

namespace ViaEventAssociation.Core.Application.Features.Event;

public abstract class EventHandler(IEventRepository repository, IUnitOfWork unitOfWork) : ICommandHandler<Command<EventId>> {
    private Result<global::Event> @event;

    public async Task<Result> HandleAsync(Command<EventId> command) {
        var result = await repository.GetByIdAsync(command.Id);

        if (result.IsFailure)
            return result.Error;

        var @event = result.Payload;

        if (@event is null)
            return Error.EventIsNotFound;

        var action = await PerformAction(@event, command);
        if (action.IsFailure)
            return action.Error;

        await unitOfWork.SaveChangesAsync();

        return Result.Success();
    }

    protected abstract Task<Result> PerformAction(global::Event eve, Command<EventId> command);
}