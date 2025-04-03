using Serilog;
using VIAEventAssociation.Core.Application.CommandDispatching.Commands;
using VIAEventAssociation.Core.Domain.Common.UnitOfWork;
using VIAEventAssociation.Core.Tools.OperationResult;

namespace VIAEventAssociation.Core.Application.CommandDispatching.Dispatcher.Decorators;

public class TransactionCommandDispatcherDecorator : ICommandDispatcher
{
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

    public async Task<Result> DispatchAsync<TId>(ICommand<TId> command) {
        var result = await _decoratedDispatcher.DispatchAsync(command);

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