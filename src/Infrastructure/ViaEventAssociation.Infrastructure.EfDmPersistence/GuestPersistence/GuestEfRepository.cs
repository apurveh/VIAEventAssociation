using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using ViaEventAssociation.Core.Domain.Agregates.Guests;

namespace ViaEventAssociation.Infrastructure.SqliteDmPersistence.GuestPersistence;

public class GuestEfRepository(DmContext context) : RepositoryEfBase<Guest, GuestId>(context), IGuestRepository {
    public async Task<Result> AddAsync(Guest aggregate) {
        await context.Guests.AddAsync(aggregate);
        await context.SaveChangesAsync();
        return await Task.FromResult(Result.Success());
    }

    public Task<Result> UpdateAsync(Guest aggregate) {
        throw new NotImplementedException();
    }

    public Task<Result> DeleteAsync(GuestId id) {
        throw new NotImplementedException();
    }

    public async Task<Result<Guest>> GetByIdAsync(GuestId id) {
        return await context
            .Guests
            .Include(p => p.Participations)
            .SingleAsync(p => p.Id == id);
    }

    public async Task<Result<List<Guest>>> GetAllAsync() {
        return await context
            .Guests
            .Include(p => p.Participations)
            .ToListAsync();
    }
}