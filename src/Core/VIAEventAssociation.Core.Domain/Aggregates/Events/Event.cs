using VIAEventAssociation.Core.Domain.Aggregates.Entities;
using VIAEventAssociation.Core.Domain.Aggregates.Entities.Invitation;
using VIAEventAssociation.Core.Domain.Aggregates.Guests;
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
    public NumberOfGuests MaxNumberOfGuests { get; set; }
    public HashSet<Participation> Participations { get; private set; }
    public int ConfirmedParticipants =>
        Participations.Count(p => p.ParticipationStatus is ParticipationStatus.Accepted);

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
            EventVisibility = eventVisibility,
            MaxNumberOfGuests = NumberOfGuests.Create(CONST.MIN_NUMBER_OF_GUESTS).Payload,
            Participations = new HashSet<Participation>()
        };
    }

    
    public Result UpdateTitle(string? newTitle)
    {
        if (newTitle is null)
            return Error.NullString;

        if (EventStatus == EventStatus.Active)
            return Error.EventStatusIsActive;

        if (EventStatus == EventStatus.Cancelled)  
            return Error.EventStatusIsCanceled;

        var eventTitleResult = EventTitle.Create(newTitle);

        if (eventTitleResult.IsFailure)
            return eventTitleResult.Error;

        EventTitle = eventTitleResult.Payload;

        return Result.Success();
    }


    public Result UpdateDescription(string newDescription)
    {
        var eventDescriptionResult = EventDescription.Create(newDescription);

        if (eventDescriptionResult.IsFailure)
        {
            return eventDescriptionResult.Error;
        }
        
        EventDescription = eventDescriptionResult.Payload;
        return Result.Success();
    }

    public Result MakePrivate()
    {
        if (EventStatus == EventStatus.Active)
        {
            return Error.ActiveEventCannotBeMadePrivate;
        }

        if (EventStatus == EventStatus.Cancelled)
        {
            return Error.CancelledEventCannotBeModified;
        }
        if (EventVisibility == EventVisibility.Public)
        {
            EventVisibility = EventVisibility.Private;
            EventStatus = EventStatus.Draft;
        }
        return Result.Success();
    }

    public Result SetMaxNumberOfGuests(int maxNumberOfGuests)
    {
        if (EventStatus == EventStatus.Active && maxNumberOfGuests < MaxNumberOfGuests.Value)
        {
            return Error.EventStatusIsActiveAndMaxGuestsReduced;
        }
        if (EventStatus == EventStatus.Cancelled)
        {
            return Error.EventStatusIsCanceled;
        }

        var maxNumberOfGuestsResult = NumberOfGuests.Create(maxNumberOfGuests);
        if (maxNumberOfGuestsResult.IsFailure)
        {
            return maxNumberOfGuestsResult.Error;
        }

        MaxNumberOfGuests = maxNumberOfGuestsResult.Payload;
        return Result.Success();
    }

    public Result ReadyEvent()
    {
        if (EventStatus == EventStatus.Cancelled)
        {
            return Error.CancelledEventCannotBeModified;
        }

        if (EventTitle.Value == CONST.DRAFT_EVENT_TITLE)
        {
            return Error.EventTitleIsDefault;
        }

        if (EventDescription.Value == CONST.DRAFT_EVENT_DESCRIPTION)
        {
            return Error.EventDescriptionIsDefault;
        }
        
        EventStatus = EventStatus.Ready;
        return Result.Success();
    }
    
    public Result<ParticipationStatus> RequestToJoin(JoinRequest joinRequest)
    {
        var errors = new HashSet<Error>();

        if (ConfirmedParticipants >= MaxNumberOfGuests.Value)
            errors.Add(Error.EventIsFull);
        
        if (EventStatus is not EventStatus.Active)
            errors.Add(Error.EventStatusIsNotActive);
        
        if (DateTimeRange.IsPast(EventTime))
            errors.Add(Error.EventTimeSpanIsInPast);

        if (EventVisibility is EventVisibility.Private &&
            string.IsNullOrEmpty(joinRequest.Reason))
            errors.Add(Error.EventIsPrivate);
        
        if (errors.Any())
            return Error.Add(errors);

        Participations.Add(joinRequest);

        if (EventVisibility is EventVisibility.Private &&
            !isValidReason(joinRequest.Reason))
            return ParticipationStatus.Declined;
        
        if (EventVisibility is EventVisibility.Private &&
            isValidReason(joinRequest.Reason))
            return ParticipationStatus.Accepted;

        return ParticipationStatus.Accepted;
    }
    
    public Result<Invitation> SendInvitation(Guest guest)
    {
        var errors = new HashSet<Error>();

        if (ConfirmedParticipants >= MaxNumberOfGuests.Value)
            errors.Add(Error.EventIsFull);
        
        if (EventStatus is not EventStatus.Active && EventStatus is not EventStatus.Ready)
            errors.Add(Error.EventStatusIsNotActive);
        
        if (DateTimeRange.IsPast(EventTime))
            errors.Add(Error.EventTimeSpanIsInPast);
        
        if (IsInvitedButNotConfirmed(guest))
            errors.Add(Error.GuestAlreadyInvited);
        
        if (IsParticipating(guest))
            errors.Add(Error.GuestAlreadyParticipating);

        var result = Invitation.SendInvitation(this, guest)
            .OnSuccess(participation => Participations.Add(participation))
            .OnFailure(error => errors.Add(error));
        
        if (errors.Any())
            return Error.Add(errors);

        return result;
    }
    
    public Result ValidateInvitationResponse(Invitation invitation)
    {
        var errors = new HashSet<Error>();
        
        if (EventStatus is not EventStatus.Active)
            errors.Add(Error.EventStatusIsNotActive);
        
        if (DateTimeRange.IsPast(EventTime))
            errors.Add(Error.EventTimeSpanIsInPast);
        
        if (ConfirmedParticipants >= MaxNumberOfGuests.Value)
            errors.Add(Error.EventIsFull);
        
        if (Participations.FirstOrDefault(p => p.Event == invitation.Event) is null)
            errors.Add(Error.InvitationNotFound);
        
        if (Participations.FirstOrDefault(p => p.Event == invitation.Event).ParticipationStatus != ParticipationStatus.Accepted)
            errors.Add(Error.GuestAlreadyParticipating);

        if (errors.Any())
            return Error.Add(errors);

        return Result.Ok;
    }
    
    public Result ValidateInvitationDecline(Invitation invitation)
    {
        var errors = new HashSet<Error>();
        
        if (EventStatus is EventStatus.Cancelled)
            errors.Add(Error.EventStatusIsCanceledAndCannotRejectInvitation);
        
        if (EventStatus is EventStatus.Ready)
            errors.Add(Error.EventStatusIsReadyAndCannotRejectInvitation);

        if (errors.Any())
            return Error.Add(errors);

        return Result.Ok;
    }

    private bool IsInvitedButNotConfirmed(Guest guest)
    {
        return Participations.OfType<Invitation>()
            .Any(p => p.Guest == guest && p.ParticipationStatus is ParticipationStatus.Pending);
    }

    private bool IsParticipating(Guest guest)
    {
        return Participations.Any(p => p.Guest == guest && p.ParticipationStatus is ParticipationStatus.Accepted);
    }

    private bool isValidReason(string? joinRequestReason)
    {
        return joinRequestReason?.Length > 25;
    }
    
    public bool IsEventPast()
    {
        return DateTimeRange.IsPast(EventTime);
    }

    public override string ToString()
    {
        return EventTitle.Value;
    }
}