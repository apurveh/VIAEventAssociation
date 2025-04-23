using VIAEventAssociation.Core.Domain.Common;

namespace VIAEventAssociation.Core.Domain.Aggregates.Locations;

public interface ILocationRepository : IRepository<Location, LocationId>
{
    Task<IEnumerable<Location>> GetAvailableLocationsAsync(DateOnly date, TimeOnly startTime, TimeOnly endTime);
    Task<IEnumerable<Location>> GetLocationsByMinimumCapacityAsync(int requiredCapacity);
}
