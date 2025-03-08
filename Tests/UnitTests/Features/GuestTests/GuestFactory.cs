using VIAEventAssociation.Core.Domain.Aggregates.Guests;

namespace UnitTests.Features.GuestTests;

public class GuestFactory
{
    private static readonly string firstName = "John";
    private static readonly string lastName = "Doe";
    private static readonly string email = "123456@via.dk";

    private Guest guest;
    private GuestFactory() {}

    public static GuestFactory Init(string firstName, string lastName, string email)
    {
        var factory = new GuestFactory();
        factory.guest = Guest.Create(firstName, lastName, email).Payload;
        return factory;
    }

    public static GuestFactory InitWithDefaultsValues()
    {
        var factory = new GuestFactory();
        factory.guest = Guest.Create(firstName, lastName, email).Payload;
        return factory;
    }
    
    public Guest Build() => guest;
}