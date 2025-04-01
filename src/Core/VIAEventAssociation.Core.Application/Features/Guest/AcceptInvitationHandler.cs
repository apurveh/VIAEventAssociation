using VIAEventAssociation.Core.Application.CommandDispatching.Commands.Guest;
using VIAEventAssociation.Core.Domain.Aggregates.Events;
using VIAEventAssociation.Core.Domain.Aggregates.Guests;
using VIAEventAssociation.Core.Domain.Common.UnitOfWork;
using VIAEventAssociation.Core.Tools.OperationResult;

namespace VIAEventAssociation.Core.Application.Features.Guest;

public class AcceptInvitationHandler(IGuestRepository guestRepository, IEventRepository eventRepository, IUnitOfWork unitOfWork)
    : GuestHandler<AcceptInvitationCommand>(guestRepository, unitOfWork)
{
    protected override async Task<Result> PerformAction(Domain.Aggregates.Guests.Guest guest,
        AcceptInvitationCommand command)
    {
        var eventResult = await eventRepository.GetByIdAsync(command.EventId);
        if (eventResult.IsFailure)
            return eventResult.Error;

        var result = guest.AcceptInvitation(eventResult.Payload);
        return result.IsFailure ? result.Error : Result.Success();
    }
}