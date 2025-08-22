using Microsoft.EntityFrameworkCore;
using ViaEventAssociation.Core.Domain;
using ViaEventAssociation.Core.Domain.Aggregates.Events;
using ViaEventAssociation.Infrastructure.SqliteDmPersistence.EventPersistence;
using ViaEventAssociation.Infrastructure.SqliteDmPersistence.Persistence;
using ViaEventAssociation.Infrastructure.SqliteDmPersistence.UnitOfWork;

namespace IntegrationTests.Repositories;

public class EventRepositoryTest {
    private readonly IEventRepository _eventRepository;
    private readonly IUnitOfWork _unitOfWork;

    public EventRepositoryTest()
    {
        var context = DbContextTestHelper.SetupContext();
        _eventRepository = new EventEfRepository(context);
        _unitOfWork = new SqliteUnitOfWork(context);
    }

    [Fact]
    public async void AddEventAsync_ShouldAddEventToDatabase() {
        // Arrange
        var @event = EventFactory.Init()
            .Build();

        // Act
        var addResult = await _eventRepository.AddAsync(@event);
        await _unitOfWork.SaveChangesAsync();
        var result = await _eventRepository.GetByIdAsync(@event.Id);

        // Assert
       Assert.True(addResult.IsSuccess);
        Assert.NotNull(result.Payload);
        Assert.Equal(@event.Id, result.Payload.Id);
    }

    [Fact]
    public async void GetEventByIdAsync_ShouldReturnNull_WhenEventDoesntExist() {
        // Arrange
        // Act
        var result = await _eventRepository.GetByIdAsync(EventFactory.Init().Build().Id);

        // Assert
        Assert.True(result.IsFailure);
        Assert.Equal(result.Error, Error.EventIsNotFound);
        Assert.Null(result.Payload);
    }
}