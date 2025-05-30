namespace BlazorApp2.AuthSystems.Services.Interfaces
{
    public interface IAuthorizingService
    {
        Task GetCurrentAuthorization();
    }
}
