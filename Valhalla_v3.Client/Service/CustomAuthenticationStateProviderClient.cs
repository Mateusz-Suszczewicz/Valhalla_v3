namespace Valhalla_v3.Client;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.JSInterop;
using System.Security.Claims;
using System.Text.Json;

public class CustomAuthenticationStateProviderClient : AuthenticationStateProvider
{
    private readonly IJSRuntime _jsRuntime;
    private const string TokenKey = "authToken";

    public CustomAuthenticationStateProviderClient(IJSRuntime jsRuntime)
    {
        _jsRuntime = jsRuntime;
    }

    public override async Task<AuthenticationState> GetAuthenticationStateAsync()
    {
        // Odczyt tokena z localStorage
        var token = await _jsRuntime.InvokeAsync<string>("localStorage.getItem", TokenKey);

        if (string.IsNullOrEmpty(token))
        {
            // Brak tokena -> użytkownik anonimowy
            var anonymous = new ClaimsPrincipal(new ClaimsIdentity());
            return new AuthenticationState(anonymous);
        }

        try
        {
            var claims = ParseClaimsFromJwt(token);
            var identity = new ClaimsIdentity(claims, "jwt");
            var user = new ClaimsPrincipal(identity);
            return new AuthenticationState(user);
        }
        catch
        {
            // W razie problemów z parsowaniem -> anonim
            var anonymous = new ClaimsPrincipal(new ClaimsIdentity());
            return new AuthenticationState(anonymous);
        }
    }

    /// <summary>
    /// Metoda do ustawiania/aktualizowania tokena w Local Storage.
    /// </summary>
    public async Task SetTokenAsync(string token)
    {
        if (string.IsNullOrEmpty(token))
        {
            // Usuwamy token z localStorage (wylogowanie)
            await _jsRuntime.InvokeVoidAsync("localStorage.removeItem", TokenKey);
        }
        else
        {
            // Zapisujemy token w localStorage
            await _jsRuntime.InvokeVoidAsync("localStorage.setItem", TokenKey, token);
        }

        // Powiadamiamy Blazor, że stan autoryzacji się zmienił
        NotifyUserAuthentication(token);
    }

    /// <summary>
    /// Metoda do aktualizacji stanu uwierzytelnienia wewnątrz provider'a.
    /// </summary>
    private void NotifyUserAuthentication(string token)
    {
        var identity = string.IsNullOrEmpty(token)
            ? new ClaimsIdentity()
            : new ClaimsIdentity(ParseClaimsFromJwt(token), "jwt");

        var user = new ClaimsPrincipal(identity);
        NotifyAuthenticationStateChanged(Task.FromResult(new AuthenticationState(user)));
    }

    /// <summary>
    /// Parsowanie payloadu JWT do listy obiektów Claim.
    /// </summary>
    private IEnumerable<Claim> ParseClaimsFromJwt(string token)
    {
        var claims = new List<Claim>();

        // Część payloadu (środkowa część tokena)
        var payload = token.Split('.')[1];

        // Niekiedy trzeba dopasować Base64 (uzupełnić '='), jeśli długość nie jest wielokrotnością 4
        payload = PadBase64(payload);

        var jsonBytes = Convert.FromBase64String(payload);
        var keyValuePairs = JsonSerializer.Deserialize<Dictionary<string, object>>(jsonBytes);

        if (keyValuePairs != null)
        {
            foreach (var kvp in keyValuePairs)
            {
                // Np. "name", "role", itp.
                claims.Add(new Claim(kvp.Key, kvp.Value?.ToString() ?? ""));
            }
        }

        return claims;
    }

    private static string PadBase64(string base64)
    {
        // Uzupełnianie '=', bo czasem token nie ma paddingu
        while (base64.Length % 4 != 0)
        {
            base64 += '=';
        }
        return base64;
    }
}
