using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;

namespace FirebaseCloudMessagingApi.Helpers
{
    public static class ColorHelpers
    {
        public static string ToFcmColor(this Color color)
        {
            return "#" + color.R.ToString("X2") + color.G.ToString("X2") + color.B.ToString("X2");
        }
    }
}
