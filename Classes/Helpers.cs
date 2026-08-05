using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

namespace QuickLauncher.Classes
{
    internal class Helpers
    {
        /// <summary>
        /// Check if a string is a valid url.
        /// </summary>
        /// <param name="url">The supposed valid url.</param>
        /// <returns>bool</returns>
        public static bool IsValidUrl(string url)
        {
            return Uri.TryCreate(url, UriKind.Absolute, out Uri uri) && (uri.Scheme == Uri.UriSchemeHttp || uri.Scheme == Uri.UriSchemeHttps);
        }


        /// <summary>
        /// Check if a string is a valid email address.
        /// </summary>
        /// <param name="email">The supposed email address.</param>
        /// <returns>bool</returns>
        public static bool IsValidEmail(string email)
        {
            if (string.IsNullOrEmpty(email))
            {
                return false;
            }

            email = email.Trim();

            if (new EmailAddressAttribute().IsValid(email))
            {
                try
                {
                    string extension = Path.GetExtension(email);

                    //check the email domain extension
                    if (string.IsNullOrEmpty(extension) || extension.Length < 2)
                    {
                        return false;
                    }

                    //this validates as correct in EmailAddressAttribute().IsValid(), but not in mailkit so check for it here
                    if (email.Contains(".@") || email.Contains("@.") || email.Contains("..") || email.StartsWith("."))
                    {
                        return false;
                    }

                    return true;
                }
                catch
                {
                }
            }

            return false;
        }


        /// <summary>
        /// Gets the date without weekday in the correct localization: May 27, 2026
        /// </summary>
        /// <param name="value">The DateTime date</param>
        /// <returns>String with the date without weekday</returns>
        public static string GetDateWithoutWeekday(DateTime date)
        {
            return date.ToLongDateString().Replace(DateTimeFormatInfo.CurrentInfo.GetDayName(Convert.ToDateTime(date).DayOfWeek), "").TrimStart(", ".ToCharArray());
        }
    }
}
