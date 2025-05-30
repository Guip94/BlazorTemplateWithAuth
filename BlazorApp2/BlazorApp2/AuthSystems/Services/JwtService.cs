using BlazorApp2.AuthSystems.Services.Interfaces;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace BlazorApp2.AuthSystems.Services
{
    public class JwtService : IJwtService
    {
        public JwtPayload? DecodeJwt(string token)
        {
            var handler = new JwtSecurityTokenHandler();
            if (handler.CanReadToken(token))
            {
                var jwtToken = handler.ReadJwtToken(token);
                return jwtToken.Payload;
            }
            return null;
        }

        public Dictionary<string, string> GetClaims(string token)
        {
            var claims = new Dictionary<string, string>();
            var payload = DecodeJwt(token);
            if (payload is not null)
            {
                foreach (Claim claim in payload.Claims)
                {
                    claims[claim.Type] = claim.Value;
                }
            }
            return claims;
        }

        public string? GetUserId(string token)
        {
            return GetClaims(token).FirstOrDefault(c => c.Key == ClaimTypes.NameIdentifier).Value;
        }

        public string? GetUserName(string token)
        {
            return GetClaims(token).FirstOrDefault(c => c.Key == ClaimTypes.Name).Value;
        }
    }
}
