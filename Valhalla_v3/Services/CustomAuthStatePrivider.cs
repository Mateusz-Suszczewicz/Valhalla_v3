using System.Text.Json;

namespace Valhalla_v3.Services;
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
        var token = _httpContextAccessor.HttpContext?.Request.Cookies["AuthCookie"];

        if (string.IsNullOrEmpty(token))
        {
            var anonymous = new ClaimsPrincipal(new ClaimsIdentity());
            return Task.FromResult(new AuthenticationState(anonymous));
        }

        try
        {
            var claims = ParseClaimsFromJwt(token);
            var identity = new ClaimsIdentity(claims, "jwt");
            var user = new ClaimsPrincipal(identity);
            return Task.FromResult(new AuthenticationState(user));
        }
        catch
        {
            var anonymous = new ClaimsPrincipal(new ClaimsIdentity());
            return Task.FromResult(new AuthenticationState(anonymous));
        }
    }

    private IEnumerable<Claim> ParseClaimsFromJwt(string token)
    {
        var claims = new List<Claim>();
        var payload = token.Split('.')[1];
        var jsonBytes = Base64UrlDecode(payload);
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

    public static byte[] Base64UrlDecode(string base64Url)
    {
        // 1. Zamiana znaków URL na Base64
        var output = base64Url
            .Replace('-', '+')
            .Replace('_', '/');

        // 2. Dodanie brakujących znaków '='
        switch (output.Length % 4)
        {
            case 2: output += "=="; break;
            case 3: output += "="; break;
            case 0: /* nic nie robić */ break;
            default:
                // 1 lub inne - jest źle uformowany
                throw new ArgumentException("Invalid base64url string!");
        }

        return Convert.FromBase64String(output);
    }

}

