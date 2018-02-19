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
using MvvmCross.Platform.UI;
using MvvmCross.Plugins.Color;

namespace RoomMateExpress.Droid.Converters
{
    public class BackgroundColorConverter : MvxColorValueConverter<bool>
    {
        protected override MvxColor Convert(bool value, object parameter, CultureInfo culture)
        {
            return value ? new MvxColor(0x97BBF4) : new MvxColor(0x00FFFFFF);
        }
    }
}