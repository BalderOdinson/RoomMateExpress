using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AspNet.Security.OAuth.Validation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RoomMateExpressWebApi.EF.Models;
using RoomMateExpressWebApi.EF.Services;
using RoomMateExpressWebApi.Models.NeighborhoodViewModels;

namespace RoomMateExpressWebApi.Controllers
{
    [Produces("application/json")]
    [Route("neighborhood")]
    public class NeighborhoodController : Controller
    {
        private readonly INeighborhoodService _neighborhoodService;

        public NeighborhoodController(INeighborhoodService neighborhoodService)
        {
            _neighborhoodService = neighborhoodService;
        }

        //GET 
        [Authorize(AuthenticationSchemes = OAuthValidationDefaults.AuthenticationScheme, Policy = Constants.Authorization.UserAndAdministratorPolicy)]
        [HttpGet()]
        public async Task<IActionResult> GetAllNeighborhoods()
        {
            return Ok(await _neighborhoodService.GetAllNeighborhoods());
        }

        //GET
        [Authorize(AuthenticationSchemes = OAuthValidationDefaults.AuthenticationScheme, Policy = Constants.Authorization.UserAndAdministratorPolicy)]
        [HttpGet("{id}")]
        public async Task<IActionResult> GetNeighborhood(Guid id)
        {
            return Ok(await _neighborhoodService.GetNeighborhood(id));
        }

        //PUT, Admins(only)
        [Authorize(AuthenticationSchemes = OAuthValidationDefaults.AuthenticationScheme, Policy = Constants.Authorization.AdministratorPolicy)]
        [HttpPut]
        public async Task<IActionResult> CreateOrUpdateNeighborhood([FromBody] Neighborhood model)
        {
           
                if (ModelState.IsValid)
                {
                    var c = await _neighborhoodService.GetNeighborhood(model.Id);
                    if (c == null)
                    {
                        return Ok(await _neighborhoodService.CreateNeighborhood(model));
                    }
                    else
                    {
                        return Ok(await _neighborhoodService.UpdateNeighborhood(model, model.Id));
                    }

                }
                return BadRequest(Constants.Errors.InvalidInput);

        }

        //DELETE, Admins(only)
        [Authorize(AuthenticationSchemes = OAuthValidationDefaults.AuthenticationScheme, Policy = Constants.Authorization.AdministratorPolicy)]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteNeighborhood(Guid id)
        {
            {
                var result = await _neighborhoodService.DeleteNeighborhood(id);
                if (result.Equals(true)) return Ok();
                //If something went wrong
                return BadRequest(Constants.Errors.OperationError);
            }
        }
    }
}