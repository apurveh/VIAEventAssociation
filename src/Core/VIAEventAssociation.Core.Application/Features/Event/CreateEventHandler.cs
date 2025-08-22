using ViaEventAssociation.Core.Application.CommandDispatching;
using ViaEventAssociation.Core.Application.Features.Commands.Event;
using ViaEventAssociation.Core.Domain;
using ViaEventAssociation.Core.Domain.Aggregates.Events;

namespace ViaEventAssociation.Core.Application.Features.Event;

internal class CreateEventHandler(IEventRepository eventRepository, IUnitOfWork unitOfWork)
    : ICommandHandler<CreateEventCommand> {
    public async Task<Result> HandleAsync(CreateEventCommand command) {
        var result = await eventRepository.GetByIdAsync(command.Id);

        if (result.Payload != null)
            return Error.EventAlreadyExists;

        if (command is not CreateEventCommand createEventCommand) return Error.InvalidCommand;
        var @event = global::ViaEventAssociation.Core.Domain.Aggregates.Events.Event.Create(null);

        if (@event.IsFailure)
            return @event.Error;
        await eventRepository.AddAsync(@event.Payload);
        await unitOfWork.SaveChangesAsync();

        return Result.Success();

    }
}

