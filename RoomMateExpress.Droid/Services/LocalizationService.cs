using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using MvvmCross.Platform;
using MvvmCross.Platform.Droid;
using RoomMateExpress.Core.Extensions;
using RoomMateExpress.Core.Services;

namespace RoomMateExpress.Droid.Services
{
    public class LocalizationService : ILocalizationService
    {
        public string GetResourceString(string resourceId)
        {
            if (string.IsNullOrWhiteSpace(resourceId))
                return null;
            var resorces = Mvx.Resolve<IMvxAndroidGlobals>();
            var id = (int?)typeof(Resource.String)
                .GetField(resourceId)?.GetValue(null);
            return id.HasValue ? resorces.ApplicationContext.Resources.GetString(id.Value) : null;
        }
    }
}