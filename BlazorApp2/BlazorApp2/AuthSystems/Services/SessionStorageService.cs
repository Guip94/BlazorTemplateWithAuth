using BlazorApp2.AuthSystems.Services.Interfaces;
using Microsoft.AspNetCore.Components.WebAssembly.Authentication;
using Microsoft.JSInterop;

namespace BlazorApp2.AuthSystems.Services
{
    public class SessionStorageService : ISessionStorageService
    {
        private readonly IJSRuntime _jsRuntime;

        public SessionStorageService(IJSRuntime jsRuntime)
        {
            _jsRuntime = jsRuntime;
        }

        public async Task<string> GetAccessTokenAsync()
        {
            return await _jsRuntime.InvokeAsync<string>("sessionStorage.getItem", "access_token");
        }

        public async Task SetAccessTokenAsync(AccessToken token)
        {
            await _jsRuntime.InvokeVoidAsync("sessionStorage.setItem", "access_token", token.Value);
        }
    }
}
