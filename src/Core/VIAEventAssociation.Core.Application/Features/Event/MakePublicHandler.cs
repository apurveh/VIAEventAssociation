using ViaEventAssociation.Core.Application.CommandDispatching;
using ViaEventAssociation.Core.Application.CommandDispatching.Commands;
using ViaEventAssociation.Core.Application.Features.Commands.Event;
using ViaEventAssociation.Core.Domain;
using ViaEventAssociation.Core.Domain.Aggregates.Events;

namespace ViaEventAssociation.Core.Application.Features.Event;

public class MakePublicHandler(IEventRepository eventRepository, IUnitOfWork unitOfWork) : EventHandler(eventRepository, unitOfWork), ICommandHandler<MakePublicCommand>
{
    protected override Task<Result> PerformAction(global::ViaEventAssociation.Core.Domain.Aggregates.Events.Event @event, Command<EventId> command) {
        if (command is MakePublicCommand makePublicCommand)
            return Task.FromResult(@event.MakePublic());
        return Task.FromResult(Result.Fail(Error.InvalidCommand));
    }
}