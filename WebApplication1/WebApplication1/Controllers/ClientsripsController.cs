using Microsoft.AspNetCore.Mvc;
using WebApplication1.Repositories;
using WebApplication1.Services;

namespace WebApplication1.Controllers;


    [ApiController]
    [Route("api/clients")]
    public class ClientsripsController:ControllerBase
    {

        private readonly ITripsService _tripsService;

        public ClientsripsController(ITripsService service,ITripsRepository repository)
        {
            _tripsService = service;   
        }


        [HttpGet("{id}/trips")]
        public async Task<IActionResult> GetUsersTripsAsync(int id,CancellationToken cancellationToken )
        {
      
            var trips = await _tripsService.GetUsersTripsAsync( id, cancellationToken);
            if (!trips.Any())
            {
                return NotFound("Client not found or has no trips.");
            }
            return Ok(trips);
        }

    }
