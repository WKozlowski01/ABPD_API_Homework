using WebApplication1.DTOs;
using WebApplication1.Models;

namespace WebApplication1.Services;

public interface ITripsService
{
    Task<IEnumerable<TripDto>> GetTripsAsync(CancellationToken cancellationToken);
    Task<IEnumerable<ClientTripDto>> GetUsersTripsAsync(int id, CancellationToken cancelation);
}