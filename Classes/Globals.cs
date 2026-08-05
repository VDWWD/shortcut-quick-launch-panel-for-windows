using System;
using Microsoft.Win32;
using System.Globalization;

namespace QuickLauncher.Classes
{
    internal class Globals
    {
        //change test to true when starting app in visual studio
        public static bool IsTest { get; set; } = true;

        public static decimal AppVersion { get; set; }
        public static string AppName { get; set; }
        public static string AppUrl { get; set; }
        public static string AppEmail { get; set; }
        public static string AppCopyright { get; set; }
        public static string AppDeveloper { get; set; }
        public static bool AppConfirmClose { get; private set; }
        public static string AppLanguage { get; private set; }
        public static string AppPath { get; set; }
        public static string AppPathSettingsFile { get; set; }
        public static AppSetting.Settings AppSettings { get; set; }


        /// <summary>
        /// Init some global variables on app start.
        /// </summary>
        public static void InitGlobals()
        {
            //declare some global variables
            AppVersion = 2.0m;
            AppName = "QuickLauncher";
            AppEmail = "erwin@vanderwaal.eu";
            AppUrl = "https://www.vanderwaal.eu";
            AppCopyright = "van der Waal Webdesign";
            AppDeveloper = "VDWWD";
            AppPath = AppDomain.CurrentDomain.BaseDirectory.TrimEnd('\\');
            AppPathSettingsFile = string.Format(@"{0}\{1}.settings", AppPath, AppName);

            //detect windows language
            AppLanguage = CultureInfo.CurrentUICulture.Name;

            //load settings
            var settings = new AppSetting.Settings();
            AppSettings = settings.Load();

            //when enabled the user must confirm app closure
            AppConfirmClose = true;

            //do some stuff if the app is in test mode
            if (IsTest)
            {
                AppSettings.Test();
            }
        }


        /// <summary>
        /// Check if the current user is using dark mode.
        /// </summary>
        /// <returns>bool</returns>
        public static bool IsDarkMode()
        {
            try
            {
                var key = Registry.GetValue(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Themes\Personalize", "AppsUseLightTheme", "1");

                if (key?.ToString() == "0")
                {
                    return true;
                }
            }
            catch
            {
            }

            return false;
        }
    }
}
