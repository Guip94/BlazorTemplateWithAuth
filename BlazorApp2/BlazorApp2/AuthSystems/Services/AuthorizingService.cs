using BlazorApp2.AuthSystems.Services.Interfaces;
using Microsoft.AspNetCore.Components;
using MudBlazor;
using System.Net.Http.Headers;
namespace BlazorApp2.AuthSystems.Services
{
    public class AuthorizingService : IAuthorizingService
    {
        private readonly ILocalStorageService _localStorage;
        private readonly HttpClient _httpClient;
        private readonly ISnackbar _snackbar;
        private readonly NavigationManager _navigation;
        private readonly ILogger _logger;

        public AuthorizingService(ILocalStorageService localStorage, HttpClient httpClient)
        {
            _localStorage = localStorage;
            _httpClient = httpClient;
        }

        public async Task GetCurrentAuthorization()
        {
            string token = await _localStorage.GetItemAsync<string>("authToken");
            if (string.IsNullOrEmpty(token))
            {
                _logger.LogWarning("Token not found. Denied access");
                _snackbar.Add("token not found");
                _navigation.NavigateTo("/");
            }
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
        }
    }
}
