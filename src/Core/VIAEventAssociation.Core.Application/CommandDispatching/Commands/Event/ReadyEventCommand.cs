using VIAEventAssociation.Core.Domain.Aggregates.Events;
using VIAEventAssociation.Core.Tools.OperationResult;

namespace VIAEventAssociation.Core.Application.CommandDispatching.Commands.Event;

public class ReadyEventCommand : ICommand<EventId>
{
    public EventId Id { get; }

    private ReadyEventCommand(EventId id)
    {
        Id = id;
    }

    public static Result<ReadyEventCommand> Create(string eventIdAsString)
    {
        var errors = new HashSet<Error>();

        var eventIdResult = EventId.Create(eventIdAsString);
        if (eventIdResult.IsFailure)
        {
            errors.Add(eventIdResult.Error);
        }

        if (errors.Any())
            return Error.Add(errors);

        return new ReadyEventCommand(eventIdResult.Payload);
    }
}