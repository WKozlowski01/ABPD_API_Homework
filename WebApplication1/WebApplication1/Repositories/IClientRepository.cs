using WebApplication1.DTOs;

namespace WebApplication1.Repositories;

public interface IClientRepository
{
    Task<int> CreateClientAsync(ClientDTO client, CancellationToken cancelation);
    Task<bool> PutClientsTripAsync(int id, int tripId, CancellationToken cancellationToken);
    Task<bool> ClientExistsAsync(int clientId, CancellationToken cancellationToken);
    Task<bool> TripExistsAsync(int IdTrip, CancellationToken cancellationToken);
    Task<bool> QuantityCheckAsync(int IdTrip, CancellationToken cancellationToken);
    
    Task<bool> RecordExistsAsync(int clientId, int tripId, CancellationToken cancellationToken);
    
    Task<bool> DeleteClientFromTripAsync(int clientId, int tripId, CancellationToken cancellationToken);

}