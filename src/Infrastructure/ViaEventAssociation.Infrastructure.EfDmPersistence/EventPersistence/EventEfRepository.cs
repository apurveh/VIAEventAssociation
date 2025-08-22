using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using ViaEventAssociation.Core.Domain.Aggregates.Events;

namespace ViaEventAssociation.Infrastructure.SqliteDmPersistence.EventPersistence;

public class EventEfRepository : RepositoryEfBase<Event, EventId>, IEventRepository {
    public EventEfRepository(DmContext context) : base(context) {

    }
}