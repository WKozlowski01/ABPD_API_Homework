using WebApplication1.DTOs;
using WebApplication1.Models;
using WebApplication1.Repositories;

namespace WebApplication1.Services;

public class TripsService : ITripsService
{
    private readonly ITripsRepository _tripsrepository;
    
    public TripsService(ITripsRepository repository)
    {
        _tripsrepository = repository;
    }


    public async Task<IEnumerable<TripDto>> GetTripsAsync(CancellationToken cancellation)
    {
        var trips = await _tripsrepository.GetTripsAsync(cancellation);
        return trips;
    }

    public async Task<IEnumerable<ClientTripDto>> GetUsersTripsAsync( int id,CancellationToken cancelation )
    {
        var trips = await _tripsrepository.GetUsersTripsAsync(id,cancelation);
        if (trips == null || !trips.Any())
        {
            return Enumerable.Empty<ClientTripDto>();
        }
        return trips;
    }
}