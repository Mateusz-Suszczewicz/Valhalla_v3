
using Microsoft.AspNetCore.Components.Authorization;
using System.Security.Claims;

namespace Valhalla_v3.Client.Service;
public class CustomAuthStateProvider : AuthenticationStateProvider
{
    public override Task<AuthenticationState> GetAuthenticationStateAsync()
    {
        // Pobierz token i zwróć odpowiedni stan
        var identity = new ClaimsIdentity();
        return Task.FromResult(new AuthenticationState(new ClaimsPrincipal(identity)));
    }

}
