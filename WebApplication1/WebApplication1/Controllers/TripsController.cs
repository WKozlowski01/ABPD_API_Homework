using Microsoft.AspNetCore.Mvc;

using WebApplication1.Repositories;
using WebApplication1.Services;

namespace WebApplication1.Controllers;



[ApiController]
[Route("api/[controller]")]
public class TripsController:ControllerBase
{

    private readonly ITripsService _tripsService;

    public TripsController(ITripsService service,ITripsRepository repository)
    {
     _tripsService = service;   
    }


    [HttpGet]
    public async Task<IActionResult> GetTripsAsync( CancellationToken cancellationToken)
    {
      
        var trips = await _tripsService.GetTripsAsync(cancellationToken);
        return Ok(trips);
    }

}