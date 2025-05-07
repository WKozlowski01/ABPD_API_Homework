using Microsoft.AspNetCore.Mvc;
using WebApplication1.DTOs;
using WebApplication1.Services;

namespace WebApplication1.Controllers;


    [ApiController]
    [Route("api/clients")]
    public class ClientsripsController:ControllerBase
    {

        private readonly ITripsService _tripsService;
        private readonly ICliensService _clientsService;

        public ClientsripsController(ITripsService tripsService, ICliensService clientsService)
        {
            _tripsService = tripsService;
            _clientsService = clientsService;
        }

        [HttpPost]
        public async Task<IActionResult> AddClientAsync(ClientDTO client, CancellationToken cancelation)
        {
            var clients = await _clientsService.CreaateClientAsync(client, cancelation);
            return Ok(clients);
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

        [HttpPut("{id}/trips/{tripId}")]
        public async Task<IActionResult> GetClientTripAsync(int id, int tripId, CancellationToken cancellationToken)
        {
            var (success, error) = await _clientsService.RegisterClientToTripAsync(id, tripId, cancellationToken);
    
            if (!success)
            {
                return error switch
                {
                    "Client not found." => NotFound(error),
                    "Trip not found." => NotFound(error),
                    "Client already registered for this trip." => Conflict(error),
                    _ => BadRequest(error)
                };
            }

            return NoContent();
        }

        [HttpDelete("{id}/trips/{tripId}")]
        public async Task<IActionResult> DeleteClientFromTrip(int id, int tripId, CancellationToken cancellationToken)
        {
            var result = await _clientsService.DeleteClientFromTripAsync(id, tripId, cancellationToken);

            if (!result.Success)
            {

                if (result.Error == "Client is not registered for this trip.")
                {
                    return NotFound(result.Error);
                }
            }

            return NoContent(); 
        }
    }
