using ViaEventAssociation.Core.Application.CommandDispatching.Commands;
using ViaEventAssociation.Core.Domain.Aggregates.Events;

namespace ViaEventAssociation.Core.Application.Features.Commands.Event;

public class MakePrivateCommand : Command<EventId> {
    private MakePrivateCommand(EventId id) : base(id) { }

    public static Result<MakePrivateCommand> Create(string id) {
        var errors = new HashSet<Error>();

        var eventId = EventId.Create(id)
            .OnFailure(error => errors.Add(error));

        if (errors.Any())
            return Error.Add(errors);

        return new MakePrivateCommand(eventId.Payload);
    }
}