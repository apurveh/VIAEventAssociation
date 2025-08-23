using System.Text.Json;

namespace ViaEventAssociation.Infrastructure.EfcQueries.SeedFactories;

public class EventSeedFactory 
{
    public static List<Event> CreateEvents() 
    {
        string jsonString = File.ReadAllText(@"C:\Users\apurv\RiderProjects\VIAEventAssociation\src\Infrastructure\ViaEventAssociation.Infrastructure.EfcQueries\SeedFactories\Json\Events.json");

        List<TmpEvent> tmpEvents = JsonSerializer.Deserialize<List<TmpEvent>>(jsonString)!;

        var events = tmpEvents.Select(e => 
            new Event 
            {
                Id = e.Id,
                Title = e.Title,
                Description = e.Description,
                EventStart = DateTime.Parse(e.Start),
                EventEnd = DateTime.Parse(e.End),
                Visibility = e.Visibility,
                Status = e.Status,
                NumberOfGuests = e.MaxGuests,
            }
        ).ToList();

        return events;
    }

    public record TmpEvent
    (
        string Id,
        string Title,
        string Description,
        string Status,
        string Visibility,
        string Start,
        string End,
        string LocationId,
        int MaxGuests
    );
}