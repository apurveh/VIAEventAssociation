namespace VIAEventAssociation.Core.Tools.OperationResult;

public class Error
{
    private Error(string message)
    {
        Message = message;
        Next = null;
    }
    
    public string Message { get; }
    public Error Next { get; private set; }
    
    public static Error InvalidEmail => new ("The email address format provided is invalid.");   
    public static Error InvalidDateTimeRange => new("The start time must be before the end time.");
    public static Error Unknown => new("An unknown error occurred.");
    public static Error BlankString => new("The provided string cannot be blank.");
    public static Error TitleTooLong => new("The provided title is too long, maximum length is 50 characters.");
    public static Error NullString => new("The provided string cannot be null.");
    public static Error EventStatusIsActive => new("The event status is active and cannot be modified.");
    public static Error EventStatusIsCanceled => new("The event status is canceled and cannot be modified.");
    public static Error EventStatusIsActiveAndMaxGuestsReduced => new("The event status is active and the maximum number of guests cannot be reduced.");
    public static Error EventTimeSpanIsNotSet => new("The event start and end times are not set.");
    public static Error EventTimeSpanIsInPast => new("The event start time is in the past, change the start time to a future date.");
    public static Error EventTitleIsDefault => new("The event title is the default title and must be changed.");
    public static Error InvalidEmailDomain => new("The email address domain is invalid, only people with a VIA mail can register.");
    public static Error InvalidName => new("The provided name is invalid, only letters are allowed.");
    public static Error EventStatusIsNotActive => new("The event status is not active, only active events can be joined.");
    public static Error EventIsPrivate => new("The event is private and cannot be joined, without a valid reason.");
    public static Error GuestAlreadyParticipating => new("The guest is already participating in the event.");
    public static Error GuestAlreadyRequestedToJoinEvent => new("The guest already requested to join the event.");
    public static Error InvitationNotFound => new("The invitation was not found.");
    public static Error EventIsFull => new("The event is full and cannot accept more guests.");
    public static Error EventIsPast => new("You cannot cancel your participation of past or ongoing events.");
    public static Error InvitationPendingNotFound => new("The pending invitation was not found, only pending invitations can be accepted or rejected.");
    public static Error InvitationPendingOrAcceptedNotFound => new("The pending or accepted invitation was not found, only pending or accepted invitations can be rejected.");
    public static Error EventStatusIsCanceledAndCannotRejectInvitation => new("The event status is canceled and the invitation cannot be rejected.");
    public static Error EventStatusIsReadyAndCannotRejectInvitation => new("The event status is ready and the invitation cannot be rejected.");
    public static Error OnlyActiveEventsCanBeCanceled => new("Only active events can be canceled.");
    public static Error EventAlreadyExistsInLocation => new("The event already exists in the location.");
    public static Error EventTimeSpanOverlapsWithAnotherEvent => new("The time span overlaps with an existing event.");
    public static Error StartTimeIsInThePast => new("The start time is in the past.");
    public static Error LocationNotAvailable => new("The location is not available for the given time range.");
    public static Error EventTimeSpanOutsideOfNewAvailability => new("The event time span is outside of the new availability.");
    public static Error GuestAlreadyInvited => new("The guest is already invited to the event.");
    public static Error JoinRequestNotFound => new("The join request was not found.");
    public static Error JoinRequestIsNotPending => new("The join request is not pending, only pending join requests can be accepted or rejected.");
    
    public static Error TooShortName(int minLength) {
        return new Error($"The provided name is too short, minimum length is {minLength} characters.");
    }
    
    public static Error ActiveEventCannotBeMadePrivate => new("Active events cannot be made private.");
    
    public static Error CancelledEventCannotBeModified => new("Canceled event cannot be modified.");

    public static Error TooLongName(int maxLength) {
        return new Error($"The provided name is too long, maximum length is {maxLength} characters.");
    }

    public static Error TooFewGuests(int minGuests) {
        return new Error($"The number of guests cannot be less than {minGuests}.");
    }

    public static Error TooManyGuests(int maxGuests) {
        return new Error($"The number of guests cannot be more than {maxGuests}.");
    }

    public static Error TooShortTitle(int minTitleLength) {
        return new Error($"The provided title is too short, minimum length is {minTitleLength} characters.");
    }

    public static Error TooLongTitle(int maxTitleLength) {
        return new Error($"The provided title is too long, maximum length is {maxTitleLength} characters.");
    }

    public static Error TooShortDescription(int minTitleLength) {
        return new Error($"The provided description is too short, minimum length is {minTitleLength} characters.");
    }

    public static Error TooLongDescription(int maxTitleLength) {
        return new Error($"The provided description is too long, maximum length is {maxTitleLength} characters.");
    }

    public static Error InvalidStartDateTime(DateTime start) {
        return new Error($"The start time {start} is invalid. Rooms are usable from 08 am on a day, to 01 am on the next day.");
    }

    public static Error InvalidEndDateTime(DateTime end) {
        return new Error($"The end time {end} is invalid. Rooms are usable from 08 am on a day, to 01 am on the next day.");
    }

    public static Error InvalidDuration(DateTime start, DateTime end, TimeSpan maxDuration) {
        return new Error($"The duration of the event is invalid, maximum duration is {maxDuration}, start: {start}, end: {end}.");
    }

    public static Error EventTooShort(TimeSpan minDuration) {
        return new Error($"The duration of the event is too short, minimum duration is {minDuration}.");
    }
    
    public static Error FromException(Exception exception) {
        return new Error(exception.Message);
    }
    
    private void Append(Error error) {
        if (Next is null)
            Next = error;
        else
            Next.Append(error);
    }

    public static Error Add(HashSet<Error> errors) {
        var error = errors.First();
        foreach (var e in errors.Skip(1)) error.Append(e);

        return error;
    }

    public IEnumerable<Error> GetAllErrors() {
        var errors = new List<Error> {this};
        var current = Next;
        while (current is not null) {
            errors.Add(current);
            current = current.Next;
        }

        return errors;
    }

    public override string ToString() {
        if (Next != null) return $"{Message}\n{Next}";

        return Message;
    }

    public override bool Equals(object obj) {
        if (obj is null || GetType() != obj.GetType())
            return false;

        if (Message != ((Error) obj).Message)
            return false;

        return true;
    }

    public override int GetHashCode() {
        return Message.GetHashCode();
    }
}