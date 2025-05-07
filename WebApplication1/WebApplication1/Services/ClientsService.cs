using WebApplication1.DTOs;
using WebApplication1.Repositories;

namespace WebApplication1.Services;

public class ClientsService : ICliensService
{
    private readonly IClientRepository _clientRepository;

    public ClientsService(IClientRepository repository)
    {
        _clientRepository = repository;
    }

    public async Task<int> CreaateClientAsync(ClientDTO client, CancellationToken cancelation)
    {
        var clients = await _clientRepository.CreateClientAsync(client, cancelation);
        return clients;
    }

    public async Task<(bool Success, string Error)> RegisterClientToTripAsync(int clientId, int tripId,
        CancellationToken cancellationToken)
    {

        var clientExists = await _clientRepository.ClientExistsAsync(clientId, cancellationToken);
        if (!clientExists)
            return (false, "Client not found.");


        var tripExists = await _clientRepository.TripExistsAsync(tripId, cancellationToken);
        if (!tripExists)
            return (false, "Trip not found.");



        var registered = await _clientRepository.PutClientsTripAsync(clientId, tripId, cancellationToken);
        if (!registered)
            return (false, "Failed to register client.");

        return (true, null);
    }

    public async Task<(bool Success, string Error)> DeleteClientFromTripAsync(int clientId, int tripId,
        CancellationToken cancellationToken)
    {
        var recordExists = await _clientRepository.RecordExistsAsync(clientId, tripId, cancellationToken);
        if (!recordExists)
        {
            return (false, "Client is not registered for this trip.");
        }

        
        var deleted = await _clientRepository.DeleteClientFromTripAsync(clientId, tripId, cancellationToken);
        if (!deleted)
        {
            return (false, "Failed to remove client from trip.");
        }

        return (true, null);
    }
}