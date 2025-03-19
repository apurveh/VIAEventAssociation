using VIAEventAssociation.Core.Domain.Aggregates.Guests;

namespace VIAEventAssociation.Core.Domain.Contracts;

public interface IGuestRepository
{
    bool EmailExists(string email);
    void AddGuest(Guest guest);
}