using VIAEventAssociation.Core.Domain.Common.Bases;
using VIAEventAssociation.Core.Domain.Common.Values;
using VIAEventAssociation.Core.Tools.OperationResult;

namespace VIAEventAssociation.Core.Domain.Aggregates.Locations;

public class Location : AggregateRoot<LocationId>
{
    private Location(LocationId id, LocationName name, NumberOfGuests maxNumberOfGuests) : base(id)
    {
        LocationName = name;
        MaxNumberOfGuests = maxNumberOfGuests;
    }
    
    public LocationName LocationName { get; }
    public NumberOfGuests MaxNumberOfGuests { get; }

    public static Result<Location> Create(string name, int maxNumberOfGuests)
    {
        var errors = new HashSet<Error>();
        
        var locationIdResult = LocationId.GenerateId();
        if (locationIdResult.IsFailure)
            errors.Add(locationIdResult.Error);
        
        var locationNameResult = LocationName.Create(name);
        if (locationNameResult.IsFailure)
            errors.Add(locationNameResult.Error);
        
        var maxNumberOfGuestsResult = NumberOfGuests.Create(maxNumberOfGuests);
        if (maxNumberOfGuestsResult.IsFailure)
            errors.Add(maxNumberOfGuestsResult.Error);
        
        if (errors.Any())
            return Error.Add(errors);
        
        return new Location(locationIdResult.Payload, locationNameResult.Payload, maxNumberOfGuestsResult.Payload);
    }

    public override string ToString()
    {
        return $"Location: {LocationName} - MaxNumberOfGuests: {MaxNumberOfGuests}";
    }
}