using ViaEventAssociation.Core.Domain.Common;

namespace ViaEventAssociation.Core.Domain.Aggregates.Events;

public interface IEventRepository : IRepository<Event, EventId> { }