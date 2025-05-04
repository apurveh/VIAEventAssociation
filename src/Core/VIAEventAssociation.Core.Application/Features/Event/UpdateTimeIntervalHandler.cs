using ViaEventAssociation.Core.Application.CommandDispatching;
using ViaEventAssociation.Core.Application.CommandDispatching.Commands;
using ViaEventAssociation.Core.Application.Features.Commands.Event;
using ViaEventAssociation.Core.Domain;
using ViaEventAssociation.Core.Domain.Aggregates.Events;
using ViaEventAssociation.Core.Domain.Agregates.Events;

namespace ViaEventAssociation.Core.Application.Features.Event;

public class UpdateTimeIntervalHandler(IEventRepository eventRepository, IUnitOfWork unitOfWork) : EventHandler(eventRepository, unitOfWork), ICommandHandler<UpdateTimeIntervalCommand>
{
    protected override Task<Result> PerformAction(global::Event @event, Command<EventId> command) {
        if (command is UpdateTimeIntervalCommand updateTimeIntervalCommand) return Task.FromResult(@event.UpdateTimeSpan(updateTimeIntervalCommand.TimeInterval));

        return Task.FromResult(Result.Fail(Error.InvalidCommand));
    }
}