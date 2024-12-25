using Azure;
using Blazored.LocalStorage;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Http;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using Valhalla_v3.Client;
using Valhalla_v3.Shared;

namespace Valhalla_v3.Client.Service;

public interface IBlazorAuthenticationService
{
    Task<bool> Authenticate(string email, string password);
}
public class BlazorAuthenticationService : IBlazorAuthenticationService
{
    private readonly AuthenticationStateProvider _authenticationStateProvider;
    protected readonly ILocalStorageService _localStorage;
    private readonly HttpClient _http;
    private readonly NavigationManager _navigationManager;
    private readonly IHttpContextAccessor _httpContextAccessor;
    public BlazorAuthenticationService(HttpClient httpClient, ILocalStorageService localStorage,
        AuthenticationStateProvider authenticationStateProvider, NavigationManager navigationManager, IHttpContextAccessor httpContextAccessor)
    {
        _authenticationStateProvider = authenticationStateProvider;
        _localStorage = localStorage;
        _http = httpClient;
        _navigationManager = navigationManager;
        _httpContextAccessor = httpContextAccessor;
    }

    public async Task<bool> Authenticate(string email, string password)
    {
        LoginDto login = new LoginDto() { Username = email, Password = password };
        try
        {
            try
            {
                var response = await _http.PostAsJsonAsync(_navigationManager.ToAbsoluteUri("api/auth/login"), login);
                if (response.IsSuccessStatusCode)
                {
                    var result = await response.Content.ReadFromJsonAsync<LoginResponse>();
                    var token = result.Token;
                    if (!string.IsNullOrEmpty(token))
                    {
                        var cookieOptions = new CookieOptions
                        {
                            HttpOnly = true,
                            Secure = true,
                            SameSite = SameSiteMode.Strict,
                            Expires = DateTimeOffset.UtcNow.AddHours(1)
                        };

                        _httpContextAccessor.HttpContext?.Response.Cookies.Append("AuthCookie", token, cookieOptions);
                        return true;
                    }

                }
                else
                {
                    return false;
                }
            }
            catch (Exception ex)
            {
                return false;
                throw ex;
            }
        }
        catch
        {
            return false;
        }
        return true;
    }

    private class LoginResponse
    {
        public string Token { get; set; }
    }
}
