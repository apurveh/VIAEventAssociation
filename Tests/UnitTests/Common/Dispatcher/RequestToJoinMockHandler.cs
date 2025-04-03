using VIAEventAssociation.Core.Application.CommandDispatching;
using VIAEventAssociation.Core.Application.CommandDispatching.Commands.Guest;
using VIAEventAssociation.Core.Domain.Aggregates.Guests;
using VIAEventAssociation.Core.Tools.OperationResult;

namespace UnitTests.Common.Dispatcher;

public class RequestToJoinMockHandler : ICommandHandler<RequestToJoinCommand, GuestId>
{
    public int HandleAsyncCallCount { get; private set; }
    
    public Task<Result> HandleAsync(RequestToJoinCommand command)
    {
        HandleAsyncCallCount++;
        Result<int> result = HandleAsyncCallCount;
        return Task.FromResult(Result.Ok);
    }
}