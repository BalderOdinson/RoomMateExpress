using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;

using Android.App;
using Android.Content;
using Android.Content.Res;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Java.Lang;
using MvvmCross.Platform;
using MvvmCross.Platform.Converters;
using MvvmCross.Platform.Droid;
using RoomMateExpress.Core.Extensions;
using RoomMateExpress.Core.Helpers;
using Enum = System.Enum;

namespace RoomMateExpress.Droid.Converters
{
    public class EnumToStringConverter : IMvxValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is Enum enumValue)
            {
                var resorces = Mvx.Resolve<IMvxAndroidGlobals>();
                if (parameter is bool boolParameter && !boolParameter)
                    return resorces.ApplicationContext.Resources.GetString((int) typeof(Resource.String)
                        .GetField(enumValue.GetResourceKey()).GetValue(null)).ToLower();
                return resorces.ApplicationContext.Resources.GetString((int)typeof(Resource.String)
                    .GetField(enumValue.GetResourceKey()).GetValue(null));
            }
            return null;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}