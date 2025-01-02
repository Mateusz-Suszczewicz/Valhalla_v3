using Microsoft.JSInterop;
using System.Net.Http.Json;
using Valhalla_v3.Shared;

namespace Valhalla_v3.Client.Pages.Admin;

public partial class Login
{
    private LoginDto loginModel = new();
    private string? errorMessage;


    private async Task HandleLogin()
    {
        try
        {
            var response = await Http.PostAsJsonAsync(navigation.ToAbsoluteUri("api/auth/login"), loginModel);
            if (response.IsSuccessStatusCode)
            {
                var result = await response.Content.ReadFromJsonAsync<LoginResponse>();
                var token = result?.Token;
                if (!string.IsNullOrEmpty(token))
                {
                    await JSRuntime.InvokeVoidAsync("localStorage.setItem", "authToken", token);
                }
                navigation.NavigateTo("/");
            }
        }
        catch (Exception ex)
        {
            errorMessage = ex.Message;
        }
       
    }

    private class LoginResponse
    {
        public string Token { get; set; }
    }
}
