using Microsoft.EntityFrameworkCore;
using ViaEventAssociation.Core.Domain;
using ViaEventAssociation.Core.Domain.Aggregates.Guests;
using ViaEventAssociation.Infrastructure.SqliteDmPersistence.GuestPersistence;
using ViaEventAssociation.Infrastructure.SqliteDmPersistence.Persistence;
using ViaEventAssociation.Infrastructure.SqliteDmPersistence.UnitOfWork;

namespace IntegrationTests.Repositories;

public class GuestRepositoryTest {
    private readonly IGuestRepository _guestRepository;
    private readonly IUnitOfWork _unitOfWork;

    public GuestRepositoryTest()
    {
        var context = DbContextTestHelper.SetupContext();
        _guestRepository = new GuestEfRepository(context);
        _unitOfWork = new SqliteUnitOfWork(context);
    }

    
    [Fact]
    public async void AddGuestAsync_ShouldAddGuestToDatabase() {
        // Arrange
        var guest = GuestFactory.InitWithDefaultsValues()
            .Build();

        // Act
        var addResult = await _guestRepository.AddAsync(guest);
        await _unitOfWork.SaveChangesAsync();
        var result = await _guestRepository.GetByIdAsync(guest.Id);
        // Assert
        Assert.True(addResult.IsSuccess);
        Assert.NotNull(result.Payload);
        Assert.Equal(guest.Id, result.Payload.Id);
    }

    [Fact]
    public async void GetGuestByIdAsync_ShouldReturnNull_WhenGuestNotFound() {
        // Arrange
        var result = await _guestRepository.GetByIdAsync(GuestFactory.InitWithDefaultsValues().Build().Id);

        // Assert
        Assert.True(result.IsFailure);
        Assert.Equal(result.Error, Error.GuestIsNotFound);
        Assert.Null(result.Payload);
    }
}