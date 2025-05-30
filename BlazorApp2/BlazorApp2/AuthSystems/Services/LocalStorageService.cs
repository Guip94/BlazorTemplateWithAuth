using BlazorApp2.AuthSystems.Services.Interfaces;
using Microsoft.JSInterop;
using System.Text.Json;

namespace BlazorApp2.AuthSystems.Services
{
    public class LocalStorageService : ILocalStorageService
    {
        private readonly IJSRuntime _jsRuntime;
        private readonly JsonSerializerOptions _jsonOptions;

        public LocalStorageService(IJSRuntime jsRuntime )
        {
            _jsRuntime = jsRuntime;
            _jsonOptions = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true,
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase
            };
        }

        public async Task<T> GetItemAsync<T>(string key)
        {
            try
            {
                var json = await _jsRuntime.InvokeAsync<string>("localStorage.getItem", key);

                if (json is null)
                {
                    return default;
                }
                return JsonSerializer.Deserialize<T>(json, _jsonOptions);
            }
            catch (Exception ex) 
            {
                Console.WriteLine($"Error reading from localStorage : {ex.Message}");
                return default;
            }
        }
        public async Task SetItemAsync<T>(string key, T value)
        {
            try
            {
                var json = JsonSerializer.Serialize(value, _jsonOptions);
                await _jsRuntime.InvokeVoidAsync("localStorage.setItem", key, json);
            }
            catch(Exception ex) 
            {
                Console.WriteLine($"Error writing to localStorage : {ex.Message}");
                throw;
            }
        }
        public async Task RemoveItemAsync(string key)
        {
            try
            {
                await _jsRuntime.InvokeVoidAsync("localStorage.removeItem", key);
            }
            catch (Exception ex) 
            {
                Console.WriteLine($"Error removing item from localStorage : {ex.Message}");
                throw;
            }
        }

        public async Task ClearAsync()
        {
            try
            {
                await _jsRuntime.InvokeVoidAsync("localStorage.clear");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error clearing localStorage : {ex.Message}");
                throw;
            }
        }

        public async Task<bool> ContainKeyAsync(string key)
        {
            try
            {
               string result = await _jsRuntime.InvokeAsync<string>("localStorage.getItem", key);
                return result is not null;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error checking the key in localStorage : {ex.Message}");
                return false;
            }
        }


    }
}
