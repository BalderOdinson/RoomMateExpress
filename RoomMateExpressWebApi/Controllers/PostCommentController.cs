using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AspNet.Security.OAuth.Validation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RoomMateExpressWebApi.EF.Exceptions;
using RoomMateExpressWebApi.EF.Models;
using RoomMateExpressWebApi.EF.Services;
using RoomMateExpressWebApi.Helpers;
using RoomMateExpressWebApi.Models.CityViewModels;

namespace RoomMateExpressWebApi.Controllers
{
    [Produces("application/json")]
    [Route("comment-post")]
    [Authorize(AuthenticationSchemes = OAuthValidationDefaults.AuthenticationScheme, Policy = Constants.Authorization.UserAndAdministratorPolicy)]
    public class PostCommentController : Controller
    {
        private readonly IPostCommentService _commentForPostService;

        public PostCommentController(IPostCommentService commentForPostService)
        {
            _commentForPostService = commentForPostService;
        }


        //GET
        [HttpGet("{id}")]
        public async Task<IActionResult> GetPostComment(Guid id)
        {
            return Ok(await _commentForPostService.GetPostComment(id));
        }

        //without pagination
        [HttpGet("user/{id}")]
        public async Task<IActionResult> GetAllCommentsByUser(Guid userId)
        {
            return Ok(await _commentForPostService.GetAllCommentsByUser(userId));
        }

        //with pagination
        [HttpGet("user/{id}/page")]
        public async Task<IActionResult> GetAllCommentsByUser(Guid userId, DateTimeOffset date, int numberToTake)
        {
            return Ok(await _commentForPostService.GetAllCommentsByUser(userId, date, numberToTake));
        }

        //without pagination
        [HttpGet("post/{id}")]
        public async Task<IActionResult> GetPostComments(Guid id)
        {
            return Ok(await _commentForPostService.GetPostComments(id));
        }

        //with pagination
        [HttpGet("post/{id}/page")]
        public async Task<IActionResult> GetPostComments(Guid id, int numberToTake, DateTimeOffset date)
        {
            return Ok(await _commentForPostService.GetPostComments(id, date, numberToTake));
        }

        //POST
        [HttpPost]
        public async Task<IActionResult> CreatePostComment([FromBody] PostComment model)
        {
            try
            {
                model.Id = Guid.NewGuid();
                return Ok(await _commentForPostService.CommentPost(model));
            }
            catch (Exception e)
            {
                if (e is PostNotFoundException)
                {
                    return NotFound(e.Message);
                }

                if (e is UserNotFoundException)
                {
                    return NotFound(e.Message);
                }

                return BadRequest(e.Message);
            }
        }

        //DELETE
        [Authorize(AuthenticationSchemes = OAuthValidationDefaults.AuthenticationScheme, Policy = Constants.Authorization.AdministratorPolicy)]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletePostComment(Guid id)
        {
            {
                var result = await _commentForPostService.DeletePostComment(id);
                if (result.Equals(true)) return Ok();
                //If something went wrong
                return BadRequest(Constants.Errors.OperationError);
            }
        }
    }
}