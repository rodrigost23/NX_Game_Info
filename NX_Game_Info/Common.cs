using System;
using System.Collections.Generic;
using System.Configuration;
#if WINDOWS
using System.Drawing;
using System.Runtime.InteropServices;
using System.Text;
#endif
#if MACOS
using System.IO;
#endif
using System.Xml.Serialization;
#if MACOS
using Foundation;
#endif
using LibHac;
using Newtonsoft.Json;

#pragma warning disable IDE1006 // Naming rule violation: These words must begin with upper case characters

namespace NX_Game_Info
{
    public static partial class Common
    {
#if WINDOWS
        [DllImport("Shlwapi.dll", CharSet = CharSet.Auto)]
        public static extern Int32 StrFormatByteSize(
            long fileSize,
            [MarshalAs(UnmanagedType.LPTStr)] StringBuilder buffer,
            int bufferSize);
#endif
        public class Aaa { }

        public class History : ApplicationSettingsBase
        {
            [UserScopedSetting()]
            [DefaultSettingValue("")]
            [SettingsSerializeAs(SettingsSerializeAs.Xml)]
            public List<ArrayOfTitle> Titles
            {
                get { return (List<ArrayOfTitle>)this["Titles"]; }
                set { this["Titles"] = value; }
            }

            public static History Default = (History)Synchronized(new History());
        }

        public class RecentDirectories : ApplicationSettingsBase
        {
            [UserScopedSetting()]
            [DefaultSettingValue("")]
            [SettingsSerializeAs(SettingsSerializeAs.Xml)]
            public List<ArrayOfTitle> Titles
            {
                get { return (List<ArrayOfTitle>)this["Titles"]; }
                set { this["Titles"] = value; }
            }

            public static RecentDirectories Default = (RecentDirectories)Synchronized(new RecentDirectories());
        }

        [Serializable]
        public class ArrayOfTitle
        {
            public ArrayOfTitle() { }

            [XmlElement("Title")]
            public List<Title> title { get; set; }
            [XmlAttribute("Description")]
            public string description { get; set; }
        }

        public class VersionTitle
        {
            public string id { get; set; }
            public uint version { get; set; }
            public uint required_version { get; set; }
        }

        public class VersionList
        {
            public List<VersionTitle> titles { get; set; }
            public uint format_version { get; set; }
            public uint last_modified { get; set; }
        }

        // GetBytesReadable Credits to Shailesh N. Humbad https://www.somacon.com/p576.php
        public static string GetBytesReadable(long i)
        {
            // Get absolute value
            long absolute_i = (i < 0 ? -i : i);
            // Determine the suffix and readable value
            (double readable, string suffix) = absolute_i switch
            {
                >= 0x1000000000000000 => (i >> 50, "EB"),
                >= 0x4000000000000 => (i >> 40, "PB"),
                >= 0x10000000000 => (i >> 30, "TB"),
                >= 0x40000000 => (i >> 20, "GB"),
                >= 0x100000 => (i >> 10, "MB"),
                >= 0x400 => (i, "KB"),
                _ => (i << 10, "B")
            };
            // Divide by 1024 to get fractional value
            readable /= 1024;
            // Return formatted number with suffix
            return readable.ToString("0.## ") + suffix;
        }
    }
}
