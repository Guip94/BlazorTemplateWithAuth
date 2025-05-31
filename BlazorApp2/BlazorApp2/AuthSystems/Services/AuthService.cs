using BlazorApp2.AuthSystems.Infrastructure;
using BlazorApp2.AuthSystems.Models;
using BlazorApp2.AuthSystems.Services.Interfaces;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components;
using System.Net.Http.Json;
using System.Net.Http;
using Microsoft.AspNetCore.Components.WebAssembly.Authentication;
using System.Xml;
using Blazorise;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace BlazorApp2.AuthSystems.Services
{
    public class AuthService : IAuthService
    {
        private readonly HttpClient _httpClient;
        private readonly AuthenticationStateProvider _authStateProvider;
        private readonly ILocalStorageService _localStorageService;
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly NavigationManager _navigationManager;
        private readonly ISessionStorageService _sessionStorageService;


        public AuthService(HttpClient httpClient, AuthenticationStateProvider authStateProvider, ILocalStorageService localStorageService, IHttpClientFactory httpClientFactory, NavigationManager navigationManager, ISessionStorageService sessionStorageService)
        {
            _httpClient = httpClient;
            _authStateProvider = authStateProvider;
            _localStorageService = localStorageService;
            _httpClientFactory = httpClientFactory;
            _navigationManager = navigationManager;
            _sessionStorageService = sessionStorageService;
        }

        public async Task<AuthResult> Login(LoginModel loginModel)
        {
            try
            {
                HttpClient httpClient = _httpClientFactory.CreateClient("UserAPI");





                var response = await httpClient.PostAsJsonAsync("api/Auth/login", loginModel);




                AuthResult rslt = await response.Content.ReadFromJsonAsync<AuthResult>();


                if (response.IsSuccessStatusCode)
                {
                    //await _localStorageService.SetItemAsync("Response", true);
                    await _localStorageService.SetItemAsync("authToken", rslt.AccessToken);
                    await _localStorageService.SetItemAsync("refreshToken", rslt.RefreshToken);

                    //try
                    //{

                    //    AccessToken accessToken = new AccessToken
                    //    {
                    //        Value = rslt.AccessToken,
                    //        Expires = DateTime.UtcNow.AddSeconds(60)
                    //    };

                    //    await _sessionStorageService.SetAccessTokenAsync(accessToken);
                    //}
                    //catch (Exception ex)
                    //{
                    //    Console.WriteLine($"Erreur : {ex.Message}");
                    //}


                    ((CustomAuthStateProvider)_authStateProvider).NotifyUserAuthentication(rslt.AccessToken);




                    rslt.Success = true;

                    return rslt;
                }

                else
                {
                    return new AuthResult
                    {
                        Success = false,
                        Message = rslt?.Message ?? "Echec de connexion",
                        Errors = rslt?.Errors ?? new[] { "Une erreur est survenue lors de la connexion" }
                    };

                }
            }
            catch (Exception ex)
            {
                return new AuthResult
                {
                    Success = false,
                    Message = "Une erreur est survenue",
                    Errors = new[] { ex.Message }
                };
            }
        }
        public async Task<AuthResult> Register(RegisterModel registerModel)
        {
            try
            {
                HttpClient httpClient = _httpClientFactory.CreateClient("UserAPI");

                var response = await httpClient.PostAsJsonAsync("api/Auth/register", registerModel);

                AuthResult registerAuthResult;
                AuthResult loginAuthResult;

                if (response.IsSuccessStatusCode)
                {
                    try
                    {


                      var loginRslt =  await Login(new LoginModel(registerModel.Mail, registerModel.Pwd));

                        if (loginRslt.Success) 
                        {
                            return loginAuthResult = new AuthResult
                            {
                                Success = true,
                                Message = "Inscription réussie"
                            };

                        }
                     

                    }
                    catch (Exception ex)
                    {
                        return loginAuthResult = new AuthResult
                        {
                            Success = false,
                            Message = "Echec de connexion",
                            Errors = new[] { ex.Message }
                        };


                    }
                }

                return registerAuthResult = new AuthResult
                {
                    Success = true,
                    Message = "Inscription réussie"
                };



            }
            catch (Exception ex)
            {
                return new AuthResult
                {
                    Success = false,
                    Message = "Une erreur est survenue",
                    Errors = new[] { ex.Message }
                };
            }
        }

        public async Task Logout()
        {
            try
            {
                HttpClient httpClient = _httpClientFactory.CreateClient("UserAPI");
                await httpClient.PostAsync("api/Auth/logout", null);
            }
            catch
            {

            }
            finally
            {
                await _localStorageService.RemoveItemAsync("authToken");
                await _localStorageService.RemoveItemAsync("refreshToken");
                _httpClient.DefaultRequestHeaders.Authorization = null;
                ((CustomAuthStateProvider)_authStateProvider).NotifyUserLogout();
            }
        }





        public async Task<bool> RefreshToken()
        {
            try
            {
                var token = await _localStorageService.GetItemAsync<string>("authToken");
                var refreshToken = await _localStorageService.GetItemAsync<string>("refreshToken");

                if (string.IsNullOrEmpty(token) || string.IsNullOrEmpty(refreshToken))
                {
                    return false;
                }
                var refreshRequest = new
                {
                    Token = token,
                    RefreshToken = refreshToken,
                };
                HttpClient httpClient = _httpClientFactory.CreateClient("UserAPI");

                var response = await httpClient.PostAsJsonAsync("api/Auth/refresh", refreshRequest);

                if (!response.IsSuccessStatusCode) { return false; }

                var rslt = await response.Content.ReadFromJsonAsync<AuthResult>();

                if (!rslt.Success) { return false; }

                await _localStorageService.SetItemAsync("authToken", rslt.AccessToken);
                await _localStorageService.SetItemAsync("refreshToken", rslt.RefreshToken);

                ((CustomAuthStateProvider)_authStateProvider).NotifyUserAuthentication(rslt.AccessToken);

                return true;
            }
            catch { return false; }
        }

    }
}

