using System;
using System.Threading.Tasks;
using AspNet.Security.OAuth.Validation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using RoomMateExpressWebApi.EF.Models;
using RoomMateExpressWebApi.EF.Services;
using RoomMateExpressWebApi.Helpers;
using RoomMateExpressWebApi.Models;
using RoomMateExpressWebApi.Models.AdminViewModels;

namespace RoomMateExpressWebApi.Controllers
{
    [Authorize(AuthenticationSchemes = OAuthValidationDefaults.AuthenticationScheme, Policy = Constants.Authorization.AdministratorPolicy)]
    [Produces("application/json")]
    [Route("admin")]
    public class AdminController : Controller
    {
        private readonly IAdminService _adminService;
        private readonly UserManager<ApplicationUser> _userManager;
        public AdminController(IAdminService adminService, UserManager<ApplicationUser> userManager)
        {
            _adminService = adminService;
            _userManager = userManager;
        }

        [HttpPut]
        public async Task<IActionResult> CreateOrUpdateAdmin([FromBody] AdminViewModel model)
        {
            if (ModelState.IsValid)
            {
                //We're getting user here and if it doesn't have any admin info, we give him a new one.
                var user = await _userManager.GetUserAsync(HttpContext.User);
                if (user != null)
                {
                    if (!user.ApplicationUserInfo.HasValue)
                    {
                        user.ApplicationUserInfo = Guid.NewGuid();
                        await _userManager.UpdateAsync(user);
                    }
                    await _adminService.AddOrUpdateAdmin(new Admin
                    {
                        Id = user.ApplicationUserInfo.Value,
                        FirstName = model.FirstName,
                        LastName = model.LastName
                    });
                    return Ok(model);
                }
                return NotFound(Constants.Errors.UserNotFoundError);

            }
            return BadRequest(Constants.Errors.InvalidInput);
        }

        [HttpGet]
        public async Task<IActionResult> GetAdmin(bool includeUserReports = false)
        {
            var user = await _userManager.GetUserAsync(HttpContext.User);
            if (user != null)
            {
                if (user.ApplicationUserInfo.HasValue)
                {
                    var adminInfo = await _adminService.GetAdmin(user.ApplicationUserInfo.Value, includeUserReports);
                    return Ok(adminInfo);
                }
                return BadRequest(Constants.Errors.UserNotFoundError);
            }
            return NotFound(Constants.Errors.UserNotFoundError);
        }



        //DELETE
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAdmin(Guid id)
        {
            var result = await _adminService.DeleteAdmin(id);
            if (result) return Ok();
            return NotFound(Constants.Errors.AdminNotFoundError);
        }
    }
}