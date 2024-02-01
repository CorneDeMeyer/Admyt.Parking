using Parking.Repository.Repository.Base;
using Parking.DomainLogic.ServiceBus;
using Parking.DomainLogic.Interface;
using Parking.Repository.Repository;
using Parking.Repository.Interface;
using Parking.DomainLogic.Service;
using Parking.DomainLogic.Hubs;
using Parking.Domain.Config;

var builder = WebApplication.CreateBuilder(args);

// Connection String
var connectionString = builder.Configuration.GetConnectionString("MyDatabaseConnection");
var azureServiceBusConfig = builder.Configuration.GetValue<AzureServiceBusConfiguration>("AzureServiceBusConfig");

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddSignalR();

// Setup Repository
builder.Services.AddSingleton(new RepositoryConfiguration(connectionString));
builder.Services.AddTransient<IParkingSessionRepository, ParkingSessionRepository>();
builder.Services.AddTransient<IGateRepository, GateRepository>();
builder.Services.AddTransient<IGateEventRepository, GateEventRepository>();
builder.Services.AddTransient<IZoneRepository, ZoneRepository>();

// Setup Services
builder.Services.AddTransient<IGateService, GateService>();

// Background Service
if (azureServiceBusConfig != null && azureServiceBusConfig.Enabled)
{
    builder.Services.AddHostedService<GateEventSubscriptionBackgroundService>();
}

// CORS Service
builder.Services.AddCors(options => {
    options.AddPolicy("AllowAnyCorsPolicy", builder => 
         builder
            .AllowAnyOrigin()
            .AllowAnyHeader()
            .AllowAnyMethod());
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors("AllowAnyCorsPolicy");

app.UseHttpsRedirection();
 
// Map Signal R hub
app.MapHub<CommunicationHub>("/commands");

app.UseAuthorization();
app.MapControllers();

app.Run();
