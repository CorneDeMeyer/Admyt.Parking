using Microsoft.AspNetCore.Builder;
using Parking.DomainLogic.Hubs;
using Parking.Repository.Interface;
using Parking.Repository.Repository;
using Parking.Repository.Repository.Base;

var builder = WebApplication.CreateBuilder(args);

// Connection String
var connectionString = builder.Configuration.GetConnectionString("MyDatabaseConnection");

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddSignalR();

var app = builder.Build();

// Setup Repository
builder.Services.AddSingleton(new RepositoryConfiguration(connectionString));
builder.Services.AddTransient<IParkingSessionRepository, ParkingSessionRepository>();
builder.Services.AddTransient<IGateRepository, GateRepository>();
builder.Services.AddTransient<IGateEventRepository, GateEventRepository>();
builder.Services.AddTransient<IZoneRepository, ZoneRepository>();

// Setup Services


// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAntiforgery();

// Map Signal R hub
app.MapHub<CommunicationHub>("/commands");

app.UseAuthorization();
app.MapControllers();

app.Run();
