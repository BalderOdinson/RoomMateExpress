using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RoomMateExpressWebApi.EF.Exceptions;
using RoomMateExpressWebApi.EF.Models;
using RoomMateExpressWebApi.EF.Services;
using RoomMateExpressWebApi.Helpers;

namespace RoomMateExpressWebApi.Controllers
{
    [Produces("application/json")]
    [Route("comment-profile")]
    public class ProfileCommentController : Controller
    {
        private readonly IProfileCommentService _commentForProfileService;

        public ProfileCommentController(IProfileCommentService commentForProfileService)
        {
            _commentForProfileService = commentForProfileService;
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetProfileComment(Guid id)
        {
            return Ok(await _commentForProfileService.GetProfileComment(id));
        }

        [HttpGet("user/{userId}")]
        public async Task<IActionResult> GetAllCommentsByUser(Guid userId)
        {
            return Ok(await _commentForProfileService.GetCommentsByUser(userId));
        }

        [HttpGet("user/{userId}/page")]
        public async Task<IActionResult> GetAllCommentyByUser(Guid userId, DateTimeOffset date, int numberToTake)
        {
            return Ok(await _commentForProfileService.GetCommentsByUser(userId, date, numberToTake));
        }

        [HttpGet("for-user/{userId}")]
        public async Task<IActionResult> GetAllCommentsForUser(Guid userId)
        {
            return Ok(await _commentForProfileService.GetCommentsForUser(userId));
        }

        [HttpGet("for-user/{userId}/page")]
        public async Task<IActionResult> GetAllCommentyForUser(Guid userId, DateTimeOffset date, int numberToTake)
        {
            return Ok(await _commentForProfileService.GetCommentsForUser(userId, date, numberToTake));
        }

        [HttpPut]
        public async Task<IActionResult> CreateProfileComment([FromBody] ProfileComment model)
        {
            try
            {
                model.Id = Guid.NewGuid();
                return Ok(await _commentForProfileService.CreateProfileComment(model));
            }
            catch (Exception e)
            {
                if (e is UserNotFoundException)
                {
                    return NotFound(e.Message);
                }

                return BadRequest(e.Message);
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteProfileComment(Guid id)
        {
            {
                var result = await _commentForProfileService.DeleteProfileComment(id);
                if (result.Equals(true)) return Ok();

                return BadRequest(Constants.Errors.OperationError);
            }
        }
    }
}