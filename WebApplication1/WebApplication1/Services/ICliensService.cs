using WebApplication1.DTOs;


namespace WebApplication1.Services;

public interface ICliensService
{
    Task<int> CreaateClientAsync(ClientDTO client, CancellationToken cancelation);

    Task<(bool Success, string Error)> RegisterClientToTripAsync(int clientId, int tripId,
        CancellationToken cancellationToken);
    
    Task<(bool Success, string Error)> DeleteClientFromTripAsync(int clientId, int tripId, CancellationToken cancellationToken);
    



}