using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using AspNet.Security.OAuth.Validation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using RoomMateExpressWebApi.EF.Exceptions;
using RoomMateExpressWebApi.EF.Extensions;
using RoomMateExpressWebApi.EF.Models;
using RoomMateExpressWebApi.EF.Services;
using RoomMateExpressWebApi.Helpers;
using RoomMateExpressWebApi.Models;

namespace RoomMateExpressWebApi.Controllers
{
    [Authorize(AuthenticationSchemes = OAuthValidationDefaults.AuthenticationScheme, Policy = Constants.Authorization.UserAndAdministratorPolicy)]
    [Produces("application/json")]
    [Route("post")]
    public class PostController : Controller
    {
        private readonly IPostService _postService;
        private readonly UserManager<ApplicationUser> _userManager;

        public PostController(IPostService postService, UserManager<ApplicationUser> userManager)
        {
            _postService = postService;
            _userManager = userManager;
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetPost(Guid id)
        {
            var user = await _userManager.GetUserAsync(HttpContext.User);
            if (user?.ApplicationUserInfo != null)
            {
                return Ok(await _postService.GetPost(id, user.ApplicationUserInfo.Value));
            }
            return NotFound(Constants.Errors.UserNotFoundError);
        }

        [HttpGet("current")]
        public async Task<IActionResult> GetAllPosts(PostSortOptions sortOptions = PostSortOptions.Date,
            SortOrderOption orderOption = SortOrderOption.Descending, AccomodationOptions? accomodationOptions = null,
            decimal? minPrice = null, decimal? maxPrice = null, string city = null, string keyword = null)
        {
            var user = await _userManager.GetUserAsync(HttpContext.User);
            if (user?.ApplicationUserInfo != null &&
                await _userManager.IsInRoleAsync(user, Constants.Authorization.UserRole))
                return Ok(await _postService.GetAllPosts(user.ApplicationUserInfo.Value, sortOptions, orderOption,
                    accomodationOptions, minPrice, maxPrice, city,
                    keyword));
            return Ok(await _postService.GetAllPosts(sortOptions, orderOption,
                accomodationOptions, minPrice, maxPrice, city,
                keyword));
        }

        [HttpGet("current/page")]
        public async Task<IActionResult> GetAllPosts(DateTimeOffset date, string pagingModifier, int numberToTake, PostSortOptions sortOptions = PostSortOptions.Date,
            SortOrderOption orderOption = SortOrderOption.Descending, AccomodationOptions? accomodationOptions = null,
            decimal? minPrice = null, decimal? maxPrice = null, string city = null, string keyword = null)
        {
            var user = await _userManager.GetUserAsync(HttpContext.User);
            if (user?.ApplicationUserInfo != null &&
                await _userManager.IsInRoleAsync(user, Constants.Authorization.UserRole))
                return Ok(await _postService.GetAllPosts(user.ApplicationUserInfo.Value, date, pagingModifier.ParseModifier(sortOptions),
                    numberToTake, sortOptions, orderOption,
                    accomodationOptions, minPrice, maxPrice, city,
                    keyword));
            return Ok(await _postService.GetAllPosts(date, pagingModifier.ParseModifier(sortOptions),
                numberToTake, sortOptions, orderOption,
                accomodationOptions, minPrice, maxPrice, city,
                keyword));
        }

        [HttpGet("user/{userId}")]
        public async Task<IActionResult> GetPosts(Guid userId, string keyword)
        {
            var user = await _userManager.GetUserAsync(HttpContext.User);
            if (user?.ApplicationUserInfo != null &&
                await _userManager.IsInRoleAsync(user, Constants.Authorization.UserRole))
                return Ok(await _postService.GetPostsByUser(userId, user.ApplicationUserInfo.Value, keyword));
            return Ok(await _postService.GetPostsByUser(userId, keyword));
        }

        [HttpGet("user/{userId}/page")]
        public async Task<IActionResult> GetPosts(Guid userId, DateTimeOffset date, int numberToTake, string keyword)
        {
            var user = await _userManager.GetUserAsync(HttpContext.User);
            if (user?.ApplicationUserInfo != null && await _userManager.IsInRoleAsync(user, Constants.Authorization.UserRole))
                return Ok(await _postService.GetPostsByUser(userId, user.ApplicationUserInfo.Value,
                    date, numberToTake, keyword));
            return Ok(await _postService.GetPostsByUser(userId, date, numberToTake, keyword));
        }

        [HttpPost]
        public async Task<IActionResult> CreatePost([FromBody] Post post)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    post.Id = Guid.NewGuid();
                    return Ok(await _postService.CreatePost(post));
                }
                catch (Exception e)
                {
                    if (e is UserNotFoundException || e is NeighborhoodNotFoundException)
                        return NotFound(e.Message);
                    return BadRequest(e.Message);
                }
            }

            return BadRequest(Constants.Errors.InvalidInput);
        }

        [HttpPut]
        public async Task<IActionResult> UpdatePost([FromBody] Post post)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var user = await _userManager.GetUserAsync(HttpContext.User);
                    if (user?.ApplicationUserInfo != null)
                    {
                        return Ok(await _postService.UpdatePost(post, user.ApplicationUserInfo.Value));
                    }
                    throw new UserNotFoundException(Constants.Errors.UserNotFoundError);
                }
                catch (Exception e)
                {
                    return BadRequest(e.Message);
                }
            }

            return BadRequest(Constants.Errors.InvalidInput);
        }

        [Authorize(AuthenticationSchemes = OAuthValidationDefaults.AuthenticationScheme, Policy = Constants.Authorization.UserPolicy)]
        [HttpPost("like/{postId}")]
        public async Task<IActionResult> LikeOrDislike(Guid postId)
        {
            if (ModelState.IsValid)
            {
                var user = await _userManager.GetUserAsync(HttpContext.User);
                if (user?.ApplicationUserInfo != null)
                {
                    try
                    {
                        return Ok(await _postService.LikeDislikePost(postId, user.ApplicationUserInfo.Value));
                    }
                    catch (Exception e)
                    {
                        if (e is UserNotFoundException || e is PostNotFoundException)
                            return NotFound(e.Message);
                        return BadRequest(e.Message);
                    }
                }
            }
            return NotFound(Constants.Errors.UserNotFoundError);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletePost(Guid id)
        {
            try
            {
                var post = await _postService.GetPost(id);
                var user = await _userManager.GetUserAsync(HttpContext.User);
                if (user?.ApplicationUserInfo != null &&
                    await _userManager.IsInRoleAsync(user, Constants.Authorization.UserRole))
                {
                    if (post.User.Id != user.ApplicationUserInfo.Value)
                        return BadRequest(Constants.Errors.InvalidInput);
                }

                if (!await _postService.DeletePost(id)) return NotFound(Constants.Errors.PostNotFound);
                foreach (var picture in post.PostPictures)
                {
                    DeletePicture(picture.PictureUrl.Substring(picture.PictureUrl.LastIndexOf("/") + 1));
                }
                return Ok();
            }
            catch (Exception e)
            {
                ModelState.AddErrors(new[] { e.Message });
                return BadRequest(e.Message);
            }
        }

        #region Helpers

        private static void DeletePicture(string filename)
        {
            var rootPath = Path.Combine(
                Directory.GetCurrentDirectory(),
                "images");

            if (!Directory.Exists(rootPath))
                Directory.CreateDirectory(rootPath);

            var path = Path.Combine(
                rootPath,
                filename);

            if (System.IO.File.Exists(path))
                System.IO.File.Delete(path);
        }

        #endregion
    }
}