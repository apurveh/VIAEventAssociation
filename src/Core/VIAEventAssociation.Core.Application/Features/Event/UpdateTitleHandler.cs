using ViaEventAssociation.Core.Application.CommandDispatching;
using ViaEventAssociation.Core.Application.CommandDispatching.Commands;
using ViaEventAssociation.Core.Application.Features.Commands.Event;
using ViaEventAssociation.Core.Domain;
using ViaEventAssociation.Core.Domain.Aggregates.Events;

namespace ViaEventAssociation.Core.Application.Features.Event;

public class UpdateTitleHandler(IEventRepository eventRepository, IUnitOfWork unitOfWork) : EventHandler(eventRepository, unitOfWork), ICommandHandler<UpdateTitleCommand>
{
    protected override Task<Result> PerformAction(global::ViaEventAssociation.Core.Domain.Aggregates.Events.Event @event, Command<EventId> command) {
        if (command is UpdateTitleCommand updateTitleCommand) return Task.FromResult(@event.UpdateTitle(updateTitleCommand.Title));

        return Task.FromResult(Result.Fail(Error.InvalidCommand));
    }
}