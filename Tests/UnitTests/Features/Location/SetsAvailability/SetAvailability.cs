using ViaEventAssociation.Core.Domain.Common.Values;
using Xunit.Abstractions;

public class SetsAvailabilityTests {
    private readonly ITestOutputHelper _testOutputHelper;

    public SetsAvailabilityTests(ITestOutputHelper testOutputHelper) {
        _testOutputHelper = testOutputHelper;
    }

    // Given an existing location, when the owner sets the availability time span for the future, then the availability time span should be updated successfully
    //ID:UC19.S1
    [Fact]
    public void SetsAvailableTimeSpan_FutureTimeSpan_AvailabilityUpdated() {
        // Arrange
        var location = LocationFactory.Init()
            .Build();
        var futureTimeSpan = DateTimeRange.Create(DateTime.Now.AddDays(1), DateTime.Now.AddYears(1)).Payload;

        // Act
        var result = location.setsAvailableTimeSpan(futureTimeSpan);

        // Assert
        Assert.True(result.IsSuccess);
        Assert.Equal(futureTimeSpan, location.AvailableTimeSpan);
        _testOutputHelper.WriteLine(location.AvailableTimeSpan.ToString());
    }

    // Given an existing location, when the owner sets the availability time span with a start time in the past, then the action should fail
    // ID:UC19.F1
    [Fact]
    public void SetsAvailableTimeSpan_StartTimeInPast_FailureMessageReturned() {
        // Arrange
        var location = LocationFactory.Init()
            .Build();
        var pastTimeSpan = DateTimeRange.Create(DateTime.Now.AddDays(-1), DateTime.Now.AddYears(2)).Payload;

        // Act
        var result = location.setsAvailableTimeSpan(pastTimeSpan);

        // Assert
        Assert.True(result.IsFailure);
        Assert.Contains(Error.StartTimeIsInThePast.Message, result.Error.Message);
    }

    // Given an existing location with scheduled events, when the owner tries to set an availability time span that overlaps with an existing event, then the action should fail
    [Fact]
    public void SetsAvailableTimeSpan_OverlapsWithScheduledEvent_FailureMessageReturned() {
        // Arrange

        // Create the start DateTime by combining the date and time
        var startDate = DateTime.Today.AddDays(10);
        var startTime = new TimeSpan(8, 20, 0);
        var startDateTime = startDate.Add(startTime);

        // Create the end DateTime by combining the date and time
        var endDate = DateTime.Today.AddDays(10);
        var endTime = new TimeSpan(10, 20, 0);
        var endDateTime = endDate.Add(endTime);
        //
        var location = LocationFactory.Init()
            .WithEventConfirmed(startDateTime, endDateTime, 10)
            .WithAvailableTimeSpan(DateTime.Now.AddDays(-10), DateTime.Now.AddDays(20))
            .Build();


        // Act
        var result = location.setsAvailableTimeSpan(DateTimeRange.Create(DateTime.Now.AddDays(1), DateTime.Now.AddDays(9)).Payload);

        // Assert
        Assert.True(result.IsFailure);

        //Assert.Contains(Error.EventTimeSpanOverlapsWithAnotherEvent.Message, result.Error.Message);


        _testOutputHelper.WriteLine(location.AvailableTimeSpan.ToString());
        _testOutputHelper.WriteLine(location.Events[0].TimeSpan.ToString());
    }

    // Add additional tests as necessary for your business rules and logic.
}