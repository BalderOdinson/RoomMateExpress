using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using MvvmValidation;
using RoomMateExpress.Core.Helpers.Collections;

namespace RoomMateExpress.Core.Extensions
{
    public static class Validation
    {
        public static ObservableDictionary<string, string> AsObservableDictionary(this ValidationResult result)
        {
            var dictionary = new ObservableDictionary<string, string>();
            foreach (var item in result.ErrorList)
            {
                var key = item.Target.ToString();
                var text = item.ErrorText;
                if (dictionary.ContainsKey(key))
                {
                    dictionary[key] = dictionary.Keys + Environment.NewLine + text;
                }
                else
                {
                    dictionary[key] = text;
                }
            }
            return dictionary;
        }

        public static bool IsValidEmail(this string email)
        {
            try
            {
                var addr = new System.Net.Mail.MailAddress(email);
                return addr.Address == email;
            }
            catch
            {
                return false;
            }
        }

        public static bool IsValidPassword(this string password)
        {
            return Regex.IsMatch(password, @"(?=\w{6,}\z)(?=[^a-z]*[a-z])(?=[^A-Z]*[A-Z])(?=\D*\d)");
        }
    }
}
