using VIAEventAssociation.Core.Domain.Aggregates.Guests;

namespace VIAEventAssociation.Core.Domain.Contracts;

public interface IGuestService
{
    bool EmailExists(string email);
    void AddGuest(Guest guest);
}