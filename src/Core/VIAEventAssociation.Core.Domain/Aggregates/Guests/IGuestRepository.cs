using ViaEventAssociation.Core.Domain.Common;

namespace ViaEventAssociation.Core.Domain.Agregates.Guests;

public interface IGuestRepository : IRepository<Guest, GuestId> { }