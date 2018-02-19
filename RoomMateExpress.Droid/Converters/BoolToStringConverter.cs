using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using MvvmCross.Platform;
using MvvmCross.Platform.Converters;
using MvvmCross.Platform.Droid;

namespace RoomMateExpress.Droid.Converters
{
    public class BoolToStringConverter : IMvxValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (!(value is bool boolValue)) return null;
            var resorces = Mvx.Resolve<IMvxAndroidGlobals>();
            if (parameter is bool boolParameter && !boolParameter)
                return resorces.ApplicationContext.Resources.GetString(boolValue ? Resource.String.yes : Resource.String.no).ToLower();
            return resorces.ApplicationContext.Resources.GetString(boolValue ? Resource.String.yes : Resource.String.no);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}