
using ViaEventAssociation.Core.QueryContracts.Queries;
using ViaEventAssociation.Infrastructure.EfcQueries;
using ViaEventAssociation.Infrastructure.EfcQueries.Queries;
using Xunit;
using Xunit.Abstractions;
namespace IntegrationTests.Queries;

public class ViewUnpublishedEventQueryHandlerTests
{
    private readonly ITestOutputHelper _output;

    public ViewUnpublishedEventQueryHandlerTests(ITestOutputHelper output)
    {
       _output = output; 
    }
    
    [Fact]
    public async Task HandleAsync_WhenEventExists_ReturnsSuccessResult()
    {
        // Arrange
        DbproductionContext context = DbproductionContext.SetupContextWithSeed();
        var query = new ViewUnpublishedEvents.Query();
        var handler = new ViewUnpublishedEventsQueryHandler(context);

        // Act
        var result = await handler.HandleAsync(query);

        // Assert
        Assert.True(result.IsSuccess);
        Assert.True(result.Payload.Drafts.Count > 0);
        Assert.True(result.Payload.Ready.Count > 0);
        Assert.True(result.Payload.Cancelled.Count > 0);
    }

    [Fact]
    public async Task HandleAsync_WhenNoEventsExist_ReturnsEmptyLists()
    {
        // Arrange
        DbproductionContext context = DbproductionContext.SetupContext();
        var query = new ViewUnpublishedEvents.Query();
        var handler = new ViewUnpublishedEventsQueryHandler(context);

        // Act
        var result = await handler.HandleAsync(query);

        // Assert
        Assert.True(result.IsSuccess);
        Assert.Empty(result.Payload.Drafts);
        Assert.Empty(result.Payload.Ready);
        Assert.Empty(result.Payload.Cancelled);
    }
}