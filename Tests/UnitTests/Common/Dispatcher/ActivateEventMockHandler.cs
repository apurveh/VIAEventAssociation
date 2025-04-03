using VIAEventAssociation.Core.Application.CommandDispatching;
using VIAEventAssociation.Core.Application.CommandDispatching.Commands.Event;
using VIAEventAssociation.Core.Domain.Aggregates.Events;
using VIAEventAssociation.Core.Tools.OperationResult;

namespace UnitTests.Common.Dispatcher;

public class ActivateEventMockHandler : ICommandHandler<ActivateEventCommand, EventId>
{
    public int HandleAsyncCallCount { get; private set; }

    public Task<Result> HandleAsync(ActivateEventCommand command)
    {
        HandleAsyncCallCount++;
        Result<int> result = HandleAsyncCallCount;
        return Task.FromResult(Result.Ok);
    }
}