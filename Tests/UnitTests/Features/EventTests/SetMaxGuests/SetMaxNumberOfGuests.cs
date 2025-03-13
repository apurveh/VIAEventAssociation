using VIAEventAssociation.Core.Domain.Aggregates.Events;
using VIAEventAssociation.Core.Domain.Common.Values;
using VIAEventAssociation.Core.Tools.OperationResult;

namespace UnitTests.Features.EventTests.SetMaxGuests;
public class SetMaxNumberOfGuests
{   
    //UC7.S1 and S2
    
    [Theory]
    [InlineData(5)]
    [InlineData(10)]
    [InlineData(25)]
    [InlineData(50)]
    public void SetMaxNumberOfGuests_WhenEventIsDraft_ShouldUpdateMaxGuests(int newMaxGuests)
    {
        var @event = EventFactory
            .Init()
            .WithValidTitle()
            .WithStatus(EventStatus.Draft) // Start with Draft status
            .WithMaxNumberOfGuests(5)
            .Build();

        // Act
        var @result = @event.SetMaxNumberOfGuests(newMaxGuests);

        // Assert
        Assert.True(@result.IsSuccess);
        Assert.Equal(newMaxGuests, @event.MaxNumberOfGuests.Value);
    }
    
    [Theory]
    [InlineData(5)]
    [InlineData(10)]
    [InlineData(25)]
    [InlineData(50)]
    public void SetMaxNumberOfGuests_WhenEventIsReady_ShouldUpdateMaxGuests(int newMaxGuests)
    {
        var @event = EventFactory
            .Init()
            .WithValidTitle()
            .WithStatus(EventStatus.Ready)
            .WithMaxNumberOfGuests(5)
            .Build();

        // Act
        var @result = @event.SetMaxNumberOfGuests(newMaxGuests);

        // Assert
        Assert.True(@result.IsSuccess);
        Assert.Equal(newMaxGuests, @event.MaxNumberOfGuests.Value);
    }
    
    // UC7.S3
    [Theory]
    [InlineData(5, 5)]  
    [InlineData(5, 10)] 
    [InlineData(10, 15)]
    [InlineData(25, 50)]
    public void SetMaxNumberOfGuests_WhenEventIsActiveAndIncreasingOrSame_ShouldUpdateMaxGuests(int initialGuests, int newMaxGuests)
    {
        // Arrange
        var @event = EventFactory
            .Init()
            .WithValidTitle()
            .WithStatus(EventStatus.Active)
            .WithMaxNumberOfGuests(initialGuests) 
            .Build();

        // Act
        var result = @event.SetMaxNumberOfGuests(newMaxGuests);

        // Assert
        Assert.True(result.IsSuccess);
        Assert.Equal(newMaxGuests, @event.MaxNumberOfGuests.Value);
    }
    
    // UC7.F1
    [Theory]
    [InlineData(10, 5)]
    [InlineData(20, 10)]
    [InlineData(50, 25)]
    public void SetMaxNumberOfGuests_WhenEventIsActiveAndMaxGuestsReduced_ShouldReturnFailure(int initialGuests, int newMaxGuests)
    {
        // Arrange
        var @event = EventFactory
            .Init()
            .WithValidTitle()
            .WithStatus(EventStatus.Active)
            .WithMaxNumberOfGuests(initialGuests)
            .Build();

        // Act
        var result = @event.SetMaxNumberOfGuests(newMaxGuests);

        // Assert
        Assert.True(result.IsFailure);
        Assert.Equal(Error.EventStatusIsActiveAndMaxGuestsReduced, result.Error);
        Assert.Equal(initialGuests, @event.MaxNumberOfGuests.Value);
    }
    
    //UC7.F2
    [Theory]
    [InlineData(5)]
    [InlineData(10)]
    [InlineData(25)]
    [InlineData(50)]
    public void SetMaxNumberOfGuests_WhenEventIsCancelled_ShouldReturnFailure(int newMaxGuests)
    {
        // Arrange
        var @event = EventFactory
            .Init()
            .WithValidTitle()
            .WithStatus(EventStatus.Cancelled)
            .WithMaxNumberOfGuests(10)
            .Build();

        // Act
        var result = @event.SetMaxNumberOfGuests(newMaxGuests);

        // Assert
        Assert.True(result.IsFailure);
        Assert.Equal(Error.EventStatusIsCanceled, result.Error);
        Assert.Equal(10, @event.MaxNumberOfGuests.Value);
    }
    //UC7.F4
    [Theory]
    [InlineData(0)]
    [InlineData(-1)]
    [InlineData(4)]
    public void SetMaxNumberOfGuests_WhenGuestNumberIsTooSmall_ShouldReturnFailure(int invalidMaxGuests)
    {
        // Arrange
        var @event = EventFactory
            .Init()
            .WithValidTitle()
            .WithStatus(EventStatus.Draft)
            .WithMaxNumberOfGuests(10)
            .Build();

        // Act
        var result = @event.SetMaxNumberOfGuests(invalidMaxGuests);

        // Assert
        Assert.True(result.IsFailure);
        Assert.Equal(Error.TooFewGuests(5), result.Error);
        Assert.Equal(10, @event.MaxNumberOfGuests.Value);
    }
    //UC7.F5
    [Theory]
    [InlineData(51)]
    [InlineData(60)]
    [InlineData(100)]
    public void SetMaxNumberOfGuests_WhenGuestNumberExceedsMaximum_ShouldReturnFailure(int invalidMaxGuests)
    {
        // Arrange
        var @event = EventFactory
            .Init()
            .WithValidTitle()
            .WithStatus(EventStatus.Draft)
            .WithMaxNumberOfGuests(10)
            .Build();

        // Act
        var result = @event.SetMaxNumberOfGuests(invalidMaxGuests);

        // Assert
        Assert.True(result.IsFailure);
        Assert.Equal(Error.TooManyGuests(50), result.Error);
        Assert.Equal(10, @event.MaxNumberOfGuests.Value);
    }

}