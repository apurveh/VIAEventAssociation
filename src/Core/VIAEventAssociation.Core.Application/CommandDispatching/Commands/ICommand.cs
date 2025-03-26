namespace VIAEventAssociation.Core.Application.CommandDispatching.Commands;

public interface ICommand<TId>
{
    TId Id { get; }
}