using Microsoft.AspNetCore.Mvc;
using WebApplication1.DTOs;


namespace WebApplication1.Repositories;

public interface ITripsRepository
{
    Task<IEnumerable<TripDto>> GetTripsAsync(CancellationToken cancelation);
    Task<IEnumerable<ClientTripDto>> GetUsersTripsAsync(int id,CancellationToken cancellationToken );
    
    

}