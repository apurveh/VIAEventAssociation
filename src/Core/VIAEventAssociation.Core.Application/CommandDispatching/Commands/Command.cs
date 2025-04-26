namespace ViaEventAssociation.Core.Application.CommandDispatching.Commands;

public abstract class Command { }

public abstract class Command<TId>(TId id) : Command {
    public TId Id { get; } = id;
}