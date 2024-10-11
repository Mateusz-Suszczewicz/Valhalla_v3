using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Valhalla_v3.Client.Services;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.Services.AddScoped<ICarClient, CarClient>();

//await builder.Build().RunAsync();
