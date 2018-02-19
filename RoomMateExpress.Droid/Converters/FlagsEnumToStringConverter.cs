using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Reflection;
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
using RoomMateExpress.Core.Extensions;

namespace RoomMateExpress.Droid.Converters
{
    public class FlagsEnumToStringConverter : IMvxValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (!(value is Enum enumValue)) return null;
            var sb = new StringBuilder();
            var delimeter = parameter as string ?? " ";
            var resorces = Mvx.Resolve<IMvxAndroidGlobals>();
            var flags = enumValue.GetType().GetEnumValues();
            if (System.Convert.ToInt32(enumValue) == 0)
            {
                var zeroFlag = flags.GetValue(0);
                return resorces.ApplicationContext.Resources.GetString((int)typeof(Resource.String)
                    .GetField((zeroFlag as Enum).GetResourceKey()).GetValue(null));
            }
            foreach (Enum flag in flags)
            {
                if (enumValue.HasFlag(flag) && !flag.Equals(flags.GetValue(0)))
                {
                    if (sb.Length != 0)
                    {
                        sb.Append(delimeter);
                        sb.Append(resorces.ApplicationContext.Resources.GetString((int)typeof(Resource.String)
                            .GetField(flag.GetResourceKey()).GetValue(null)).ToLower());
                    }
                    else
                    {
                        sb.Append(resorces.ApplicationContext.Resources.GetString((int)typeof(Resource.String)
                            .GetField(flag.GetResourceKey()).GetValue(null)));
                    }
                }
            }
            return sb.ToString();
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}