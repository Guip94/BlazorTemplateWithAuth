using System.IdentityModel.Tokens.Jwt;

namespace BlazorApp2.AuthSystems.Services.Interfaces
{
    public interface IJwtService
    {
        JwtPayload? DecodeJwt(string token);

        Dictionary<string, string> GetClaims(string token);

        string? GetUserId (string token);

        string? GetUserName (string token);
    }
}
