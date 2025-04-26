using Microsoft.EntityFrameworkCore;
using ViaEventAssociation.Core.Domain;
using ViaEventAssociation.Core.Domain.Agregates.Events;
using ViaEventAssociation.Infrastructure.SqliteDmPersistence.EventPersistence;
using ViaEventAssociation.Infrastructure.SqliteDmPersistence.Persistence;
using ViaEventAssociation.Infrastructure.SqliteDmPersistence.UnitOfWork;

namespace IntegrationTests.Repositories;

public class EventRepositoryTest {
    private readonly DmContext _context;
    private readonly IEventRepository _eventRepository;
    private readonly IUnitOfWork _unitOfWork;

    public EventRepositoryTest() {
        var factory = new DesignTimeContextFactory();
        _context = factory.CreateDbContext(new string[] { });
        _context.Database.EnsureCreated();
        _context.Database.Migrate();

        _eventRepository = new EventEfRepository(_context);
        _unitOfWork = new SqliteUnitOfWork(_context);
    }

    [Fact]
    public async void AddEventAsync_ShouldAddEventToDatabase() {
        // Arrange
        var @event = EventFactory.Init()
            .Build();

        // Act
        await _eventRepository.AddAsync(@event);
        await _unitOfWork.SaveChangesAsync();

        // Assert
        var events = await _eventRepository.GetAllAsync();
        Assert.NotEmpty(events.Payload);
    }

    [Fact]
    public async void GetEventByIdAsync_ShouldReturnEvent() {
        // Arrange
        var @event = EventFactory.Init()
            .Build();

        // Act
        await _eventRepository.AddAsync(@event);
        await _unitOfWork.SaveChangesAsync();

        // Assert
        var retrieved = await _eventRepository.GetByIdAsync(@event.Id);
        Assert.NotNull(retrieved.Payload);
        Assert.Equal(@event.Id, retrieved.Payload.Id);
    }
}