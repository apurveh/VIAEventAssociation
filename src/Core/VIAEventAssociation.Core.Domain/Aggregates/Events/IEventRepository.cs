using ViaEventAssociation.Core.Domain.Aggregates.Events;
using ViaEventAssociation.Core.Domain.Common;

namespace ViaEventAssociation.Core.Domain.Agregates.Events;

public interface IEventRepository : IRepository<Event, EventId> { }