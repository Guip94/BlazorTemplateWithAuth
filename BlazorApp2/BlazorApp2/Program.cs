using BlazorApp2;
using BlazorApp2.AuthSystems.Infrastructure;
using BlazorApp2.AuthSystems.Infrastructure.Interfaces;
using BlazorApp2.AuthSystems.Services;
using BlazorApp2.AuthSystems.Services.Interfaces;
using Domain.Repositories.UserAPI;
using Domain.Services.UserAPI;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using MudBlazor.Services;
using System.Security.Claims;
using System.Security.Cryptography.X509Certificates;

var builder = WebAssemblyHostBuilder.CreateDefault(args);









builder.Services.AddMudServices();



builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });




//Configuration de l'authentication
builder.Services.AddScoped<ILocalStorageService, LocalStorageService>();
builder.Services.AddScoped<IJwtService, JwtService>();
builder.Services.AddScoped<ISessionStorageService, SessionStorageService>();
builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<AuthenticationStateProvider, CustomAuthStateProvider>();
builder.Services.AddCascadingAuthenticationState();


builder.Services.AddAuthorizationCore(options =>
{
    options.FallbackPolicy = new AuthorizationPolicyBuilder()
    .RequireAuthenticatedUser()
    .Build();

    options.AddPolicy("adminaccess", policy => policy.RequireClaim(ClaimTypes.Role, "Admin"));
    options.AddPolicy("useraccess", policy => policy.RequireClaim(ClaimTypes.Role, "User"));
});




builder.Services.AddScoped(sp => new HttpClient
{
    BaseAddress = new Uri(builder.HostEnvironment.BaseAddress)
});

builder.Services.AddScoped<AuthenticationHeaderHandler>();


builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");



builder.Services.AddSingleton<ILoggerFactory, LoggerFactory>();



//User API

// AuthenticationState for Authorizing status in the header
builder.Services.AddScoped<IAuthorizingService,AuthorizingService>();


// Repo + services 
builder.Services.AddScoped<IAdressRepository, AdressService>();
builder.Services.AddScoped<IUserRepository, UserService>();

builder.Services.AddHttpClient<ICustomHttpClient, CustomHttpClient>("UserAPI", client =>
{
    client.BaseAddress = new Uri("https://localhost:20443");

    client.DefaultRequestHeaders.Add("Accept", "application/json");
}).AddHttpMessageHandler<AuthenticationHeaderHandler>();








await builder.Build().RunAsync();
