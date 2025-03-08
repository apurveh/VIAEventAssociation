using VIAEventAssociation.Core.Domain.Aggregates.Entities;
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