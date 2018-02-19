using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AspNet.Security.OAuth.Validation;
using FirebaseCloudMessagingApi;
using FirebaseCloudMessagingApi.Helpers;
using FirebaseCloudMessagingApi.Models;
using FirebaseCloudMessagingApi.Models.Android;
using FirebaseCloudMessagingApi.Models.Core;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using RoomMateExpressWebApi.EF.Models;
using RoomMateExpressWebApi.EF.Services;
using RoomMateExpressWebApi.Helpers;
using RoomMateExpressWebApi.Models;
using RoomMateExpressWebApi.Models.UserReportViewModels;
using RoomMateExpressWebApi.Services;

namespace RoomMateExpressWebApi.Controllers
{
    [Authorize(AuthenticationSchemes = OAuthValidationDefaults.AuthenticationScheme, Policy = Constants.Authorization.AdministratorPolicy)]
    [Produces("application/json")]
    [Route("user-report")]
    public class UserReportController : Controller
    {
        private readonly IUserReportService _userReportService;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IFirebaseCloudMessagingService _firebaseCloudMessagingService; //Service for sending notifications.

        public UserReportController(IUserReportService userReportService, 
            UserManager<ApplicationUser> userManager, 
            IFirebaseCloudMessagingService firebaseCloudMessagingService)
        {
            _userReportService = userReportService;
            _userManager = userManager;
            _firebaseCloudMessagingService = firebaseCloudMessagingService;
        }

        [HttpGet("all")]
        public async Task<IActionResult> GetAllUserReports()
        {
            return Ok(await _userReportService.GetAllUserReports());
        }

        [HttpGet("all-part")]
        public async Task<IActionResult> GetAllUserReports(int numberPerPage, DateTimeOffset oldestReport)
        {
            return Ok(await _userReportService.GetAllUserReports(oldestReport, numberPerPage));
        }

        [HttpGet("in-process")]
        public async Task<IActionResult> GetAllInProcessUserReports()
        {
            return Ok(await _userReportService.GetAllInProcessUserReports());
        }

        [HttpGet("in-process-part")]
        public async Task<IActionResult> GetAllInProcessUserReports(int numberPerPage,DateTimeOffset oldestReport)
        {
            return Ok(await _userReportService.GetAllInProcessUserReports(oldestReport, numberPerPage));
        }


        [HttpGet("processed")]
        public async Task<IActionResult> GetAllProcessedUserReports()
        {
            return Ok(await _userReportService.GetAllProcessedUserReports());
        }

        [HttpGet("processed-part")]
        public async Task<IActionResult> GetAllProcessedUserReports(int numberPerPage,DateTimeOffset oldestReport)
        {
            return Ok(await _userReportService.GetAllProcessedUserReports(oldestReport, numberPerPage));
        }
        
        [HttpGet("user-reported")]
        public async Task<IActionResult> GetUserReportsHistory(Guid userId)
        {
            return Ok(await _userReportService.GetUserReportsHistory(userId));
        }

        [HttpGet("user-reported-part")]
        public async Task<IActionResult> GetUserReportsHistory(Guid userId, int numberPerPage,DateTimeOffset oldestReport)
        {
            return Ok(await _userReportService.GetUserReportsHistory(userId, oldestReport, numberPerPage));
        }

        [HttpGet("user-reporting")]
        public async Task<IActionResult> GetUserReportingHistory(Guid userId)
        {
            return Ok(await _userReportService.GetUserReportingHistory(userId));
        }

        [HttpGet("user-reporting-part")]
        public async Task<IActionResult> GetUserReportingHistory(Guid userId, int numberPerPage,DateTimeOffset oldestReport)
        {
            return Ok(await _userReportService.GetUserReportingHistory(userId, oldestReport, numberPerPage));
        }

        [HttpGet("current/all")]
        public async Task<IActionResult> GetUserReports()
        {
            var user = await _userManager.GetUserAsync(HttpContext.User);
            if (user != null)
            {
                return Ok(await _userReportService.GetUserReports(user.Id));
            }
            return NotFound(Constants.Errors.UserNotFoundError);
        }

        [HttpGet("current/all-part")]
        public async Task<IActionResult> GetUserReports(int numberPerPage,DateTimeOffset oldestReport)
        {
            var user = await _userManager.GetUserAsync(HttpContext.User);
            if (user != null)
            {
                return Ok(await _userReportService.GetUserReports(user.Id, oldestReport, numberPerPage));
            }
            return NotFound(Constants.Errors.UserNotFoundError);
        }

        [HttpGet("current/in-process")]
        public async Task<IActionResult> GetInProcessUserReports()
        {
            var user = await _userManager.GetUserAsync(HttpContext.User);
            if (user != null)
            {
                return Ok(await _userReportService.GetInProcessUserReports(user.Id));
            }
            return NotFound(Constants.Errors.UserNotFoundError);
        }

        [HttpGet("current/in-process-part")]
        public async Task<IActionResult> GetInProcessUserReports(int numberPerPage,DateTimeOffset oldestReport)
        {
            var user = await _userManager.GetUserAsync(HttpContext.User);
            if (user != null)
            {
                return Ok(await _userReportService.GetInProcessUserReports(user.Id, oldestReport, numberPerPage));
            }
            return NotFound(Constants.Errors.UserNotFoundError);
        }

        [HttpGet("processed")]
        public async Task<IActionResult> GetProcessedUserReports()
        {
            var user = await _userManager.GetUserAsync(HttpContext.User);
            if (user != null)
            {
                return Ok(await _userReportService.GetProcessedUserReports(user.Id));
            }
            return NotFound(Constants.Errors.UserNotFoundError);
        }

        [HttpGet("processed-part")]
        public async Task<IActionResult> GetProcessedUserReports(int numberPerPage,DateTimeOffset oldestReport)
        {
            var user = await _userManager.GetUserAsync(HttpContext.User);
            if (user != null)
            {
                return Ok(await _userReportService.GetProcessedUserReports(user.Id, oldestReport, numberPerPage));
            }
            return NotFound(Constants.Errors.UserNotFoundError);
        }

        //We want to make here that users(only) can send report.
        [Authorize(AuthenticationSchemes = OAuthValidationDefaults.AuthenticationScheme, Policy = Constants.Authorization.UserPolicy)]
        [HttpPost]
        public async Task<IActionResult> ReportUser([FromBody] ReportUserViewModel model)
        {
            if (ModelState.IsValid)
            {
                //Getting user who requested this method.
                var user = await _userManager.GetUserAsync(HttpContext.User);
                if (user != null)
                {
                    var userReport = await _userReportService.ReportUser(model.UserId, user.Id, model.Text);
                    if (userReport != null)
                    {
                        return Accepted();
                    }
                }
                return NotFound(Constants.Errors.UserNotFoundError);
            }
            return BadRequest(Constants.Errors.InvalidInput);
        }

        [HttpPut("process")]
        public async Task<IActionResult> ProcessReport([FromBody] ProcessUserReportViewModel model)
        {
            if (ModelState.IsValid)
            {
                var userReport = await _userReportService.ProcessReport(model.Id, model.Decision);
                if (userReport != null)
                    return Ok(userReport);
                return NotFound(Constants.Errors.UserReportNotFoundError);
            }
            return BadRequest(Constants.Errors.InvalidInput);
        }

        [HttpPut("set-inprocess")]
        public async Task<IActionResult> SetInProcessReport([FromBody] SetInProcessViewModel model)
        {
            if (ModelState.IsValid)
            {
                var userReport = await _userReportService.SetInProcessReport(model.Id);
                if (userReport != null)
                    return Ok(userReport);
                return NotFound(Constants.Errors.UserReportNotFoundError);
            }
            return BadRequest(Constants.Errors.InvalidInput);
        }
    }
}