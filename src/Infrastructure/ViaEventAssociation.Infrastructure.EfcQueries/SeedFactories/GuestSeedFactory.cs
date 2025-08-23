using System.Text.Json;

namespace ViaEventAssociation.Infrastructure.EfcQueries.SeedFactories;

public class GuestSeedFactory 
{
    public static List<Guest> CreateGuest() 
    {
        string jsonString = File.ReadAllText(@"C:\Users\apurv\RiderProjects\VIAEventAssociation\src\Infrastructure\ViaEventAssociation.Infrastructure.EfcQueries\SeedFactories\Json\Guests.json");

        List<TmpGuest> tmpGuests = JsonSerializer.Deserialize<List<TmpGuest>>(jsonString)!;

        var guests = tmpGuests.Select(g => 
            new Guest 
            {
                Id = g.Id,
                FirstName = g.FirstName,
                LastName = g.LastName,
                Email = g.Email,
            }
        ).ToList();

        return guests;
    }

    public record TmpGuest(string Id, string FirstName, string LastName, string Email, string Url);
}