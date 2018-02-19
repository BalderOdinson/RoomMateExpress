using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace RoomMateExpressWebApi.Helpers
{
    public static class ErrorsExtensions
    {
        public static void AddErrors(this ModelStateDictionary modelState, IdentityResult result)
        {
            foreach (var error in result.Errors)
            {
                modelState.AddModelError(string.Empty, error.Description);
            }
        }

        public static void AddErrors(this ModelStateDictionary modelState, string[] errors)
        {
            foreach (var error in errors)
            {
                modelState.AddModelError(string.Empty, error);
            }
        }
    }
}
