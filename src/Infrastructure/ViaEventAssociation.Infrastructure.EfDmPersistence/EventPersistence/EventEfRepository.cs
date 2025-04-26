using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using ViaEventAssociation.Core.Domain.Aggregates.Events;
using ViaEventAssociation.Core.Domain.Agregates.Events;

namespace ViaEventAssociation.Infrastructure.SqliteDmPersistence.EventPersistence;

public class EventEfRepository(DmContext context) : RepositoryEfBase<Event, EventId>(context), IEventRepository {
    public async Task<Result> AddAsync(Event aggregate) {
        await context.Events.AddAsync(aggregate);
        await context.SaveChangesAsync();
        return await Task.FromResult(Result.Success());
    }

    public Task<Result> UpdateAsync(Event aggregate) {
        throw new NotImplementedException();
    }

    public Task<Result> DeleteAsync(EventId id) {
        throw new NotImplementedException();
    }

    public async Task<Result<Event>> GetByIdAsync(EventId id) {
        return await context
            .Events
            .Include(p => p.Participations)
            .SingleAsync(p => p.Id == id);
    }

    public async Task<Result<List<Event>>> GetAllAsync() {
        return await context
            .Events
            .Include(p => p.Participations)
            .ToListAsync();
    }
}