using ViaEventAssociation.Core.Application.CommandDispatching.Commands;
using ViaEventAssociation.Core.Domain.Aggregates.Events;

namespace ViaEventAssociation.Core.Application.Features.Commands.Event;

public class UpdateTitleCommand : Command<EventId> {
    private UpdateTitleCommand(EventId id, EventTitle title) : base(id) {
        Title = title;
    }

    public EventTitle Title { get; }

    public static Result<UpdateTitleCommand> Create(string id, string title) {
        var errors = new HashSet<Error>();

        var eventId = EventId.Create(id)
            .OnFailure(error => errors.Add(error));

        var eventTitle = EventTitle.Create(title)
            .OnFailure(error => errors.Add(error));

        if (errors.Any())
            return Error.Add(errors);

        return new UpdateTitleCommand(eventId.Payload, eventTitle.Payload);
    }
}