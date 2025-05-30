using Domain.Entity;
using Domain.Repository;
using LocalNuggetTools.ResultPattern;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Domain.Query.Roles;
namespace UserAPI.Controllers
{
    [Authorize(Policy = "adminaccess")]
    [Route("api/[controller]")]
    [ApiController]
    public class RoleController(IRoleRepository _roleRepo) : ControllerBase
    {
        
        [HttpGet("GetAllRoles")]
        public IActionResult GetAll()
        {
            QueryResult<IEnumerable<Role>> rslt = _roleRepo.Execute(new GetAllRoles());

            return Ok(rslt);
        }

    }
}
