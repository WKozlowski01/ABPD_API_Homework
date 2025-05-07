using Microsoft.AspNetCore.Mvc;
using WebApplication1.DTOs;


namespace WebApplication1.Services;

public interface ITripsService
{
    Task<IEnumerable<TripDto>> GetTripsAsync(CancellationToken cancellationToken);
    Task<IEnumerable<ClientTripDto>> GetUsersTripsAsync(int id, CancellationToken cancelation);
    

}