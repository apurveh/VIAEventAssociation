using ViaEventAssociation.Core.Application.CommandDispatching;
using ViaEventAssociation.Core.Application.Features.Commands.Event;

namespace UnitTests.Common.Dispatcher;

public class ActivateEventMockHandler : ICommandHandler<ActivateEventCommand> {
    public int HandleAsyncCallCount { get; private set; }

    public Task<Result> HandleAsync(ActivateEventCommand command) {
        HandleAsyncCallCount++;
        Result<int> result = HandleAsyncCallCount;
        return Task.FromResult(Result.Ok);
    }
}