using System;
using System.Threading.Tasks;
using AspNet.Security.OAuth.Validation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RoomMateExpressWebApi.EF.Models;
using RoomMateExpressWebApi.EF.Services;
using RoomMateExpressWebApi.Models.CityViewModels;

namespace RoomMateExpressWebApi.Controllers
{

    [Produces("application/json")]
    [Route("city")]
    public class CityController : Controller
    {
        private readonly ICityService _cityService;

        public CityController(ICityService cityService)
        {
            _cityService = cityService;
        }

        //GET 
        [Authorize(AuthenticationSchemes = OAuthValidationDefaults.AuthenticationScheme, Policy = Constants.Authorization.UserAndAdministratorPolicy)]
        [HttpGet]
        public async Task<IActionResult> GetAllCities()
        {
            return Ok(await _cityService.GetAllCities());
        }

        //GET
        [Authorize(AuthenticationSchemes = OAuthValidationDefaults.AuthenticationScheme, Policy = Constants.Authorization.UserAndAdministratorPolicy)]
        [HttpGet("{id}")]
        public async Task<IActionResult> GetCity(Guid id)
        {
            return Ok(await _cityService.GetCity(id));
        }



        //PUT, Admins(only)
        [Authorize(AuthenticationSchemes = OAuthValidationDefaults.AuthenticationScheme, Policy = Constants.Authorization.AdministratorPolicy)]
        [HttpPut]
        public async Task<IActionResult> CreateOrUpdateCity([FromBody] City city)
        {
            if (ModelState.IsValid)
            {
                var c = await _cityService.GetCity(city.Id);
                if (c == null)
                {
                    return Ok(await _cityService.CreateCity(city));
                }
                else
                {
                    return Ok(await _cityService.UpdateCity(city, city.Id));
                }

            }
            return BadRequest(Constants.Errors.InvalidInput);
        }

        //DELETE, Admins(only)
        [Authorize(AuthenticationSchemes = OAuthValidationDefaults.AuthenticationScheme, Policy = Constants.Authorization.AdministratorPolicy)]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCity(Guid id)
        {
            var result = await _cityService.DeleteCity(id);
            if (result.Equals(true)) return Ok();
            //If something went wrong
            return BadRequest(Constants.Errors.OperationError);
        }


    }
}