using ViaEventAssociation.Core.Domain.Aggregates.Events;
using ViaEventAssociation.Core.Domain.Agregates.Events;
using ViaEventAssociation.Core.Domain.Agregates.Guests;
using ViaEventAssociation.Core.Domain.Agregates.Locations;
using ViaEventAssociation.Core.Domain.Common.Bases;
using ViaEventAssociation.Core.Domain.Common.Values;
using ViaEventAssociation.Core.Domain.Entities;
using ViaEventAssociation.Core.Domain.Entities.Invitation;

public class Event : AggregateRoot<EventId> {
    private Event(EventId id) : base(id) { }

    private Event() : base(EventId.GenerateId().Payload) { } // Required by EF Core
    internal Organizer? Organizer { get; private set; }
    internal EventTitle Title { get; set; }
    internal EventDescription Description { get; set; }

    internal EventDateTime?
        TimeSpan { get; set; } //TODO my setter is public for testing mode should I have a developer mode?

    internal EventVisibility
        Visibility { get; set; } //TODO my setter is public for testing mode should I have a developer mode?

    internal EventStatus Status { get; set; } //TODO ask troels about this this is public for testing mode
    internal NumberOfGuests MaxNumberOfGuests { get; set; }
    public HashSet<Participation> Participations { get; private set; }
    internal Location? Location { get; private set; }

    public int ConfirmedParticipations =>
        Participations.Count(p => p.ParticipationStatus is ParticipationStatus.Accepted);

    public static Result<Event> Create() {
        var newEvent = new Event(EventId.GenerateId().Payload) {
            Title = EventTitle.Create(CONST.DEFAULT_TITLE_EVENT).Payload,
            MaxNumberOfGuests = NumberOfGuests.Create(CONST.MIN_NUMBER_OF_GUESTS).Payload,
            Status = EventStatus.Draft,
            Visibility = EventVisibility.Private,
            Description = EventDescription.InitEmpty().Payload,
            Participations = new HashSet<Participation>()
        };
        return newEvent;
    }

    public static Result<Event> Create(Organizer organizer) {
        var newEvent = new Event(EventId.GenerateId().Payload) {
            Organizer = organizer,
            Title = EventTitle.Create(CONST.DEFAULT_TITLE_EVENT).Payload,
            MaxNumberOfGuests = NumberOfGuests.Create(CONST.MIN_NUMBER_OF_GUESTS).Payload,
            Status = EventStatus.Draft,
            Visibility = EventVisibility.Private,
            Description = EventDescription.InitEmpty().Payload,
            Participations = new HashSet<Participation>()
        };
        return newEvent;
    }

    public Result UpdateTitle(EventTitle title) {
        // Use a list or hashset to collect errors since there can be multiple issues with the title.
        HashSet<Error> errors = new HashSet<Error>();

        if (Status is EventStatus.Active)
            errors.Add(Error.EventStatusIsActive);

        if (Status is EventStatus.Cancelled)
            errors.Add(Error.EventStatusIsCanceled);

        // If there are any errors, return them
        if (errors.Any())
            return Result.Fail(Error.Add(errors));

        // If there are no errors, update the title and return success
        Title = title;
        Status = EventStatus.Draft;
        return Result.Ok;
    }


    public Result UpdateDescription(EventDescription description) {
        // Use a list or hashset to collect errors since there can be multiple issues with the description.
        var errors = new HashSet<Error>();

        if (Status is EventStatus.Active)
            errors.Add(Error.EventStatusIsActive);

        if (Status is EventStatus.Cancelled)
            errors.Add(Error.EventStatusIsCanceled);

        // If there are any errors, return them
        if (errors.Any())
            return Result.Fail(Error.Add(errors)); // This assumes Error.Add can handle a HashSet<Error>

        // If there are no errors, update the description and return success
        Description = description;
        Status = EventStatus.Draft;
        return Result.Ok;
    }

    public Result UpdateTimeSpan(EventDateTime newTimeSpan) {
        // Use a list or hashset to collect errors since there can be multiple issues with the time span.
        var errors = new HashSet<Error>();

        if (Status is EventStatus.Active)
            errors.Add(Error.EventStatusIsActive);

        if (Status is EventStatus.Cancelled)
            errors.Add(Error.EventStatusIsCanceled);

        // If there are any errors, return them
        if (errors.Any())
            return Result.Fail(Error.Add(errors)); // This assumes Error.Add can handle a HashSet<Error>

        // If there are no errors, update the time span and return success
        TimeSpan = newTimeSpan;
        Status = EventStatus.Draft;
        return Result.Ok;
    }

    public Result MakePublic() {
        if (Status is EventStatus.Cancelled)
            return Result.Fail(Error.EventStatusIsCanceled);

        Visibility = EventVisibility.Public;
        return Result.Ok;
    }

    public Result MakePrivate() {
        if (Status is EventStatus.Active)
            return Result.Fail(Error.EventStatusIsActive);

        if (Status is EventStatus.Cancelled)
            return Result.Fail(Error.EventStatusIsCanceled);

        Visibility = EventVisibility.Private;
        Status = EventStatus.Draft;
        return Result.Ok;
    }

    public Result SetMaxGuests(int maxGuests) {
        var errors = new HashSet<Error>();

        if (Status is EventStatus.Active && maxGuests < MaxNumberOfGuests.Value)
            errors.Add(Error.EventStatusIsActiveAndMaxGuestsReduced);

        if (Status is EventStatus.Cancelled)
            errors.Add(Error.EventStatusIsCanceled);

        if (maxGuests < CONST.MIN_NUMBER_OF_GUESTS)
            errors.Add(Error.TooFewGuests(CONST.MIN_NUMBER_OF_GUESTS));

        if (maxGuests > CONST.MAX_NUMBER_OF_GUESTS)
            errors.Add(Error.TooManyGuests(CONST.MAX_NUMBER_OF_GUESTS));

        if (errors.Any())
            return Error.Add(errors);

        MaxNumberOfGuests = NumberOfGuests.Create(maxGuests).Payload;
        return Result.Ok;
    }

    public Result SetReady() {
        var errors = new HashSet<Error>();

        if (TimeSpan is null)
            errors.Add(Error.EventTimeSpanIsNotSet);

        if (Status is EventStatus.Cancelled)
            errors.Add(Error.EventStatusIsCanceled);

        if (TimeSpan is not null && TimeSpan?.Start! < DateTime.Now)
            errors.Add(Error.EventTimeSpanIsInPast);

        if (Title.Value == CONST.DEFAULT_TITLE_EVENT)
            errors.Add(Error.EventTitleIsDefault);

        if (errors.Any())
            return Error.Add(errors);

        Status = EventStatus.Ready;
        return Result.Ok;
    }

    public Result CancelEvent() {
        if (Status is not EventStatus.Active)
            return Result.Fail(Error.OnlyActiveEventsCanBeCanceled);

        Status = EventStatus.Cancelled;
        return Result.Ok;
    }

    public Result Activate() {
        var errors = new HashSet<Error>();

        if (Status is EventStatus.Cancelled)
            errors.Add(Error.EventStatusIsCanceled);

        if (TimeSpan is not null && TimeSpan?.Start! < DateTime.Now)
            errors.Add(Error.EventTimeSpanIsInPast);

        if (TimeSpan is null)
            errors.Add(Error.EventTimeSpanIsNotSet);

        if (errors.Any())
            return Error.Add(errors);

        Status = EventStatus.Active;
        return Result.Ok;
    }

    public Result<ParticipationStatus> RequestToJoin(JoinRequest joinRequest) {
        var errors = new HashSet<Error>();

        // Check if the event is full
        if (ConfirmedParticipations >= MaxNumberOfGuests.Value)
            errors.Add(Error.EventIsFull);

        // Check if the event is active
        if (Status is not EventStatus.Active)
            errors.Add(Error.EventStatusIsNotActive);

        // Check if the event is in the past
        if (DateTimeRange.isPast(TimeSpan))
            errors.Add(Error.EventTimeSpanIsInPast);

        if (Visibility is EventVisibility.Private && string.IsNullOrEmpty(joinRequest.Reason))
            errors.Add(Error.EventIsPrivate);

        if (errors.Any())
            return Error.Add(errors);

        Participations.Add(joinRequest);

        if (Visibility is EventVisibility.Private &&
            !isValidReason(joinRequest.Reason))
            return ParticipationStatus.Declined;

        if (Visibility is EventVisibility.Private &&
            isValidReason(joinRequest.Reason))
            return ParticipationStatus.Pending;

        return ParticipationStatus.Accepted;
    }

    private bool isValidReason(string? joinRequestReason) {
        return joinRequestReason?.Length > 25;
    }

    public Result SendInvitation(Guest guest) {
        var errors = new HashSet<Error>();

        if (ConfirmedParticipations >= MaxNumberOfGuests.Value)
            errors.Add(Error.EventIsFull);

        if (Status is not EventStatus.Active && Status is not EventStatus.Ready)
            errors.Add(Error.EventStatusIsNotActive);

        if (DateTimeRange.isPast(TimeSpan))
            errors.Add(Error.EventTimeSpanIsInPast);

        if (IsParticipating(guest) || IsInvitedButNotConfirmed(guest))
            errors.Add(Error.GuestAlreadyRequestedToJoinEvent);

        var result = Invitation.SendInvitation(this, guest)
            .OnSuccess(participation => Participations.Add(participation))
            .OnFailure(error => errors.Add(error));

        if (errors.Any())
            return Error.Add(errors);

        return Result.Ok;
    }

    public Result ValidateInvitationResponse(Invitation invitation) {
        var errors = new HashSet<Error>();

        var participation = Participations.FirstOrDefault(p => p == invitation);

        if (participation is null)
            return Error.InvitationPendingNotFound;

        if (participation is not Invitation)
            errors.Add(Error.JoinRequestNotFound);

        if (participation is Invitation && participation.ParticipationStatus is ParticipationStatus.Accepted)
            errors.Add(Error.GuestAlreadyParticipating);

        if (Status is not EventStatus.Active)
            errors.Add(Error.EventStatusIsNotActive);

        if (DateTimeRange.isPast(TimeSpan))
            errors.Add(Error.EventTimeSpanIsInPast);

        if (ConfirmedParticipations >= MaxNumberOfGuests.Value)
            errors.Add(Error.EventIsFull);

        if (errors.Any())
            return Error.Add(errors);

        return Result.Ok;
    }

    public Result ValidateInvitationDecline(Invitation invitation) {
        var errors = new HashSet<Error>();

        if (Status is EventStatus.Cancelled)
            errors.Add(Error.EventStatusIsCanceledAndCannotRejectInvitation);

        if (Status is EventStatus.Ready)
            errors.Add(Error.EventStatusIsReadyAndCannotRejectInvitation);

        if (errors.Any())
            return Error.Add(errors);

        return Result.Ok;
    }

    public Result ApproveJoinRequest(Guest guest) {
        var errors = new HashSet<Error>();

        var participation = Participations.OfType<JoinRequest>().FirstOrDefault(p => p.Guest == guest);

        if (participation is null)
            errors.Add(Error.JoinRequestNotFound);

        if (participation is not null && participation.ParticipationStatus is not ParticipationStatus.Pending)
            errors.Add(Error.JoinRequestIsNotPending);

        if (Status is not EventStatus.Active)
            errors.Add(Error.EventStatusIsNotActive);

        if (DateTimeRange.isPast(TimeSpan))
            errors.Add(Error.EventTimeSpanIsInPast);

        if (ConfirmedParticipations >= MaxNumberOfGuests.Value)
            errors.Add(Error.EventIsFull);

        if (errors.Any())
            return Error.Add(errors);

        return participation!.AcceptJoinRequest();
    }

    public Result DeclineJoinRequest(Guest guest) {
        var errors = new HashSet<Error>();

        var participation = Participations.OfType<JoinRequest>().FirstOrDefault(p => p.Guest == guest);

        if (participation is null)
            errors.Add(Error.JoinRequestNotFound);

        if (participation is not null && participation.ParticipationStatus is not ParticipationStatus.Pending)
            errors.Add(Error.JoinRequestIsNotPending);

        if (Status is not EventStatus.Active)
            errors.Add(Error.EventStatusIsNotActive);

        if (DateTimeRange.isPast(TimeSpan))
            errors.Add(Error.EventTimeSpanIsInPast);

        if (ConfirmedParticipations >= MaxNumberOfGuests.Value)
            errors.Add(Error.EventIsFull);

        if (errors.Any())
            return Error.Add(errors);

        return participation!.DeclineJoinRequest();
    }

    public bool IsParticipating(Guest guest) {
        return Participations.Any(p => p.Guest == guest && p.ParticipationStatus is ParticipationStatus.Accepted);
    }

    public bool IsInvitedButNotConfirmed(Guest guest) {
        return Participations.OfType<Invitation>()
            .Any(p => p.Guest == guest && p.ParticipationStatus is ParticipationStatus.Pending);
    }

    public Result AddLocation(Location location) {
        Location = location;
        return Result.Ok;
    }


    public bool isEventPast() {
        return DateTimeRange.isPast(TimeSpan);
    }


    public override string ToString() {
        return Title.Value;
    }
}