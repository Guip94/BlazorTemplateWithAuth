using Domain.Command.User;
using Domain.Entity;
using Domain.Query.Utilisateurs;
using Domain.Repository;
using LocalNuggetTools.ResultPattern;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using UserAPI.Entities.UserEntity.UpdateDTOs;
namespace UserAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController(IUserRepository _userRepo) : ControllerBase
    {


        [Authorize(Policy = "adminaccess")]
        [HttpGet("AllUsers")]
        public IActionResult GetAll()
        {
            QueryResult<IEnumerable<User>> rslt = _userRepo.Execute(new GetAllUser());

            return Ok(rslt.Result);
        }

        [Authorize(Policy = "adminaccess")]
        [HttpGet("role/{id}")]
        public IActionResult GetRole(int id)
        {
            try
            {
                QueryResult<User> rslt = _userRepo.Execute(new GetUserRoleQuery(id));
                if (rslt.IsFailure) { return NotFound(); }
                return Ok(rslt.Result.Role);

            }
            catch (Exception ex) { return NotFound(); }

        }


        [Authorize]
        [HttpPatch("updatemail")]
        [HttpPut("updatemail")]
        public IActionResult UpdateMail(int id, UpdateUserMailDTO user)
        {
            CommandResult rslt = _userRepo.Execute(new UpdateUserMailCommand(id , user.Pwd, user.Mail));

            if (rslt.IsFailure) { return BadRequest(); }
            return NoContent();
        }

        [HttpGet("userinfos/{id}")]
        public IActionResult GetUserById(int id)
        {
            try
            {
            QueryResult<User> rslt = _userRepo.Execute(new GetUserById(id));
                if (rslt.IsFailure) { return BadRequest("Utilisateur non existant"); }
                return Ok(rslt.Result);
            }
            catch (Exception ex) { return BadRequest(ex.Message); }

        }

    [HttpPatch("updateusernames")]
    [HttpPut("updateusernames")]
    public IActionResult UpdateUserFirstname(int id, UpdateUserDTO user)
    {
            try
            {
                CommandResult rslt = _userRepo.Execute(new UpdateUserNamesCommand(id, user.Lastname, user.Firstname));

                if (rslt.IsFailure) { return BadRequest(); }
                return NoContent();

            }
            catch (Exception ex) { return BadRequest(ex.Message); }
        }

     


    }

}
