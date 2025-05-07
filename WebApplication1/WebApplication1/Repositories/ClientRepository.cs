using System.Data.SqlClient;
using WebApplication1.DTOs;

namespace WebApplication1.Repositories;

public class ClientRepository:IClientRepository
{
    
    
    private readonly string _conectionString;
    
    public ClientRepository(IConfiguration configuration)
    {
        _conectionString = configuration.GetConnectionString("DefaultConnection");
    }
    
    
    public async Task<int> CreateClientAsync(ClientDTO client, CancellationToken cancelation)
    {
        await using var con = new SqlConnection(_conectionString);
        await using var com = new SqlCommand();
        com.Connection = con;
        
        
        com.Parameters.AddWithValue("@FirstName", client.FirstName);
        com.Parameters.AddWithValue("@LastName", client.LastName);
        com.Parameters.AddWithValue("@Email", client.Email);
        com.Parameters.AddWithValue("@Telephone", client.Telephone);
        com.Parameters.AddWithValue("@Pesel", client.Pesel);
        

        com.CommandText = "INSERT INTO Client (FirstName, LastName, Email, Telephone, Pesel) VALUES (@FirstName, @LastName, @Email, @Telephone, @Pesel);SELECT IdClient FROM Client WHERE FirstName = @FirstName AND LastName = @LastName AND Email = @Email AND Telephone = @Telephone AND Pesel = @Pesel;";

        await con.OpenAsync(cancelation);
        
        int IdClient = 0;

        SqlDataReader reader = await com.ExecuteReaderAsync(cancelation);
        while (await reader.ReadAsync(cancelation))
        {

            IdClient = (int)reader["IdClient"];
        }

        return IdClient;
    }

    public async Task<bool> PutClientsTripAsync(int id, int tripId, CancellationToken cancellationToken)
    {
        await using var con = new SqlConnection(_conectionString);
        await using var com = new SqlCommand();
        com.Connection = con;

        com.Parameters.AddWithValue("@IdClient", id);
        com.Parameters.AddWithValue("@tripId", tripId);

        DateTime thisDay = DateTime.Today;
        string yeas = thisDay.Year.ToString();
        string days = thisDay.Day.ToString();
        string months = thisDay.Month.ToString();

        string newDate = yeas + months + days;
        int newDateInt = int.Parse(newDate);

        await con.OpenAsync(cancellationToken);

      
        com.CommandText = "SELECT COUNT(*) FROM Client_Trip WHERE IdClient = @IdClient AND IdTrip = @tripId";
        var exists = (int)(await com.ExecuteScalarAsync(cancellationToken));
        if (exists > 0)
        {
            throw new InvalidOperationException("Klient jest już zapisany na tę wycieczkę.");
        }

       
        com.CommandText = @"
        INSERT INTO Client_TRIP (IdClient, IdTrip, RegisteredAt) 
        VALUES (@IdClient, @tripId, @newDate);";
        com.Parameters.AddWithValue("@newDate", newDateInt);

        try
        {
            int affected = await com.ExecuteNonQueryAsync(cancellationToken);
            return affected > 0;
        }
        catch (SqlException e)
        {
            throw new InvalidOperationException("Nie udało się zapisać klienta na wycieczkę", e);
        }
    }
    
    public async Task<bool> ClientExistsAsync(int clientId, CancellationToken cancellationToken)
    {
    
        
        await using var con = new SqlConnection(_conectionString);
        await using var com = new SqlCommand();
        
        com.Connection = con;
        com.CommandText = "SELECT 1 FROM Client WHERE IdClient = @id";
        
        
        com.Parameters.AddWithValue("@id", clientId);
        await con.OpenAsync(cancellationToken);
        var result = await com.ExecuteScalarAsync(cancellationToken);
        return result != null;
    }
    
    public async Task<bool> TripExistsAsync(int IdTrip, CancellationToken cancellationToken)
    {
    
        
        await using var con = new SqlConnection(_conectionString);
        await using var com = new SqlCommand();
        
        com.Connection = con;
        com.CommandText = "SELECT 1 FROM Trip WHERE IdTrip = @id";
        
        
        com.Parameters.AddWithValue("@id", IdTrip);
        await con.OpenAsync(cancellationToken);
        var result = await com.ExecuteScalarAsync(cancellationToken);
        return result != null;
    }
    
    public async Task<bool> QuantityCheckAsync(int IdTrip, CancellationToken cancellationToken)
    {
        await using var con = new SqlConnection(_conectionString);
        await using var com = new SqlCommand();

        com.Connection = con;
        com.CommandText = "SELECT MaxPeople FROM Trip WHERE IdTrip = @id";

        int maxPeople = 0;
        int currentPeople = 0;

        com.Parameters.AddWithValue("@id", IdTrip);
        await con.OpenAsync(cancellationToken);

        var result = await com.ExecuteScalarAsync(cancellationToken); //tylko jedna wartosc jest zwracana 
        if (result == null)
        {
            throw new Exception("Trip Not Found");
        }
        maxPeople = (int)result;

        await using var com2 = new SqlCommand();
        com2.Connection = con;
        com2.CommandText = "SELECT COUNT(IdClient) FROM Client_Trip WHERE IdTrip = @id";
        com2.Parameters.AddWithValue("@id", IdTrip);

        var result2 = await com2.ExecuteScalarAsync(cancellationToken);
        currentPeople = (int)result2;

        return currentPeople < maxPeople;
    }

    public async Task<bool> RecordExistsAsync(int clientId, int tripId, CancellationToken cancellationToken)
    {
        await using var con = new SqlConnection(_conectionString);
        await using var com = new SqlCommand();

        com.Connection = con;
        com.CommandText = "SELECT 1 FROM Client_Trip WHERE IdTrip = @Tripid AND IdClient = @clientID";

        com.Parameters.AddWithValue("@Tripid", tripId);
        com.Parameters.AddWithValue("@clientID", clientId); 

        await con.OpenAsync(cancellationToken);
        var result = await com.ExecuteScalarAsync(cancellationToken);
        return result != null;
    }

    public async Task<bool> DeleteClientFromTripAsync(int clientId, int tripId, CancellationToken cancellationToken)
    {
        await using var con = new SqlConnection(_conectionString);
        await using var com = new SqlCommand();

        com.Connection = con;
        com.CommandText = "DELETE FROM Client_Trip WHERE IdTrip = @Tripid AND IdClient = @clientID";

        com.Parameters.AddWithValue("@Tripid", tripId);
        com.Parameters.AddWithValue("@clientID", clientId); 

        await con.OpenAsync(cancellationToken);

        try
        {
            int affected = await com.ExecuteNonQueryAsync(cancellationToken);
            return affected > 0;
        }
        catch (SqlException e)
        {
            throw new InvalidOperationException("Nie udało się usunąć rejestracji klienta z wycieczki", e);
        }
    }
}