using UnitTests.Features.EventTests;
using UnitTests.IntegrationTests;
using VIAEventAssociation.Core.Domain.Aggregates.Events;
using VIAEventAssociation.Infrastructure.SqliteDmPersistence;
using VIAEventAssociation.Infrastructure.SqliteDmPersistence.Repositories;
using Xunit;

namespace IntegrationTests.Repositories;

public class EventRepositoryTests : IntegrationTestBase
{
    private readonly EventRepository _eventRepository;
    private readonly EfUnitOfWork _unitOfWork;

    public EventRepositoryTests()
    {
        _eventRepository = new EventRepository(Context);
        _unitOfWork = new EfUnitOfWork(Context);
    }

    [Fact]
    public async Task AddAsync_ShouldSaveEventCorrectly()
    {
        // Arrange
        var @event = EventFactory.Init()
            .WithValidTitle()
            .WithValidDescription()
            .WithValidTimeInFuture()
            .WithMaxNumberOfGuests(20)
            .WithVisibility(EventVisibility.Public)
            .Build();

        // Act
        await _eventRepository.AddAsync(@event);
        await _unitOfWork.SaveChangesAsync();

        // Assert
        var savedEvent = await Context.Events.FindAsync(@event.Id);
        Assert.NotNull(savedEvent);
        Assert.Equal(@event.Id, savedEvent.Id);
        Assert.Equal(@event.EventTitle, savedEvent.EventTitle);
    }
}
