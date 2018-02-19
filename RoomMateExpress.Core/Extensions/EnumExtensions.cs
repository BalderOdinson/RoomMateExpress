using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Text;
using RoomMateExpress.Core.Helpers;
using RoomMateExpress.Core.Helpers.Enums;

namespace RoomMateExpress.Core.Extensions
{
    public static class EnumExtensions
    {
        public static TAttribute GetAttribute<TAttribute>(this Enum value)
            where TAttribute : Attribute
        {
            var type = value.GetType();
            var name = Enum.GetName(type, value);
            return type.GetRuntimeField(name)
                .GetCustomAttributes(false)
                .OfType<TAttribute>()
                .SingleOrDefault();
        }

        public static IEnumerable<ResourceKeyAttribute> ConvertEnumToList<TEnum>()
            where TEnum : struct // can't constrain to enums so closest thing
        {
            return Enum.GetValues(typeof(TEnum)).Cast<Enum>()
                .Select(val => val.GetAttribute<ResourceKeyAttribute>())
                .ToList();
        }

        public static string GetResourceKey(this Enum value)
        {
            return value.GetAttribute<ResourceKeyAttribute>().Key;
        }

        public static ResourceKeyAttribute GetEnumByType<TEnum>(string value)
            where TEnum : struct // can't constrain to enums so closest thing
        {
            return Enum.GetValues(typeof(TEnum)).Cast<Enum>()
                .Select(val => val.GetAttribute<ResourceKeyAttribute>())
                .ToList().FirstOrDefault(v => v.Key.Equals(value));
        }
    }
}
