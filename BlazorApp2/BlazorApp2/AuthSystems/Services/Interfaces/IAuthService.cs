using BlazorApp2.AuthSystems.Models;

namespace BlazorApp2.AuthSystems.Services.Interfaces
{
    public enum AuthType
    {
        ApiBearer
    }
    public interface IAuthService
    {

        Task<AuthResult> Login(LoginModel loginModel);

        Task<AuthResult> Register(RegisterModel registerModel);


        Task Logout();

        Task<bool> RefreshToken();
    }
}
