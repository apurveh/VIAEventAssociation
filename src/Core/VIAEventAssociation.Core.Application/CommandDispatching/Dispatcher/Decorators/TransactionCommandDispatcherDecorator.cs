using Serilog;
using ViaEventAssociation.Core.Application.CommandDispatching.Commands;
using ViaEventAssociation.Core.Application.Features.Dispatcher;
using ViaEventAssociation.Core.Domain;

namespace ViaEventAssociation.Core.Application.CommandDispatching.Dispatcher;

public class TransactionCommandDispatcherDecorator : ICommandDispatcher {
    private readonly ICommandDispatcher _decoratedDispatcher;
    private readonly IUnitOfWork _unitOfWork;

    static TransactionCommandDispatcherDecorator() {
        Log.Logger = new LoggerConfiguration()
            .MinimumLevel.Debug()
            .WriteTo.Console()
            .WriteTo.File("logs/commands_log.txt", rollingInterval: RollingInterval.Day)
            .CreateLogger();
    }


    public TransactionCommandDispatcherDecorator(ICommandDispatcher decoratedDispatcher, IUnitOfWork unitOfWork) {
        _decoratedDispatcher = decoratedDispatcher;
        _unitOfWork = unitOfWork;
    }

    public async Task<Result> DispatchAsync(Command command) {
        // First, dispatch the command using the decorated dispatcher
        var result = await _decoratedDispatcher.DispatchAsync(command);

        // If successful, commit the transaction
        if (result.IsSuccess) {
            await _unitOfWork.SaveChangesAsync();
            Log.Information("Transaction committed successfully");
        }
        else {
            Log.Information("Transaction rolled back due to command failure");
        }

        return result;
    }
}