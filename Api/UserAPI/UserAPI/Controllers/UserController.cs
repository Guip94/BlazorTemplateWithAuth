using Domain.Command.User;
using Domain.Entity;
using Domain.Query.Utilisateurs;
using Domain.Repository;
using LocalNuggetTools.ResultPattern;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using UserAPI.Entities.UserEntity;
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
        [HttpPatch("{id}")]
        [HttpPut("{id}")]
        public IActionResult UpdateMail(int id,UpdateUserDTO user)
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

    [HttpPatch("updatefirstname/{id}")]
    [HttpPut("updatefirstname/{id}")]
    public IActionResult UpdateUserFirstname(int id, [FromBody] UpdateUserDTO user)
    {
            try
            {
                CommandResult rslt = _userRepo.Execute(new UpdateUserFirstname(id, user.Firstname));

                if (rslt.IsFailure) { return BadRequest(); }
                return NoContent();

            }
            catch (Exception ex) { return BadRequest(ex.Message); }
        }

        [HttpPatch("updatelastname/{id}")]
        [HttpPut("updatelastname/{id}")]
        public IActionResult UpdateUserLastname(int id, [FromBody] UpdateUserDTO user)
        {
            try
            {
                CommandResult rslt = _userRepo.Execute(new UpdateUserFirstname(id, user.Lastname));

                if (rslt.IsFailure) { return BadRequest(); }
                return NoContent();

            }
            catch (Exception ex) { return BadRequest(ex.Message); }
        }


    }

}
