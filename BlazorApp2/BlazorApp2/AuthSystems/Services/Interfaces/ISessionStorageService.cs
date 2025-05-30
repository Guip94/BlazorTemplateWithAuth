using Microsoft.AspNetCore.Components.WebAssembly.Authentication;

namespace BlazorApp2.AuthSystems.Services.Interfaces
{
    public interface ISessionStorageService
    {

        Task<string> GetAccessTokenAsync();

        Task SetAccessTokenAsync(AccessToken token);

    }
}
