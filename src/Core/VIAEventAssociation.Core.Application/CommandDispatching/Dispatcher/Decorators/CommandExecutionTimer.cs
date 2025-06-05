using System.Diagnostics;
using Serilog;
using ViaEventAssociation.Core.Application.CommandDispatching.Commands;
using ViaEventAssociation.Core.Application.Features.Dispatcher;

namespace ViaEventAssociation.Core.Application.CommandDispatching.Dispatcher.Decorators;

public class CommandExecutionTimer : ICommandDispatcher {
    private readonly ICommandDispatcher _next;

    static CommandExecutionTimer() {
        Log.Logger = new LoggerConfiguration()
            .MinimumLevel.Debug()
            .WriteTo.Console()
            .WriteTo.File("logs/commands_log.txt", rollingInterval: RollingInterval.Day)
            .CreateLogger();
    }

    public CommandExecutionTimer(ICommandDispatcher next) {
        _next = next;
    }

    public async Task<Result> DispatchAsync(Command command) {
        var stopwatch = Stopwatch.StartNew();
        var result = await _next.DispatchAsync(command);
        stopwatch.Stop();
        Log.Information("Command {CommandType} executed in {ElapsedMilliseconds}ms", command.GetType().Name, stopwatch.ElapsedMilliseconds);
        return result;
    }
}