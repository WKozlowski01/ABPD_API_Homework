using WebApplication1.Repositories;
using WebApplication1.Services;

namespace WebApplication1;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        // Add services to the container.
        builder.Services.AddAuthorization();

        // Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
        builder.Services.AddOpenApi();
        builder.Services.AddControllers();

        builder.Services.AddScoped<ITripsService, TripsService>();
        builder.Services.AddScoped<ITripsRepository, TripsRepository>();
        builder.Services.AddScoped<ICliensService, ClientsService>();
        builder.Services.AddScoped<IClientRepository, ClientRepository>();

        var app = builder.Build();

        // Configure the HTTP request pipeline.
        if (app.Environment.IsDevelopment())
        {
            app.MapOpenApi();
        }

        app.UseHttpsRedirection();

        app.UseAuthorization();
        
        app.MapControllers();

       

        app.Run();
    }
}