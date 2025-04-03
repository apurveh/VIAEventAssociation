using VIAEventAssociation.Core.Application.CommandDispatching.Commands.Event;
using VIAEventAssociation.Core.Domain.Aggregates.Events;
using VIAEventAssociation.Core.Domain.Common.UnitOfWork;
using VIAEventAssociation.Core.Tools.OperationResult;

namespace VIAEventAssociation.Core.Application.Features.Event;

public class MakeEventPrivateHandler(
    IEventRepository eventRepository,
    IUnitOfWork unitOfWork
) : EventHandler<MakeEventPrivateCommand>(eventRepository, unitOfWork)
{
    protected override Task<Result> PerformAction(Domain.Aggregates.Events.Event @event, MakeEventPrivateCommand command)
    {
        var result = @event.MakePrivate();
        return Task.FromResult(result);
    }
}