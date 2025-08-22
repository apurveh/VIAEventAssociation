using ViaEventAssociation.Core.Application.CommandDispatching;
using ViaEventAssociation.Core.Application.CommandDispatching.Commands;
using ViaEventAssociation.Core.Application.CommandDispatching.Commands.Guest;
using ViaEventAssociation.Core.Application.Features.Guest;
using ViaEventAssociation.Core.Domain;
using ViaEventAssociation.Core.Domain.Aggregates.Events;
using ViaEventAssociation.Core.Domain.Aggregates.Guests;

public class AcceptsInvitationHandler(IGuestRepository guestRepository, IUnitOfWork unitOfWork, IEventRepository eventRepository)
    : GuestHandler(guestRepository, unitOfWork), ICommandHandler<AcceptsInvitationCommand>
{
    private readonly IEventRepository _eventRepository = eventRepository;

    protected override Task<Result> PerformAction(Guest guest, Command<GuestId> command) {
        if (command is AcceptsInvitationCommand acceptsInvitationCommand) {
            var @event = _eventRepository.GetByIdAsync(acceptsInvitationCommand.EventId).Result;

            if (@event.IsFailure)
                return Task.FromResult(Result.Fail(@event.Error));

            var result = guest.AcceptInvitation(@event.Payload);

            if (result.IsFailure)
                return Task.FromResult(Result.Fail(result.Error));

            return Task.FromResult(Result.Success());
        }

        return Task.FromResult(Result.Fail(Error.InvalidCommand));
    }
}