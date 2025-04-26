using ViaEventAssociation.Core.Domain.Agregates.Events;
using ViaEventAssociation.Core.Domain.Common.Values;

public class EventTest {
    //test for non nullable single primitive
    public async Task StrongIdAsPk() {
        await using var ctx = DbContextTestHelper.SetupContext();

        var @event = Event.Create().Payload;

        await DbContextTestHelper.SaveAndClearAsync(@event, ctx);

        var retrieved = ctx.Events.SingleOrDefault(x => x.Id == @event.Id);
    }

    // test for non nullable single primitive valued value object EventTitle
    [Fact]
    public async Task NonNullableSinglePrimitiveValuedValueObject() {
        await using var ctx = DbContextTestHelper.SetupContext();
        var value = EventTitle.Create("Event Title").Payload;
        var entity = EventFactory.Init().Build();
        entity.UpdateTitle(value);

        await DbContextTestHelper.SaveAndClearAsync(entity, ctx);

        var retrieved = ctx.Events.Single(x => x.Id == entity.Id);
        Assert.NotNull(retrieved.Title);
        Assert.Equal(value, retrieved.Title);
    }

    // test for nullable single primitive valued value object EventDescription
    [Fact]
    public async Task NullableSinglePrimitiveValuedValueObject() {
        await using var ctx = DbContextTestHelper.SetupContext();
        var value = EventDescription.Create("Event Description").Payload;
        var entity = EventFactory.Init().Build();
        entity.UpdateDescription(value);

        await DbContextTestHelper.SaveAndClearAsync(entity, ctx);

        var retrieved = ctx.Events.Single(x => x.Id == entity.Id);
        Assert.NotNull(retrieved.Description);
        Assert.Equal(value, retrieved.Description);
    }

    // test for non nullable single primitive valued value object DateTimeRange
    [Fact]
    public async Task NonNullableSinglePrimitiveValuedValueObjectDateTimeRange() {
        await using var ctx = DbContextTestHelper.SetupContext();
        var value = EventDateTime.Create(DateTime.Now, DateTime.Now.AddHours(1)).Payload;
        var entity = EventFactory.Init().Build();
        entity.UpdateTimeSpan(value);

        await DbContextTestHelper.SaveAndClearAsync(entity, ctx);

        var retrieved = ctx.Events.Single(x => x.Id == entity.Id);
        Assert.NotNull(retrieved.TimeSpan);
        Assert.Equal(value, retrieved.TimeSpan);
    }

    // test for nullable single primitive valued value object DateTimeRange
    [Fact]
    public async Task NullableSinglePrimitiveValuedValueObjectDateTimeRange() {
        await using var ctx = DbContextTestHelper.SetupContext();
        var value = EventDateTime.Create(DateTime.Now, DateTime.Now.AddHours(1)).Payload;
        var entity = EventFactory.Init().Build();
        entity.UpdateTimeSpan(value);

        await DbContextTestHelper.SaveAndClearAsync(entity, ctx);

        var retrieved = ctx.Events.Single(x => x.Id == entity.Id);
        Assert.NotNull(retrieved.TimeSpan);
        Assert.Equal(value, retrieved.TimeSpan);
    }

    // test for non nullable single primitive valued value object EventVisibility
    [Fact]
    public async Task NonNullableSinglePrimitiveValuedValueObjectEventVisibility() {
        await using var ctx = DbContextTestHelper.SetupContext();
        var value = EventVisibility.Public;
        var entity = EventFactory.Init().Build();
        entity.Visibility = value;

        await DbContextTestHelper.SaveAndClearAsync(entity, ctx);

        var retrieved = ctx.Events.Single(x => x.Id == entity.Id);
        Assert.NotNull(retrieved.Visibility);
        Assert.Equal(value, retrieved.Visibility);
    }

    // test for nullable single primitive valued value object Status
    [Fact]
    public async Task NullableSinglePrimitiveValuedValueObjectStatus() {
        await using var ctx = DbContextTestHelper.SetupContext();
        var value = EventStatus.Ready;
        var entity = EventFactory.Init().Build();
        entity.Status = value;

        await DbContextTestHelper.SaveAndClearAsync(entity, ctx);

        var retrieved = ctx.Events.Single(x => x.Id == entity.Id);
        Assert.NotNull(retrieved.Status);
        Assert.Equal(value, retrieved.Status);
    }

    // test for non nullable single primitive valued value object NumberOfGuests
    [Fact]
    public async Task NonNullableSinglePrimitiveValuedValueObjectNumberOfGuests() {
        await using var ctx = DbContextTestHelper.SetupContext();
        var value = NumberOfGuests.Create(5).Payload;
        var entity = EventFactory.Init().Build();
        entity.MaxNumberOfGuests = value;

        await DbContextTestHelper.SaveAndClearAsync(entity, ctx);

        var retrieved = ctx.Events.Single(x => x.Id == entity.Id);
        Assert.NotNull(retrieved.MaxNumberOfGuests);
        Assert.Equal(value, retrieved.MaxNumberOfGuests);
    }
}