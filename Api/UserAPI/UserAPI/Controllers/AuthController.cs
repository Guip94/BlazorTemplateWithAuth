using Domain.Repository;
using LocalNuggetTools.ResultPattern;
using Microsoft.AspNetCore.Mvc;
using Domain.Command.User;
using Domain.Entity;
using UserAPI.Entities.Mappers;
using UserAPI.Infrastructure;
using Microsoft.AspNetCore.Authorization;
using Domain.Query.Utilisateurs;
using UserAPI.Entities.UserEntity;
using System.Security.Claims;
using System.IdentityModel.Tokens.Jwt;

namespace UserAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]


    // AllowAnonymous permet l'accès à n'importe qui!!!!!!!!!!!!!
    public class AuthController(IUserRepository _userRepo, ITokenRepository _tokenRepo, IBlackList _blackListedToken) : ControllerBase
    {
        

        [HttpPost("register")]
        public IActionResult Register(RegisterDTO user)
        {
            CommandResult rslt = _userRepo.Execute(new AddUserCommand(user.Mail, user.Pwd, user.Lastname, user.Firstname));
            if (rslt.IsFailure) { return BadRequest(rslt.ErrorMessage); }
            return NoContent();
        }

        [HttpPost("login")]
        public IActionResult Login(LoginDTO login)
        {
            try
            {
                QueryResult<User> rslt = _userRepo.Execute(new LoginQuery(login.Mail, login.Pwd));
                if (rslt.IsFailure) { return BadRequest(rslt); }

                UserDTO userDto = rslt.Result.ToUserDto();
                _tokenRepo.ApplyToken(userDto);
                return Ok(userDto);

            }
            catch (Exception ex) { return BadRequest(new { ErrorType = ex.GetType() }); }

        }

   

     

    

        [Authorize]
        [HttpPost("logout")]

        public IActionResult logout()
        {
            try
            {
                var token = Request.Headers["Authorization"].ToString().Replace("Bearer ", "").Trim();

                if (!string.IsNullOrWhiteSpace(token))
                {
                    _blackListedToken.AddToken(token);
                }

                return Ok(new { message = "Déconnexion réussie" });
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Une erreur est survenue lors de la déconnexion");
            }
        }
      
    

        [Authorize]
        [HttpPost("refresh")]
        public IActionResult Refresh()
        {
            UserDTO user;
            List<Claim> claims = new();

            if (User is not null)
            {
                claims = User.Claims.ToList();
            }
            //Récupération du claim "expire" et traduction de celui-ci en une valeur comparable à Datetime
            var expClaim = claims.FirstOrDefault(c => c.Type == JwtRegisteredClaimNames.Exp);

            long ValueAsUnixTimestamp = long.Parse(expClaim.Value);

            DateTime expirationTime = DateTimeOffset.FromUnixTimeSeconds(ValueAsUnixTimestamp).DateTime;

            var tempData = DateTime.UtcNow - expirationTime;
            //On s'assure que le temps soit à 0
            if(Math.Abs(tempData.Minutes) <= 0)
            {
                var ToUserId = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;
                int userId = int.Parse(ToUserId);
                //On récupère le refresh token de la db
                try
                {
                    QueryResult<User> rslt = _userRepo.Execute(new GetUserByIdWithToken(userId));


                    user = rslt.Result.ToUserDtoWithToken();
                    //user = new UserDTO
                    //{
                    //    Id = rslt.Result.Id,
                    //    Mail = rslt.Result.Mail,
                    //    Lastname = rslt.Result.Lastname,
                    //    Firstname = rslt.Result.Firstname,
                    //    RefreshToken = rslt.Result.RefreshToken
                    //};

                    string refreshToken = user.RefreshToken;

                    _tokenRepo.GenerateAccessTokenFromRefreshToken(user, refreshToken);


                }
                catch (Exception ex) { return BadRequest(ex.Message); }


            }

            else
            {
                return BadRequest("Vous êtes toujours connecté avec le même token");
            }
           
            return Ok(user);
        }
    }
}
