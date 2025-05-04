using ViaEventAssociation.Core.Application.CommandDispatching.Commands;

namespace ViaEventAssociation.Core.Application.CommandDispatching;

public interface ICommandHandler<TCommand>
    where TCommand : Command { }