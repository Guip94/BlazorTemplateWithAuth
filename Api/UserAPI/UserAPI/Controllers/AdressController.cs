using Domain.Command.Adress;
using Domain.Entity;
using Domain.Repository;
using LocalNuggetTools.ResultPattern;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using UserAPI.Entities.AdressEntity;
using Domain.Query.Adresses;
using Microsoft.AspNetCore.Authorization;
namespace UserAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AdressController : ControllerBase
    {
        private readonly IAdressRepository _adressRepo;


        public AdressController(IAdressRepository adressRepo)
        {
            _adressRepo = adressRepo;
        }

        [Authorize]
        [HttpPost]
        public IActionResult CreateAdress(AddAdressDTO adress)
        {
            CommandResult rslt = _adressRepo.Execute(new AddAdressCommand(adress.Country, adress.Zipcode, adress.City, adress.Street, adress.UserId));
            if (rslt.IsFailure) {  return BadRequest(rslt.ErrorMessage); }

            return NoContent();
        }


        [HttpPatch]
        [HttpPut]
        public IActionResult UpdateAdress( UpdateAdressDTO adress)
        {
            CommandResult rslt = _adressRepo.Execute(new UpdateAdressCommand(adress.Country, adress.Zipcode, adress.City, adress.Street, adress.UserId));
            if (rslt.IsFailure) { return BadRequest(rslt.ErrorMessage); }

            return NoContent();
        }

      
        [HttpGet("{userId}")]

        //GetAdressByUserId
        public IActionResult GetSpecificAdress(int userId)
        {
            try
            {

                QueryResult<Adress> rslt = _adressRepo.Execute(new GetAdressByUserId(userId));
                if (rslt.IsFailure) { return BadRequest(rslt.ErrorMessage); }

                return Ok(rslt.Result);

            }
            catch (Exception ex) { return BadRequest(ex.Message); }
        }
    }
}
