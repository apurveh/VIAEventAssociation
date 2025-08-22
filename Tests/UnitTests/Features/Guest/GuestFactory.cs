using ViaEventAssociation.Core.Domain.Agregates.Guests;
using ViaEventAssociation.Core.Domain.Common.Values;

public class GuestFactory {
    private static readonly string _firstName = "John";
    private static readonly string _lastName = "Doe";
    private static readonly string _email = "JDO@via.dk";

    private Guest _guest;
    private GuestFactory() { }

    public static GuestFactory Init(string firstName, string lastName, string email) {
        var factory = new GuestFactory();
        var firstNameType = NameType.Create(firstName).Payload;
        var lastNameType = NameType.Create(lastName).Payload;
        var emailType = Email.Create(email).Payload;
        factory._guest = Guest.Create(firstNameType, lastNameType, emailType).Payload;
        return factory;
    }

    public static GuestFactory InitWithDefaultsValues() {
        var factory = new GuestFactory();
        var firstNameType = NameType.Create(_firstName).Payload;
        var lastNameType = NameType.Create(_lastName).Payload;
        var emailType = Email.Create(_email).Payload;

        factory._guest = Guest.Create(firstNameType, lastNameType, emailType).Payload;
        return factory;
    }

    public Guest Build() {
        return _guest;
    }
}