using ViaEventAssociation.Core.Domain.Agregates.Guests;
using ViaEventAssociation.Core.Domain.Common.Values;

namespace IntegrationTests.DmContextConfiguration;

public class GuestTest {
    //test for non nullable single primitive valued ID
    [Fact]
    public async Task NonNullableSinglePrimitiveValuedID() {
        await using var ctx = DbContextTestHelper.SetupContext();
        var name = NameType.Create("John").Payload;
        var lastName = NameType.Create("Doe").Payload;
        var email = Email.Create("JDO@via.dk").Payload;

        var guest = Guest.Create(name, lastName, email).Payload;

        await DbContextTestHelper.SaveAndClearAsync(guest, ctx);

        var retrieved = ctx.Guests.SingleOrDefault(x => x.Id == guest.Id);
    }


    // test for non nullable single primitive valued value object NameType
    [Fact]
    public async Task NonNullableSinglePrimitiveValuedValueObjectNameType() {
        await using var ctx = DbContextTestHelper.SetupContext();
        var name = NameType.Create("John").Payload;
        var lastName = NameType.Create("Doe").Payload;
        var email = Email.Create("JDO@via.dk").Payload;

        var guest = Guest.Create(name, lastName, email).Payload;

        await DbContextTestHelper.SaveAndClearAsync(guest, ctx);

        var retrieved = ctx.Guests.Single(x => x.Id == guest.Id);
        Assert.NotNull(retrieved.FirstName);
        Assert.Equal(name, retrieved.FirstName);
    }

    // test for non nullable single primitive valued value object Email
    [Fact]
    public async Task NonNullableSinglePrimitiveValuedValueObjectEmail() {
        await using var ctx = DbContextTestHelper.SetupContext();
        var guest = GuestFactory.InitWithDefaultsValues().Build();

        await DbContextTestHelper.SaveAndClearAsync(guest, ctx);

        var retrieved = ctx.Guests.Single(x => x.Id == guest.Id);
        Assert.NotNull(retrieved.Email);
        Assert.Equal(guest.Email, retrieved.Email);
    }

    // test for non nullable single primitive valued value object LastName
    [Fact]
    public async Task NonNullableSinglePrimitiveValuedValueObjectLastName() {
        await using var ctx = DbContextTestHelper.SetupContext();
        var guest = GuestFactory.InitWithDefaultsValues().Build();

        await DbContextTestHelper.SaveAndClearAsync(guest, ctx);

        var retrieved = ctx.Guests.Single(x => x.Id == guest.Id);
        Assert.NotNull(retrieved.LastName);
        Assert.Equal(guest.LastName, retrieved.LastName);
    }
}