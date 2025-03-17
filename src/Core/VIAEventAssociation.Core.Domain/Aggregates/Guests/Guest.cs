using VIAEventAssociation.Core.Domain.Aggregates.Entities;
using VIAEventAssociation.Core.Domain.Aggregates.Entities.Invitation;
using VIAEventAssociation.Core.Domain.Aggregates.Events;
using VIAEventAssociation.Core.Domain.Common.Bases;
using VIAEventAssociation.Core.Domain.Common.Values;
using VIAEventAssociation.Core.Tools.OperationResult;

namespace VIAEventAssociation.Core.Domain.Aggregates.Guests;

public class Guest : AggregateRoot<GuestId>
{
    private Guest(GuestId id, NameType firstName, NameType lastName, Email email) : base(id)
    {
        FirstName = firstName;
        LastName = lastName;
        Email = email;
        Participations = new List<Participation>();
    }
    
    public NameType FirstName { get; }
    public NameType LastName { get; }
    public Email Email { get; }
    public List<Participation> Participations { get; }

    public static Result<Guest> Create(string firstName, string lastName, string email)
    {
        var errors = new HashSet<Error>();

        var guestFirstName = NameType.Create(firstName);
        if (guestFirstName.IsFailure)
            errors.Add(guestFirstName.Error);
        
        var guestLastName = NameType.Create(lastName);
        if (guestLastName.IsFailure)
            errors.Add(guestLastName.Error);
        
        var guestEmail = Email.Create(email);
        if (guestEmail.IsFailure)
            errors.Add(guestEmail.Error);

        if (errors.Any()) 
            return Error.Add(errors);

        return new Guest(GuestId.GenerateId().Payload, guestFirstName.Payload, guestLastName.Payload, guestEmail.Payload);
    }

    public Result<JoinRequest> RegisterToEvent(Event @event, string reason = null!)
    {
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
    
    public Result CancelParticipation(Event @event)
    {
        var participation = Participations.FirstOrDefault(p => p.Event == @event && p.ParticipationStatus != ParticipationStatus.Canceled);
        if (participation is null)
            return Error.GuestNotFound;

        var result = participation.CancelParticipation();
        return result.IsFailure ? result.Error : Result.Ok;
    }
    
    public Result<Participation> Serve(Invitation invitation)
    {
        var participation = Participations.FirstOrDefault(p => p.Event == invitation.Event);

        if (participation is null)
        {
            Participations.Add(invitation);
            return invitation;
        }
        
        if (participation.ParticipationStatus == ParticipationStatus.Accepted)
            return Error.GuestAlreadyParticipating;

        return participation switch
        {
            JoinRequest when participation.ParticipationStatus == ParticipationStatus.Pending => Error
                .GuestAlreadyRequestedToJoinEvent,
            Invitation when participation.ParticipationStatus == ParticipationStatus.Pending => Error
                .GuestAlreadyInvited,
            _ => Error.GuestAlreadyParticipating
        };
    }
    
    public Result AcceptInvitation(Event @event)
    {
        var invitation = Participations.OfType<Invitation>().FirstOrDefault(p => p.Event == @event && p.ParticipationStatus == ParticipationStatus.Pending);
        if (invitation is null)
            return Error.InvitationPendingNotFound;

        var result = invitation.AcceptInvitation();
        return result.IsFailure ? result.Error : Result.Ok;
    }
    
    public Result DeclineInvitation(Event @event)
    {
        var invitation = Participations.OfType<Invitation>().FirstOrDefault(p =>
            p.Event == @event && p.ParticipationStatus == ParticipationStatus.Pending || p.ParticipationStatus == ParticipationStatus.Accepted);
        if (invitation is null)
            return Error.InvitationPendingOrAcceptedNotFound;
        
        var result = invitation.DeclineInvitation();
        return result.IsFailure ? result.Error : Result.Ok;
    }

    public Result<bool> IsPendingInEvent(Event @event)
    {
        return Participations.Any(p => p.Event == @event && p.ParticipationStatus == ParticipationStatus.Pending);
    }

    public Result<bool> IsConfirmedInEvent(Event @event)
    {
        return Participations.Any(p => p.Event == @event && p.ParticipationStatus == ParticipationStatus.Accepted);
    }

    public override string ToString()
    {
        return $"{FirstName} {LastName}";
    }
}