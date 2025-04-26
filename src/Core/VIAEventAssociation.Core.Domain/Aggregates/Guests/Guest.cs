using ViaEventAssociation.Core.Domain.Common.Bases;
using ViaEventAssociation.Core.Domain.Common.Values;
using ViaEventAssociation.Core.Domain.Contracts;
using ViaEventAssociation.Core.Domain.Entities;
using ViaEventAssociation.Core.Domain.Entities.Invitation;

namespace ViaEventAssociation.Core.Domain.Agregates.Guests;

public class Guest : AggregateRoot<GuestId> {
    private Guest(GuestId id, NameType firstName, NameType lastName, Email email) : base(id) {
        FirstName = firstName;
        LastName = lastName;
        Email = email;
        Participations = new List<Participation>();
    }

    //Required by EF Core
    private Guest() : base(default!) { } // Required by EF Core

    public NameType FirstName { get; }
    public NameType LastName { get; }
    public Email Email { get; }
    public List<Participation> Participations { get; }


    public static Result<Guest> Create(GuestId guid, NameType firstName, NameType lastName, Email email) {
        if (IEmailUnusedChecker.IsEmailUsed(email).Payload)
            return Error.EmailAlreadyUsed;
        try {
            var guest = new Guest(guid, firstName, lastName, email);
            return guest;
        }
        catch (Exception exception) {
            return Error.FromException(exception);
        }
    }

    public static Result<Guest> Create(NameType firstName, NameType lastName, Email email) {
        var guid = GuestId.GenerateId().Payload;
        return Create(guid, firstName, lastName, email);
    }

    public Result<JoinRequest> RegisterToEvent(Event @event, string reason = null!) {
        var errors = new HashSet<Error>();

        if (IsPendingInEvent(@event).Payload || IsConfirmedInEvent(@event).Payload)
            errors.Add(Error.GuestAlreadyParticipating);

        var participationResult = JoinRequest.SendJoinRequest(@event, this, reason)
            .OnSuccess(participation => Participations.Add(participation));

        if (participationResult.IsFailure)
            errors.Add(participationResult.Error);

        if (errors.Any())
            return Error.Add(errors);

        return participationResult.Payload;
    }

    //TODO improve this method
    public Result CancelParticipation(Event @event) {
        var participation = Participations.FirstOrDefault(p =>
            p.Event == @event && p.ParticipationStatus != ParticipationStatus.Canceled);
        if (participation is null)
            return Result.Ok; //I could return an error that the user is not found. is better than do not act TROELS
        var result = participation.CancelParticipation();
        if (result.IsFailure)
            return result.Error;
        return Result.Ok;
    }

    public Result Serve(Invitation invitation) {
        // Check if the user is already registered to the event
        var participation = Participations.FirstOrDefault(p =>
            p.Event == invitation.Event && p.ParticipationStatus == ParticipationStatus.Accepted);

        if (participation is null) {
            Participations.Add(invitation);
            return Result.Ok;
        }

        if (participation.ParticipationStatus == ParticipationStatus.Accepted)
            return Result.Fail(Error.GuestAlreadyParticipating);

        if (participation is JoinRequest && participation.ParticipationStatus == ParticipationStatus.Pending)
            return Result.Fail(Error.GuestAlreadyRequestedToJoinEvent);

        if (participation is Invitation && participation.ParticipationStatus == ParticipationStatus.Pending)
            return Result.Fail(Error.GuestAlreadyInvited);

        return Error.GuestAlreadyParticipating;
    }

    public Result AcceptInvitation(Event @event) {
        var invitation = Participations.OfType<Invitation>().FirstOrDefault(p =>
            p.Event == @event && p.ParticipationStatus == ParticipationStatus.Pending);
        if (invitation is null)
            return Error.InvitationPendingNotFound;

        var result = invitation.AcceptsInvitation();
        if (result.IsFailure)
            return result.Error;
        return Result.Ok;
    }

    public Result DeclineInvitation(Event @event) {
        var invitation = Participations.OfType<Invitation>().FirstOrDefault(p =>
            (p.Event == @event &&
             p.ParticipationStatus == ParticipationStatus.Pending) ||
            p.ParticipationStatus == ParticipationStatus.Accepted);

        if (invitation is null)
            return Error.InvitationPendingOrAcceptedNotFound;

        var result = invitation.DeclineInvitation();
        if (result.IsFailure)
            return result.Error;
        return Result.Ok;
    }

    public Result<bool> IsConfirmedInEvent(Event @event) {
        return Participations.Any(p => p.Event == @event && p.ParticipationStatus == ParticipationStatus.Accepted);
    }

    public Result<bool> IsPendingInEvent(Event @event) {
        return Participations.Any(p => p.Event == @event && p.ParticipationStatus == ParticipationStatus.Pending);
    }

    public override string ToString() {
        return $"{FirstName} {LastName}";
    }
}