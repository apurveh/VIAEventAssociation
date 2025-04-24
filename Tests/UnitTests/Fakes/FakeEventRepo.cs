using VIAEventAssociation.Core.Domain.Aggregates.Events;
using VIAEventAssociation.Core.Domain.Aggregates.Guests;
using VIAEventAssociation.Core.Tools.OperationResult;

namespace UnitTests.Fakes;

public class FakeEventRepo : IEventRepository
{
    public readonly List<Event> _events = new();
    
    public Task<Result> AddAsync(Event aggregate)
    {
        _events.Add(aggregate);
        return Task.FromResult(Result.Success());
    }

    public Task<Result> UpdateAsync(Event aggregate)
    {
        var existingEvent = _events.FirstOrDefault(e => e.Id == aggregate.Id);
        if (existingEvent == null)
            return Task.FromResult(Result.Fail(Error.EventIsNotFound));

        existingEvent = aggregate;
        return Task.FromResult(Result.Success());
    }

    public Task<Result> DeleteAsync(EventId id)
    {
        var existingEvent = _events.FirstOrDefault(e => e.Id == id);
        if (existingEvent == null)
            return Task.FromResult(Result.Fail(Error.EventIsNotFound));

        _events.Remove(existingEvent);
        return Task.FromResult(Result.Success());
    }

    public Task<Result<Event>> GetByIdAsync(EventId id)
    {
        var existingEvent = _events.FirstOrDefault(e => e.Id == id);
        return Task.FromResult(existingEvent == null ? Result<Event>.Fail(Error.EventIsNotFound) : Result<Event>.Success(existingEvent));
    }

    public Task<Result<List<Event>>> GetAllAsync()
    {
        return Task.FromResult(Result<List<Event>>.Success(_events));
    }

    public Task<IEnumerable<Event>> GetPublicActiveEventsAsync()
    {
        throw new NotImplementedException();
    }

    public Task<Event?> GetEventWithParticipantsAsync(EventId id)
    {
        throw new NotImplementedException();
    }

    public Task<bool> IsGuestAlreadyParticipating(EventId eventId, GuestId guestId)
    {
        throw new NotImplementedException();
    }

    public Task<bool> IsGuestAlreadyInvited(EventId eventId, GuestId guestId)
    {
        throw new NotImplementedException();
    }

    public Task<int> GetConfirmedParticipantCountAsync(EventId eventId)
    {
        throw new NotImplementedException();
    }

    public Task<IEnumerable<Event>> GetUpcomingEventsAsync()
    {
        throw new NotImplementedException();
    }
}