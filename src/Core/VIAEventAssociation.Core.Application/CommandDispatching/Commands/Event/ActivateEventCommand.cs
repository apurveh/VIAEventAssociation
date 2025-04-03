using VIAEventAssociation.Core.Application.CommandDispatching.Commands;
using VIAEventAssociation.Core.Domain.Aggregates.Events;
using VIAEventAssociation.Core.Tools.OperationResult;

namespace VIAEventAssociation.Core.Application.CommandDispatching.Commands.Event
{
    public class ActivateEventCommand : ICommand<EventId>
    {
        public EventId Id { get; }

        private ActivateEventCommand(EventId id)
        {
            Id = id;
        }

        public static Result<ActivateEventCommand> Create(string eventIdAsString)
        {
            var errors = new HashSet<Error>();
            var eventId = EventId.Create(eventIdAsString)
                .OnFailure(error => errors.Add(error));

            if (errors.Any())
                return Error.Add(errors);

            return new ActivateEventCommand(eventId.Payload);
        }
    }
}