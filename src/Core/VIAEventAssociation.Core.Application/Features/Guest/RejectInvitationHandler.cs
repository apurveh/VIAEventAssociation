using ViaEventAssociation.Core.Application.CommandDispatching.Commands;
using ViaEventAssociation.Core.Application.CommandDispatching.Commands.Guest;
using ViaEventAssociation.Core.Domain;
using ViaEventAssociation.Core.Domain.Agregates.Events;
using ViaEventAssociation.Core.Domain.Agregates.Guests;

namespace ViaEventAssociation.Core.Application.Features.Guest;

public class RejectInvitationHandler(IGuestRepository guestRepository, IUnitOfWork unitOfWork, IEventRepository eventRepository)
    : GuestHandler(guestRepository, unitOfWork) {
    private readonly IEventRepository _eventRepository = eventRepository;

    protected override Task<Result> PerformAction(Domain.Agregates.Guests.Guest guest, Command<GuestId> command) {
        if (command is RejectInvitationCommand rejectInvitationCommand) {
            var @event = _eventRepository.GetByIdAsync(rejectInvitationCommand.EventId).Result;

            if (@event.IsFailure)
                return Task.FromResult(Result.Fail(@event.Error));

            var result = guest.DeclineInvitation(@event.Payload);

            if (result.IsFailure)
                return Task.FromResult(Result.Fail(result.Error));

            return Task.FromResult(Result.Success());
        }

        return Task.FromResult(Result.Fail(Error.InvalidCommand));
    }
}