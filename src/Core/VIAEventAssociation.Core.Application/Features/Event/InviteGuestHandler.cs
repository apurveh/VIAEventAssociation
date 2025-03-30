using VIAEventAssociation.Core.Application.CommandDispatching.Commands.Event;
using VIAEventAssociation.Core.Domain.Aggregates.Events;
using VIAEventAssociation.Core.Domain.Aggregates.Guests;
using VIAEventAssociation.Core.Domain.Common.UnitOfWork;
using VIAEventAssociation.Core.Tools.OperationResult;

namespace VIAEventAssociation.Core.Application.Features.Event;

public class InviteGuestHandler(IGuestRepository guestRepository, IEventRepository eventRepository, IUnitOfWork unitOfWork)
    : EventHandler<InviteGuestCommand>(eventRepository, unitOfWork)
{
    protected override async Task<Result> PerformAction(Domain.Aggregates.Events.Event @event,
        InviteGuestCommand command)
    {
        var guest = await guestRepository.GetByIdAsync(command.GuestId);
        if (guest.IsFailure)
            return guest.Error;

        var result = @event.SendInvitation(guest.Payload);
        return result.IsFailure ? result.Error : Result.Success();
    }
}