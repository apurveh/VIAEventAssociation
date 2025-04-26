using Microsoft.EntityFrameworkCore;
using ViaEventAssociation.Core.Domain;
using ViaEventAssociation.Core.Domain.Agregates.Guests;
using ViaEventAssociation.Infrastructure.SqliteDmPersistence.GuestPersistence;
using ViaEventAssociation.Infrastructure.SqliteDmPersistence.Persistence;
using ViaEventAssociation.Infrastructure.SqliteDmPersistence.UnitOfWork;

namespace IntegrationTests.Repositories;

public class GuestRepositoryTest {
    private readonly DmContext _context;
    private readonly IGuestRepository _guestRepository;
    private readonly IUnitOfWork _unitOfWork;

    public GuestRepositoryTest() {
        var factory = new DesignTimeContextFactory();
        _context = factory.CreateDbContext(new string[] { });
        _context.Database.EnsureCreated();
        _context.Database.Migrate();

        _guestRepository = new GuestEfRepository(_context);
        _unitOfWork = new SqliteUnitOfWork(_context);
    }

    [Fact]
    public async void AddGuestAsync_ShouldAddGuestToDatabase() {
        // Arrange
        var guest = GuestFactory.InitWithDefaultsValues()
            .Build();

        // Act
        await _guestRepository.AddAsync(guest);
        await _unitOfWork.SaveChangesAsync();

        // Assert
        var guests = await _guestRepository.GetAllAsync();
        Assert.NotEmpty(guests.Payload);
    }

    [Fact]
    public async void GetGuestByIdAsync_ShouldReturnGuest() {
        // Arrange
        var guest = GuestFactory.InitWithDefaultsValues()
            .Build();

        // Act
        await _guestRepository.AddAsync(guest);
        await _unitOfWork.SaveChangesAsync();

        // Assert
        var retrieved = await _guestRepository.GetByIdAsync(guest.Id);
        Assert.NotNull(retrieved.Payload);
        Assert.Equal(guest.Id, retrieved.Payload.Id);
    }
}