using Domain.Repository;
using Microsoft.Extensions.Primitives;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Domain.Query.Utilisateurs;
using LocalNuggetTools.ResultPattern;
using Domain.Entity;
using ToolSecurity;
using UserAPI.Properties;
using UserAPI.Entities.UserEntity;
using System.Security.Cryptography;
using Domain.Command.User;
namespace UserAPI.Infrastructure
{
    public class TokenService : ITokenRepository
    {

        private readonly IConfiguration _configuration;
        private readonly IHttpContextAccessor _httpContext;
        private readonly IUserRepository _userRepository;
        private IRsaService rsaServiceSK = new RsaService(Resource.sk);

        public TokenService(IConfiguration configuration, IHttpContextAccessor httpContext, IUserRepository userRepo, IUserRepository userRepository)
        {
            _configuration = configuration;
            _httpContext = httpContext;
            _userRepository = userRepository;
        }

        public UserDTO? User 
        {
            get
            {
                string? Token = ExtractToken();

                if (Token is null) { return null; }
                return ExtractDataFromToken(Token);
            }
        }
        public void ApplyToken(UserDTO user)
        {
            SymmetricSecurityKey key = new SymmetricSecurityKey(Encoding.Default.GetBytes(rsaServiceSK.DecryptAsString(Resource.skData)));
            SigningCredentials creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

           
            QueryResult<User> rslt = _userRepository.Execute(new GetUserRoleQuery(user.Id));

            var temp = rslt.Result.Role;
            var audiences = _configuration.GetSection("Jwt:Audience").Get<string[]>(); 
            var token = new JwtSecurityToken
                 (

                 // Jwt renvoie au appsettings json
                 issuer: _configuration["Jwt:Issuer"],
                 audience: audiences?.FirstOrDefault(),
                 claims:
                     [
                     new Claim (ClaimTypes.NameIdentifier, user.Id.ToString()),
                    new Claim (ClaimTypes.Name, user.Lastname),
                    new Claim ("Firstname", user.Firstname),
                    new Claim ("Mail", user.Mail),
                    new Claim (ClaimTypes.Role, temp),

                    new Claim (JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),

                     ],
                 expires: DateTime.UtcNow.AddMinutes(_configuration.GetValue<int>("Jwt:ExpirationMinutes")),

                 signingCredentials: creds
                 );

            user.RefreshToken = GenerateRefreshToken();

            //create Refresh token to that user Id
            CommandResult UpdateToken = _userRepository.Execute(new InsertRTokenToUserCommand(user.Id, user.RefreshToken));


            user.AccessToken = new JwtSecurityTokenHandler().WriteToken(token);
        }

       


        //Méthode pour générer un refresh token
        public string GenerateRefreshToken()
        {
            var randomNumber = new byte[32];
            using var rng = RandomNumberGenerator.Create();
            rng.GetBytes(randomNumber);
            return Convert.ToBase64String(randomNumber);
        }


       
        public void GenerateAccessTokenFromRefreshToken(UserDTO user, string refreshToken)
        {

            QueryResult<User> rslt = _userRepository.Execute(new GetUserFromTokenQuery(refreshToken));
            
            SymmetricSecurityKey key = new SymmetricSecurityKey(Encoding.Default.GetBytes(rsaServiceSK.DecryptAsString(Resource.skData)));
            SigningCredentials creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var temp = rslt.Result.Role;
            var audiences = _configuration.GetSection("Jwt:Audience").Get<string[]>();
            var token = new JwtSecurityToken
                 (

                 // Jwt renvoie au appsettings json
                 issuer: _configuration["Jwt:Issuer"],
                 audience: audiences?.FirstOrDefault(),
                 claims:
                     [
                     new Claim (ClaimTypes.NameIdentifier, user.Id.ToString()),
                    new Claim (ClaimTypes.Name, user.Lastname),
                    new Claim ("Firstname", user.Firstname),
                    new Claim ("Mail", user.Mail),
                    new Claim (ClaimTypes.Role, rslt.Result.Role),

                    new Claim (JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),

                     ],
                 expires: DateTime.UtcNow.AddDays(7),

                 signingCredentials: creds
                 );

            
            user.AccessToken = new JwtSecurityTokenHandler().WriteToken(token);


        }

        

        private string? ExtractToken()
        {
            const string prefix = "Bearer";
            HttpContext? httpContext = _httpContext.HttpContext;
            if (httpContext is null) { throw new InvalidOperationException(); }

            StringValues autorisations = httpContext.Request.Headers["Authorization"];

            string? token = autorisations.SingleOrDefault(a => a.StartsWith(prefix));

            if (token is null) { return null; }
            return token.Replace(prefix, "");
        }

        private UserDTO? ExtractDataFromToken(string token)
        {
           JwtSecurityTokenHandler handler = new JwtSecurityTokenHandler();
            JwtSecurityToken? jsonToken = handler.ReadToken(token) as JwtSecurityToken;

            if (jsonToken is null) { throw new InvalidOperationException("Invalid token"); }

            JwtPayload payload = jsonToken.Payload;

            return new UserDTO()
            {
                Id = int.Parse((string)payload["Id"]),
                Lastname = (string)payload["Lastname"],
                Firstname = (string)payload["Firstname"],
                Mail = (string)payload["Mail"],
                AccessToken = token
            };
        }


    }
}
