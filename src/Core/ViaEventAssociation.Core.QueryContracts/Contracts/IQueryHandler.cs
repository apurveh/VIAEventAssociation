namespace ViaEventAssociation.Core.QueryContracts.Contracts;

public interface IQueryHandler<in TQuery, TAnswer>
{
    public Task<Result<TAnswer>> HandleAsync(TQuery query);
}