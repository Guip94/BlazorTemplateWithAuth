using UserAPI.Entities.UserEntity;

namespace UserAPI.Infrastructure
{
    public interface ITokenRepository
    {
        UserDTO? User { get; }
        void ApplyToken(UserDTO user);
        void GenerateAccessTokenFromRefreshToken(UserDTO user, string refreshToken);
        string GenerateRefreshToken();

    }
}
