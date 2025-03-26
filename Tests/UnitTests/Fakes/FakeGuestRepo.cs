using VIAEventAssociation.Core.Domain.Aggregates.Guests;
using VIAEventAssociation.Core.Tools.OperationResult;

namespace UnitTests.Fakes;

public class FakeGuestRepo : IGuestRepository
{
    public readonly List<Guest> _guests = new();
    
    public Task<Result> AddAsync(Guest aggregate)
    {
        _guests.Add(aggregate);
        return Task.FromResult(Result.Success());
    }

    public Task<Result> UpdateAsync(Guest aggregate)
    {
        var existingGuest = _guests.FirstOrDefault(e => e.Id == aggregate.Id);
        if (existingGuest == null) return Task.FromResult(Result.Fail(Error.GuestNotFound));

        existingGuest = aggregate;
        return Task.FromResult(Result.Success());
    }

    public Task<Result> DeleteAsync(GuestId id)
    {
        var existingGuest = _guests.FirstOrDefault(e => e.Id == id);
        if (existingGuest == null) return Task.FromResult(Result.Fail(Error.GuestNotFound));

        _guests.Remove(existingGuest);
        return Task.FromResult(Result.Success());
    }

    public Task<Result<Guest>> GetByIdAsync(GuestId id)
    {
        var existingGuest = _guests.FirstOrDefault(e => e.Id == id);
        if (existingGuest == null) return Task.FromResult(Result<Guest>.Fail(Error.GuestNotFound));

        return Task.FromResult(Result<Guest>.Success(existingGuest));
    }

    public Task<Result<List<Guest>>> GetAllAsync()
    {
        return Task.FromResult(Result<List<Guest>>.Success(_guests));
    }
}