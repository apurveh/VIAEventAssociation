namespace ViaEventAssociation.Core.QueryContracts.Contracts;

public interface IQueryHandler<in TQuery, TAnswer>
{
    Task<TAnswer> HandleAsync(TQuery query);
}