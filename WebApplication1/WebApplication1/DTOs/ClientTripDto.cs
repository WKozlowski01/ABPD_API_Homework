namespace WebApplication1.DTOs;

public class ClientTripDto
{
    public int IdClient { get; set; }
    public int IdTrip { get; set; }
    public int RegisteredAt { get; set; }
    public int? PaymentDate { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
}