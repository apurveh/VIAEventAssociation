using System.Threading.Tasks;
using ViaEventAssociation.Core.Application.CommandDispatching;
using ViaEventAssociation.Core.Application.CommandDispatching.Commands.Guest;

namespace UnitTests.Common.Dispatcher;

public class RequestToJoinMockHandler : ICommandHandler<RequestToJoinCommand> {
    public int HandleAsyncCallCount { get; private set; }


    public Task<Result> HandleAsync(RequestToJoinCommand command) {
        HandleAsyncCallCount++;
        Result<int> result = HandleAsyncCallCount;
        return Task.FromResult(Result.Ok);
    }
}