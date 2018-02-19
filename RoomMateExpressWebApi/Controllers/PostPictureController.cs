using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RoomMateExpressWebApi.EF.Models;
using RoomMateExpressWebApi.EF.Services;
using RoomMateExpressWebApi.Models.PostPictureViewModels;

namespace RoomMateExpressWebApi.Controllers
{
    [Produces("application/json")]
    [Route("post-picture")]
    public class PostPictureController : Controller
    {
        private readonly IPostPictureService _postPictureService;

        public PostPictureController(IPostPictureService postPictureService)
        {
            _postPictureService = postPictureService;
        }

        [HttpGet]
        public async Task<IActionResult> GetPostPicture(Guid id)
        {
            return Ok(await _postPictureService.GetPostPicture(id));
        }



        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletePostPicture(Guid id)
        {
            var result = await _postPictureService.DeletePostPicture(id);
            if (result.Equals(true)) return Ok();

            return BadRequest(ModelState);
        }

        [HttpPost]
        public async Task<IActionResult> AddPostPicture([FromBody] PostPicture model)
        {
            return Ok(await _postPictureService.AddPostPicture(model));
        }
    }
}