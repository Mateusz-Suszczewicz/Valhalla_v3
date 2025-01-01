using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Valhalla_v3.Client;
using Valhalla_v3.Services;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.Services.AddScoped(sp => new HttpClient());
builder.Services.AddScoped<ApiService>();
builder.Services.AddScoped<AuthenticationStateProvider,
    CustomAuthenticationStateProviderClient>();


builder.Services.AddAuthorizationCore(config =>
{
    config.AddPolicy(Policies.IsAdmin, Policies.IsUserLogged());
    config.AddPolicy(Policies.IsUserLog, Policies.IsUserLogged());
    config.AddPolicy(Policies.IsUser, Policies.IsUserPolicy());
    config.AddPolicy(Policies.IsClaim, Policies.IsClaimed());
});

await builder.Build().RunAsync();
