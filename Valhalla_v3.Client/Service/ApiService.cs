using System.Net.Http.Json;
using System.Text.Json;
using System.Text;
using Valhalla_v3.Shared.CarHistory;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.AspNetCore.Components;

public class ApiService
{
    private int operId = 3;
    private readonly HttpClient _httpClient;
    private readonly NavigationManager navigation;

    public ApiService(HttpClient httpClient, NavigationManager navigationManager)
    {
        _httpClient = httpClient;
        navigation = navigationManager;
    }

    /// <summary>
    /// Pobiera listę danych z API.
    /// </summary>
    public async Task<(List<T>?, string)> GetListAsync<T>(string endpoint)
    {
        try
        {
            var response = await _httpClient.GetAsync(navigation.ToAbsoluteUri(endpoint));
            switch (response.StatusCode)
            {
                case System.Net.HttpStatusCode.OK:
                    var data = await response.Content.ReadFromJsonAsync<List<T>>();
                    return (data ?? new List<T>(), string.Empty);

                case System.Net.HttpStatusCode.NoContent:
                    return (new List<T>(), string.Empty);

                default:
                    var error = await response.Content.ReadAsStringAsync();
                    return (null, $"Błąd API: {response.StatusCode} - {error}");
            }
        }
        catch (Exception ex)
        {
            return (null, $"Błąd aplikacji: {ex.Message}");
        }
    }

    /// <summary>
    /// Pobiera pojedynczy obiekt z API.
    /// </summary>
    public async Task<(T?, string)> GetAsync<T>(string endpoint)
    {
        try
        {
            var response = await _httpClient.GetAsync(navigation.ToAbsoluteUri(endpoint));
            if (response.IsSuccessStatusCode)
            {
                var data = await response.Content.ReadFromJsonAsync<T>();
                return (data, string.Empty);
            }
            var error = await response.Content.ReadAsStringAsync();
            return (default, $"Błąd API: {response.StatusCode} - {error}");
        }
        catch (Exception ex)
        {
            return (default, $"Błąd aplikacji: {ex.Message}");
        }
    }

    /// <summary>
    /// Tworzy nowy obiekt w API.
    /// </summary>
    public async Task<(bool, string)> PostAsync<T>(string endpoint, T obj)
    {
        obj.GetType().GetProperty("OperatorCreateId")?.SetValue(obj, operId);
        obj.GetType().GetProperty("OperatorModifyId")?.SetValue(obj, operId);
        obj.GetType().GetProperty("DateTimeAdd")?.SetValue(obj, DateTime.Now);
        obj.GetType().GetProperty("DateTimeModify")?.SetValue(obj, DateTime.Now);
        try
        {
            var json = JsonSerializer.Serialize(obj);
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            var response = await _httpClient.PostAsync(navigation.ToAbsoluteUri(endpoint), content);
            if (response.IsSuccessStatusCode)
            {
                return (true, string.Empty);
            }
            var error = await response.Content.ReadAsStringAsync();
            return (false, $"Błąd API: {response.StatusCode} - {error}");
        }
        catch (Exception ex)
        {
            return (false, $"Błąd aplikacji: {ex.Message}");
        }
    }

    /// <summary>
    /// Aktualizuje istniejący obiekt w API.
    /// </summary>
    public async Task<(bool, string)> PutAsync<T>(string endpoint, T obj)
    {
        obj.GetType().GetProperty("OperatorCreateId")?.SetValue(obj, operId);
        obj.GetType().GetProperty("OperatorModifyId")?.SetValue(obj, operId);
        obj.GetType().GetProperty("DateTimeModify")?.SetValue(obj, DateTime.Now);
        try
        {
            var json = JsonSerializer.Serialize(obj);
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            var response = await _httpClient.PutAsync(navigation.ToAbsoluteUri(endpoint), content);
            if (response.IsSuccessStatusCode)
            {
                return (true, string.Empty);
            }
            var error = await response.Content.ReadAsStringAsync();
            return (false, $"Błąd API: {response.StatusCode} - {error}");
        }
        catch (Exception ex)
        {
            return (false, $"Błąd aplikacji: {ex.Message}");
        }
    }

    /// <summary>
    /// Usuwa obiekt z API.
    /// </summary>
    public async Task<(bool, string)> DeleteAsync(string endpoint)
    {
        try
        {
            var response = await _httpClient.DeleteAsync(navigation.ToAbsoluteUri(endpoint));
            if (response.IsSuccessStatusCode)
            {
                return (true, string.Empty);
            }
            var error = await response.Content.ReadAsStringAsync();
            return (false, $"Błąd API: {response.StatusCode} - {error}");
        }
        catch (Exception ex)
        {
            return (false, $"Błąd aplikacji: {ex.Message}");
        }
    }
}
