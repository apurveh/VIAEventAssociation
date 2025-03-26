using VIAEventAssociation.Core.Domain.Common;

namespace VIAEventAssociation.Core.Domain.Aggregates.Events;

public interface IEventRepository : IRepository<Event, EventId> { }