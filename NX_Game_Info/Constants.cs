using System;
using System.Collections.Generic;
#if WINDOWS
using System.Drawing;
using System.Runtime.InteropServices;
using System.Text;
#endif
#if MACOS
using System.IO;
#endif
using System.Linq;
#if MACOS
using Foundation;
#endif

#pragma warning disable IDE1006 // Naming rule violation: These words must begin with upper case characters

namespace NX_Game_Info
{
    public static class Constants
    {
        public static readonly string APPLICATION_DIRECTORY_PATH_PREFIX =
#if MACOS
            Path.GetDirectoryName(NSBundle.MainBundle.BundleUrl.Path) + "/";
#else
            "";
#endif
        public static readonly string USER_PROFILE_PATH_PREFIX = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile) + "/.switch/";

        public static readonly string LOG_FILE = "debug.log";

        public static readonly string USER_SETTINGS = "user.settings";
        public static readonly int HISTORY_SIZE = 10;

        public static readonly string PROD_KEYS = "prod.keys";
        public static readonly string TITLE_KEYS = "title.keys";
        public static readonly string CONSOLE_KEYS = "console.keys";
        public static readonly string HAC_VERSIONLIST = "hac_versionlist.json";
        public static readonly string TITLE_KEYS_URI = "https://gist.githubusercontent.com/gneurshkgau/81bcaa7064bd8f98d7dffd1a1f1781a7/raw/title.keys";
        public static readonly string HAC_VERSIONLIST_URI = "https://gist.githubusercontent.com/gneurshkgau/81bcaa7064bd8f98d7dffd1a1f1781a7/raw/hac_versionlist.json";
    }

    public static class VersionExtension
    {
        public static int ToInt(this Version version)
        {
            return Math.Max(version.Major, 0) * 100_00_00 + Math.Max(version.Minor, 0) * 100_00 + Math.Max(version.Build, 0) * 100 + Math.Max(version.Revision, 0);
        }
    }

    public static class StringExtension
    {
        public static string Quote(this string text, char separator = ' ')
        {
            return text.Contains(separator) ? String.Format("\"{0}\"", text) : text;
        }
    }
}
