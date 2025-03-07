using VIAEventAssociation.Core.Domain.Common.Bases;
using VIAEventAssociation.Core.Domain.Common.Values;
using VIAEventAssociation.Core.Tools.OperationResult;

namespace VIAEventAssociation.Core.Domain.Aggregates.Events;

public class Event : AggregateRoot<EventId>
{
    private Event(EventId id) : base(id) { }
    
    public EventTitle EventTitle { get; set; }
    public EventDescription EventDescription { get; set; }
    public EventDateTime EventTime { get; set; }
    public EventVisibility EventVisibility { get; set; }
    public EventStatus EventStatus { get; set; }

    public static Result<Event> Create()
    {
        var errors = new HashSet<Error>();
        
        var eventIdResult = EventId.GenerateId();
        if (eventIdResult.IsFailure)
            errors.Add(eventIdResult.Error);
        
        var eventTitleResult = EventTitle.Create(CONST.DRAFT_EVENT_TITLE);
        var eventDescriptionResult = EventDescription.Create(CONST.DRAFT_EVENT_DESCRIPTION);
        var eventStatus = EventStatus.Draft;
        var eventVisibility = EventVisibility.Private;

        if (errors.Any())
            return Error.Add(errors);

        return new Event(eventIdResult.Payload)
        {
            EventTitle = eventTitleResult.Payload,
            EventDescription = eventDescriptionResult.Payload,
            EventStatus = eventStatus,
            EventVisibility = eventVisibility
        };
    }

    public override string ToString()
    {
        return EventTitle.Value;
    }
}