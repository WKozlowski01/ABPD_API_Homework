using System.Data.SqlClient;
using System.Transactions;
using WebApplication1.DTOs;
using WebApplication1.Models;


namespace WebApplication1.Repositories;

public class TripsRepository: ITripsRepository
{
    private readonly string _conectionString;
    
    public TripsRepository(IConfiguration configuration)
    {
        _conectionString = configuration.GetConnectionString("DefaultConnection");
    }
    
    public async Task<IEnumerable<TripDto>> GetTripsAsync(CancellationToken cancelation)
    {
        await using var con = new SqlConnection(_conectionString);
        await using var com = new SqlCommand();
        com.Connection = con;

        com.CommandText = @"
        SELECT 
            t.IdTrip,
            t.Name,
            t.Description,
            t.DateFrom,
            t.DateTo,
            t.MaxPeople,
            STRING_AGG(c.Name, ', ') AS Countries
        FROM Trip t
        JOIN Country_Trip ct ON t.IdTrip = ct.IdTrip
        JOIN Country c ON ct.IdCountry = c.IdCountry
        GROUP BY 
            t.IdTrip, t.Name, t.Description, t.DateFrom, t.DateTo, t.MaxPeople
        ORDER BY t.IdTrip;
    ";

        await con.OpenAsync(cancelation);

        SqlDataReader reader = await com.ExecuteReaderAsync(cancelation);

        var trips = new List<TripDto>();

        while (await reader.ReadAsync(cancelation))
        {
            int IdTrip = (int)reader["IdTrip"];
            string Name = (string)reader["Name"];
            string Description = reader["Description"] as string ?? string.Empty;
            DateTime DateFrom = (DateTime)reader["DateFrom"];
            DateTime DateTo = (DateTime)reader["DateTo"];
            int MaxPeople = (int)reader["MaxPeople"];
            string Countries = reader["Countries"] as string ?? string.Empty;

            var trip = new TripDto()
            {
                IdTrip = IdTrip,
                Name = Name,
                Description = Description,
                DateFrom = DateFrom,
                DateTo = DateTo,
                MaxPeople = MaxPeople,
                Country = Countries
            };

            trips.Add(trip);
        }

        return trips;
    }

    public async Task<IEnumerable<ClientTripDto>> GetUsersTripsAsync(int id,CancellationToken cancellationToken )
    {
        await using var con = new SqlConnection(_conectionString);
        await using var com = new SqlCommand();
        com.Connection = con;
        
        
        com.CommandText = @"
            SELECT DISTINCT IdClient,Client_Trip.IdTrip,RegisteredAt,PaymentDate,Trip.Name,Trip.Description  
            FROM Client_Trip 
            Join Trip on Trip.IdTrip = Client_Trip.IdTrip 
            Join Country_Trip on Country_Trip.IdTrip = Trip.IdTrip
            Join Country on Country_Trip.IdCountry = Country.IdCountry
            WHERE IdClient = @IdClient
            ";
        com.Parameters.AddWithValue("@IdClient", id);
        
        await con.OpenAsync(cancellationToken);

        SqlDataReader reader = await com.ExecuteReaderAsync(cancellationToken);

        var trips = new List<ClientTripDto>();

        while (await reader.ReadAsync(cancellationToken))
        {
            var client_Trip = new ClientTripDto()
            {
                IdClient = reader.GetInt32(reader.GetOrdinal("IdClient")),
                IdTrip = reader.GetInt32(reader.GetOrdinal("IdTrip")),
                RegisteredAt = reader.GetInt32(reader.GetOrdinal("RegisteredAt")),
                PaymentDate = reader.IsDBNull(reader.GetOrdinal("PaymentDate"))
                    ? 0
                    : reader.GetInt32(reader.GetOrdinal("PaymentDate")),
                Name = reader.IsDBNull(reader.GetOrdinal("Name"))
                    ? string.Empty
                    : reader.GetString(reader.GetOrdinal("Name")),
                Description = reader.IsDBNull(reader.GetOrdinal("Description"))
                    ? string.Empty
                    : reader.GetString(reader.GetOrdinal("Description"))
            };
            

            trips.Add(client_Trip);
        }

        return trips;
    }
}