using ViaEventAssociation.Core.Application.CommandDispatching.Commands;
using ViaEventAssociation.Core.Domain.Aggregates.Events;
using ViaEventAssociation.Core.Domain.Agregates.Events;

namespace ViaEventAssociation.Core.Application.Features.Commands.Event;

public class UpdateDescriptionCommand : Command<EventId> {
    private UpdateDescriptionCommand(EventId id, EventDescription description) : base(id) {
        Description = description;
    }

    public EventDescription Description { get; }

    public static Result<UpdateDescriptionCommand> Create(string id, string description) {
        var errors = new HashSet<Error>();

        var eventId = EventId.Create(id)
            .OnFailure(error => errors.Add(error));

        var eventDescription = EventDescription.Create(description)
            .OnFailure(error => errors.Add(error));

        if (errors.Any())
            return Error.Add(errors);

        return new UpdateDescriptionCommand(eventId.Payload, eventDescription.Payload);
    }
}