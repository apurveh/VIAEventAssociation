//
//
// using ViaEventAssociation.Core.Application.CommandDispatching;
// using ViaEventAssociation.Core.Application.CommandDispatching.Commands;
// using ViaEventAssociation.Core.Application.Features.Commands.Event;
// using ViaEventAssociation.Core.Domain;
// using ViaEventAssociation.Core.Domain.Aggregates.Events;
// using ViaEventAssociation.Core.Domain.Agregates.Events;
//
// internal class CreateEventHandler(IEventRepository eventRepository, IUnitOfWork unitOfWork) : ICommandHandler<Command<EventId>> {
//     // private readonly IEventRepository _eventRepository;
//     // private readonly IUnitOfWork _unitOfWork;
//
//     // public CreateEventHandler(IEventRepository eventRepository, IUnitOfWork unitOfWork) {
//     //     _eventRepository = eventRepository;
//     //     _unitOfWork = unitOfWork;
//     // }
//
//     public async Task<Result> HandleAsync(CreateEventCommand command) {
//         global::Event.Create(Organizer.Create("test", "test@test.com:").Payload);
//         // var result = await _eventRepository.AddAsync(@event);
//         // if (result.IsFailure) {
//         // return Result.Failure(result.Error);
//         // }
//
//         // await _unitOfWork.SaveChangesAsync();
//         return Result.Success();
//     }
//
//     public async Task<Result> HandleAsync(Command<EventId> command) {
//         var result = await eventRepository.GetByIdAsync(command.Id);
//
//         if (result.IsSuccess)
//             return Error.EventAlreadyExists;
//
//         if (command is CreateEventCommand createEventCommand) {
//             //TODO: Implement CreateEventCommand
//             var @event = global::Event.Create(null);
//
//             if (@event.IsFailure)
//                 return @event.Error;
//
//             await eventRepository.AddAsync(@event.Payload);
//             await unitOfWork.SaveChangesAsync();
//
//             return Result.Success();
//         }
//     }
// }

