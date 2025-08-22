using System.Threading.Tasks;
using ViaEventAssociation.Core.Domain;
using ViaEventAssociation.Core.Domain.Aggregates.Events;
using ViaEventAssociation.Core.Domain.Aggregates.Guests;
using ViaEventAssociation.Core.Domain.Aggregates.Locations;

namespace UnitTests.Fakes;

public class FakeUoW : IUnitOfWork {
    public FakeUoW(IEventRepository events, ILocationRepository locations, IGuestRepository guests) {
        Events = events;
        Locations = locations;
        Guests = guests;
    }

    public FakeUoW() {
        //     Events = new FakeEventRepository();
        //     Locations = new FakeLocationRepository();
        //     Guests = new FakeGuestRepository();
    }

    public IEventRepository Events { get; }
    public ILocationRepository Locations { get; }
    public IGuestRepository Guests { get; }

    public Task SaveChangesAsync() {
        return Task.CompletedTask;
    }

    public void Dispose() { }
}