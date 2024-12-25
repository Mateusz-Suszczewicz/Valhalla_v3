using System.Security.Claims;
using System.Text.Json;
using Blazored.LocalStorage;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.JSInterop;

namespace Valhalla_v3.Client;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;
using System.Threading.Tasks;

public class CustomAuthenticationStateProvider : AuthenticationStateProvider
{
    private readonly IHttpContextAccessor _httpContextAccessor;

    public CustomAuthenticationStateProvider(IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;
    }

    public override Task<AuthenticationState> GetAuthenticationStateAsync()
    {
        // 1. Odczytaj token z cookie
        var token = _httpContextAccessor.HttpContext?.Request.Cookies["AuthCookie"];
        //      ↑ użyj tu nazwy swojego ciasteczka JWT

        if (string.IsNullOrEmpty(token))
        {
            // Brak ciasteczka => niezalogowany użytkownik
            var anonymous = new ClaimsPrincipal(new ClaimsIdentity());
            return Task.FromResult(new AuthenticationState(anonymous));
        }

        try
        {
            // 2. Dekoduj JWT i stwórz ClaimsIdentity
            var claims = ParseClaimsFromJwt(token); // ta sama metoda, co w przypadku localStorage
            var identity = new ClaimsIdentity(claims, "jwt");
            var user = new ClaimsPrincipal(identity);

            // 3. Zwróć użytkownika
            return Task.FromResult(new AuthenticationState(user));
        }
        catch
        {
            // Token nieprawidłowy => traktuj jako niezalogowanego
            var anonymous = new ClaimsPrincipal(new ClaimsIdentity());
            return Task.FromResult(new AuthenticationState(anonymous));
        }
    }

    // Metoda do dekodowania JWT (taka sama jak wcześniej, np. w wersji minimalnej):
    private IEnumerable<Claim> ParseClaimsFromJwt(string token)
    {
        var claims = new List<Claim>();
        var payload = token.Split('.')[1];
        var jsonBytes = Convert.FromBase64String(payload);
        var keyValuePairs = JsonSerializer.Deserialize<Dictionary<string, object>>(jsonBytes);

        if (keyValuePairs != null)
        {
            foreach (var kvp in keyValuePairs)
            {
                claims.Add(new Claim(kvp.Key, kvp.Value.ToString()));
            }
        }

        return claims;
    }
    public void NotifyUserAuthentication(string token)
    {
        var identity = string.IsNullOrEmpty(token)
            ? new ClaimsIdentity()
            : new ClaimsIdentity(ParseClaimsFromJwt(token), "jwt");

        var user = new ClaimsPrincipal(identity);
        NotifyAuthenticationStateChanged(Task.FromResult(new AuthenticationState(user)));
    }
}

