using VIAEventAssociation.Core.Domain.Common;

namespace VIAEventAssociation.Core.Domain.Aggregates.Guests;

public interface IGuestRepository : IRepository<Guest, GuestId> { }