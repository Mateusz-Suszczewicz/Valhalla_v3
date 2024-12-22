using Microsoft.AspNetCore.Components.WebAssembly.Hosting;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.Services.AddScoped(sp => new HttpClient());
builder.Services.AddScoped<ApiService>();

await builder.Build().RunAsync();
