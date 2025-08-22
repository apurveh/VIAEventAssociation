using ViaEventAssociation.Core.Application.CommandDispatching;
using ViaEventAssociation.Core.Application.CommandDispatching.Commands;
using ViaEventAssociation.Core.Application.Features.Commands.Event;
using ViaEventAssociation.Core.Domain;
using ViaEventAssociation.Core.Domain.Aggregates.Events;

namespace ViaEventAssociation.Core.Application.Features.Event;

public class MakePrivateHandler(IEventRepository eventRepository, IUnitOfWork unitOfWork) : EventHandler(eventRepository, unitOfWork), ICommandHandler<MakePrivateCommand>
{
    protected override Task<Result> PerformAction(global::ViaEventAssociation.Core.Domain.Aggregates.Events.Event @event, Command<EventId> command) {
        if (command is MakePrivateCommand makePrivateCommand)
            return Task.FromResult(@event.MakePrivate());
        return Task.FromResult(Result.Fail(Error.InvalidCommand));
    }
}