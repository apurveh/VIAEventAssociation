using ViaEventAssociation.Core.Domain.Common;

namespace ViaEventAssociation.Core.Domain.Aggregates.Guests;

public interface IGuestRepository : IRepository<Guest, GuestId> { }