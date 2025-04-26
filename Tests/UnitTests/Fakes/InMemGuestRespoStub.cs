using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ViaEventAssociation.Core.Domain.Agregates.Guests;

namespace UnitTests.Fakes;

public class InMemGuestRespoStub : IGuestRepository {
    // set up the in-memory database
    public readonly List<Guest> _guests = new();

    public Task<Result> AddAsync(Guest aggregate) {
        _guests.Add(aggregate);
        return Task.FromResult(Result.Success());
    }

    public Task<Result> UpdateAsync(Guest aggregate) {
        // find the guest in the list
        var existingGuest = _guests.FirstOrDefault(e => e.Id == aggregate.Id);
        if (existingGuest == null) return Task.FromResult(Result.Fail(Error.GuestIsNotFound));

        // update the guest
        existingGuest = aggregate;
        return Task.FromResult(Result.Success());
    }

    public Task<Result> DeleteAsync(GuestId id) {
        // find the guest in the list
        var existingGuest = _guests.FirstOrDefault(e => e.Id == id);
        if (existingGuest == null) return Task.FromResult(Result.Fail(Error.GuestIsNotFound));

        // delete the guest
        _guests.Remove(existingGuest);
        return Task.FromResult(Result.Success());
    }

    public Task<Result<Guest>> GetByIdAsync(GuestId id) {
        // find the guest in the list
        var existingGuest = _guests.FirstOrDefault(e => e.Id == id);
        if (existingGuest == null) return Task.FromResult(Result<Guest>.Fail(Error.GuestIsNotFound));

        return Task.FromResult(Result<Guest>.Success(existingGuest));
    }

    public Task<Result<List<Guest>>> GetAllAsync() {
        return Task.FromResult(Result<List<Guest>>.Success(_guests));
    }
}