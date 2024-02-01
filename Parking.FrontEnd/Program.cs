using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Parking.FrontEnd.ClientService.Interface;
using Microsoft.AspNetCore.Components.Web;
using Parking.FrontEnd.ClientService;
using Parking.Domain.Config;
using Parking.FrontEnd;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

// Add Blazor Registered Services
builder.Services.AddBlazorBootstrap();

// Add Configurations
builder.Services.AddSingleton(new ClientConfiguration(
    builder.Configuration.GetValue<string>("API_URL") ?? string.Empty,
    builder.Configuration.GetValue<string>("SignalR_URL") ?? string.Empty
));

builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });
builder.Services.AddTransient<IGateClientService, GateClientService>();

await builder.Build().RunAsync();
