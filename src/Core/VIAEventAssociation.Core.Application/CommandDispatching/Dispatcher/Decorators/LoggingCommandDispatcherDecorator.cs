using Serilog;
using ViaEventAssociation.Core.Application.CommandDispatching.Commands;
using ViaEventAssociation.Core.Application.Features.Dispatcher;

namespace ViaEventAssociation.Core.Application.CommandDispatching.Dispatcher.Decorators;

public class LoggingCommandDispatcherDecorator : ICommandDispatcher {
    private readonly ICommandDispatcher _next;

    static LoggingCommandDispatcherDecorator() {
        Log.Logger = new LoggerConfiguration()
            .MinimumLevel.Debug()
            .WriteTo.Console()
            .WriteTo.File("logs/commands_log.txt", rollingInterval: RollingInterval.Day)
            .CreateLogger();
    }

    public LoggingCommandDispatcherDecorator(ICommandDispatcher next) {
        _next = next;
    }


    public async Task<Result> DispatchAsync(Command command) {
        Log.Information("Dispatching command {CommandType}", command.GetType().Name);
        try {
            var result = await _next.DispatchAsync(command);
            Log.Information("Command {CommandType} processed with result: {Result}", command.GetType().Name, result.IsSuccess ? "Success" : "Failure");
            return result;
        }
        catch (Exception ex) {
            Log.Error(ex, "Error processing command {CommandType}", command.GetType().Name);
            throw; // Important to re-throw the exception unless specifically handled
        }
    }
}