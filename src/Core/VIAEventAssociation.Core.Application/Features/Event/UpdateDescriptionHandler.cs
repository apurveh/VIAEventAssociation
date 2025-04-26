using ViaEventAssociation.Core.Application.CommandDispatching.Commands;
using ViaEventAssociation.Core.Application.Features.Commands.Event;
using ViaEventAssociation.Core.Domain;
using ViaEventAssociation.Core.Domain.Aggregates.Events;
using ViaEventAssociation.Core.Domain.Agregates.Events;

namespace ViaEventAssociation.Core.Application.Features.Event;

public class UpdateDescriptionHandler(IEventRepository eventRepository, IUnitOfWork unitOfWork) : EventHandler(eventRepository, unitOfWork) {
    protected override Task<Result> PerformAction(global::Event @event, Command<EventId> command) {
        if (command is UpdateDescriptionCommand updateDescriptionCommand)
            return Task.FromResult(@event.UpdateDescription(updateDescriptionCommand.Description));
        return Task.FromResult(Result.Fail(Error.InvalidCommand));
    }
}