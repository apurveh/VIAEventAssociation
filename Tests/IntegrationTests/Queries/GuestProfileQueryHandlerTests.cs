using ViaEventAssociation.Core.QueryContracts.Queries;
using ViaEventAssociation.Infrastructure.EfcQueries;
using ViaEventAssociation.Infrastructure.EfcQueries.Queries;
using Xunit;
using Xunit.Abstractions;

namespace IntegrationTests.Queries;

public class GuestProfileQueryHandlerTests
{
    private readonly ITestOutputHelper _output;

    public GuestProfileQueryHandlerTests(ITestOutputHelper output)
    {
        _output = output;
    }

    [Fact]
    public async Task HandleAsync_WhenGuestExists_ReturnsSuccessResult()
    {
        // Arrange
        DbproductionContext context = DbproductionContext.SetupContextWithSeed();
        var handler = new ProfilePageQueryHandler(context);
        const string guestId = "230c1a99-d5c7-4fbc-9f48-07ccbb100936";
        var query = new GuestProfilePage.Query(guestId);

        // Act
        var result = await handler.HandleAsync(query);

        // Assert
        Assert.True(result.IsSuccess);
        Assert.Equal(guestId,result.Payload.Id);
    }
}