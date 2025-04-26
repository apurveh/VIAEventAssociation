using ViaEventAssociation.Core.Application.CommandDispatching.Commands;
using ViaEventAssociation.Core.Application.CommandDispatching.Commands.Guest;
using ViaEventAssociation.Core.Application.Features.Guest;
using ViaEventAssociation.Core.Domain;
using ViaEventAssociation.Core.Domain.Agregates.Events;
using ViaEventAssociation.Core.Domain.Agregates.Guests;

public class CancelEventParticipationHandler(IGuestRepository guestRepository, IUnitOfWork unitOfWork, IEventRepository eventRepository)
    : GuestHandler(guestRepository, unitOfWork) {
    private readonly IEventRepository _eventRepository = eventRepository;

    protected override Task<Result> PerformAction(Guest guest, Command<GuestId> command) {
        if (command is CancelEventParticipationCommand cancelEventParticipationCommand) {
            var @event = _eventRepository.GetByIdAsync(cancelEventParticipationCommand.EventId).Result;

            if (@event.IsFailure)
                return Task.FromResult(Result.Fail(@event.Error));

            var result = guest.CancelParticipation(@event.Payload);

            if (result.IsFailure)
                return Task.FromResult(Result.Fail(result.Error));

            return Task.FromResult(Result.Success());
        }

        return Task.FromResult(Result.Fail(Error.InvalidCommand));
    }
}