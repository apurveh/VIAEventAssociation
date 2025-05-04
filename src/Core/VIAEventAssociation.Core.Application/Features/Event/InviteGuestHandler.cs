using ViaEventAssociation.Core.Application.CommandDispatching;
using ViaEventAssociation.Core.Application.CommandDispatching.Commands;
using ViaEventAssociation.Core.Application.Features.Commands.Event;
using ViaEventAssociation.Core.Domain;
using ViaEventAssociation.Core.Domain.Aggregates.Events;
using ViaEventAssociation.Core.Domain.Agregates.Events;
using ViaEventAssociation.Core.Domain.Agregates.Guests;
using EventHandler = ViaEventAssociation.Core.Application.Features.Event.EventHandler;

public class InviteGuestHandler(IGuestRepository guestRepository, IUnitOfWork unitOfWork, IEventRepository eventRepository) : EventHandler(eventRepository, unitOfWork), ICommandHandler<InviteGuestCommand>
{
    private readonly IGuestRepository _guestRepository = guestRepository;

    protected override Task<Result> PerformAction(Event eve, Command<EventId> command) {
        if (command is InviteGuestCommand inviteGuestCommand) {
            var guest = _guestRepository.GetByIdAsync(inviteGuestCommand.GuestId).Result;
            if (guest.IsFailure)
                return Task.FromResult(Result.Fail(guest.Error));

            var result = eve.SendInvitation(guest.Payload);
            if (result.IsFailure)
                return Task.FromResult(Result.Fail(result.Error));
            return Task.FromResult(Result.Success());
        }

        return Task.FromResult(Result.Fail(Error.InvalidCommand));
    }
}