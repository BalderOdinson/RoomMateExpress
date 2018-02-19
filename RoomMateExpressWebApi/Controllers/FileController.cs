using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using AspNet.Security.OAuth.Validation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.AspNetCore.Mvc;
using RoomMateExpressWebApi.EF.Models;
using RoomMateExpressWebApi.Models;

namespace RoomMateExpressWebApi.Controllers
{
    [Route("image")]
    [Authorize(AuthenticationSchemes = OAuthValidationDefaults.AuthenticationScheme, Policy = Constants.Authorization.UserAndAdministratorPolicy)]
    public class FileController : Controller
    {
        private readonly IHostingEnvironment _hostingEnvironment;
        public FileController(IHostingEnvironment hostingEnvironment)
        {
            _hostingEnvironment = hostingEnvironment;
        }

        [HttpPost]
        public async Task<IActionResult> Upload(IFormFile image)
        {
            if (image == null)
            {
                return BadRequest(Constants.Errors.InvalidInput);
            }

            var rootPath = Path.Combine(
                Directory.GetCurrentDirectory(),
                "images");

            if (!Directory.Exists(rootPath))
                Directory.CreateDirectory(rootPath);

            var path = Path.Combine(
                rootPath,
                Path.GetRandomFileName() + ".jpg");

            using (var stream = new FileStream(path, FileMode.Create))
            {
                await image.CopyToAsync(stream);
            }
            return Ok(Request.GetDisplayUrl() + "/" + new FileInfo(path).Name);
        }

        [HttpDelete("{filename}")]
        public IActionResult DeleteFile(string filename)
        {
            if (filename == null)
                return BadRequest(Constants.Errors.ArgumentNullError);

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

            return Ok();
        }

        [AllowAnonymous]
        [HttpGet("{filename}")]
        public async Task<IActionResult> Download(string filename)
        {
            if (filename == null)
                return Content("filename not present");

            var rootPath = Path.Combine(
                Directory.GetCurrentDirectory(),
                "images");

            if (!Directory.Exists(rootPath))
                Directory.CreateDirectory(rootPath);

            var path = Path.Combine(
                rootPath,
                filename);

            var memory = new MemoryStream();
            using (var stream = new FileStream(path, FileMode.Open))
            {
                await stream.CopyToAsync(memory);
            }
            memory.Position = 0;
            return File(memory, "image/jpeg", Path.GetFileName(path));
        }
    }
}
