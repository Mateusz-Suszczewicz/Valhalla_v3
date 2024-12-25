using Blazored.LocalStorage;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Valhalla_v3.Client;
using Valhalla_v3.Client.Helpers;
using Valhalla_v3.Client.Service;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.Services.AddScoped(sp => new HttpClient());
builder.Services.AddScoped<ApiService>();

await builder.Build().RunAsync();

public static class Policies
{
    public const string IsAdmin = "IsAdmin";
    public const string IsUserLog = "IsUserLog";
    public const string IsUser = "IsUser";
    public const string IsClaim = "IsClaim";

    public static AuthorizationPolicy IsAdminPolicy()
    {
        return new AuthorizationPolicyBuilder().RequireAuthenticatedUser()

                                               .RequireRole("adminEdu")
                                               .Build();
    }

    public static AuthorizationPolicy IsUserLogged()
    {
        return new AuthorizationPolicyBuilder().RequireAuthenticatedUser()

                                               .Build();
    }

    public static AuthorizationPolicy IsClaimed()
    {
        return new AuthorizationPolicyBuilder().RequireAuthenticatedUser()
            .RequireClaim("MyCos", "MyValue")
            .Build();
    }

    public static AuthorizationPolicy IsUserPolicy()
    {
        return new AuthorizationPolicyBuilder().RequireAuthenticatedUser()
                                               .RequireRole("User")
                                               .Build();
    }
}