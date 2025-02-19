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
using System.Linq;
using System.Xml.Serialization;
#if MACOS
using Foundation;
#endif
using LibHac;
using Newtonsoft.Json;

#pragma warning disable IDE1006 // Naming rule violation: These words must begin with upper case characters

namespace NX_Game_Info
{
    public class Common
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

#if WINDOWS
        [DllImport("Shlwapi.dll", CharSet = CharSet.Auto)]
        public static extern Int32 StrFormatByteSize(
            long fileSize,
            [MarshalAs(UnmanagedType.LPTStr)] StringBuilder buffer,
            int bufferSize);
#endif

        public class Settings : ApplicationSettingsBase
        {
            [UserScopedSetting()]
            [DefaultSettingValue("0")]
            public int Version
            {
                get { return (int)this["Version"]; }
                set { this["Version"] = value; }
            }

            [UserScopedSetting()]
            [DefaultSettingValue("")]
            public string InitialDirectory
            {
                get { return (string)this["InitialDirectory"]; }
                set { this["InitialDirectory"] = value; }
            }

            [UserScopedSetting()]
            [DefaultSettingValue("")]
            public string SDCardDirectory
            {
                get { return (string)this["SDCardDirectory"]; }
                set { this["SDCardDirectory"] = value; }
            }

            [UserScopedSetting()]
            [DefaultSettingValue("False")]
            public bool DebugLog
            {
                get { return (bool)this["DebugLog"]; }
                set { this["DebugLog"] = value; }
            }

            [UserScopedSetting()]
            [DefaultSettingValue("")]
            public char CsvSeparator
            {
                get { return (char)this["CsvSeparator"]; }
                set { this["CsvSeparator"] = value; }
            }

            [UserScopedSetting()]
            [DefaultSettingValue("{n} [{i}][v{v}]")]
            public string RenameFormat
            {
                get { return (string)this["RenameFormat"]; }
                set { this["RenameFormat"] = value; }
            }

            [UserScopedSetting()]
            [DefaultSettingValue("False")]
            public bool NszExtension
            {
                get { return (bool)this["NszExtension"]; }
                set { this["NszExtension"] = value; }
            }

#if WINDOWS
            [UserScopedSetting()]
            [DefaultSettingValue("0, 0")]
            public Point WindowLocation
            {
                get { return (Point)this["WindowLocation"]; }
                set { this["WindowLocation"] = value; }
            }

            [UserScopedSetting()]
            [DefaultSettingValue("800, 600")]
            public Size WindowSize
            {
                get { return (Size)this["WindowSize"]; }
                set { this["WindowSize"] = value; }
            }

            [UserScopedSetting()]
            [DefaultSettingValue("False")]
            public bool Maximized
            {
                get { return (bool)this["Maximized"]; }
                set { this["Maximized"] = value; }
            }

            [UserScopedSetting()]
            [DefaultSettingValue("")]
            public List<string> Columns
            {
                get { return (List<string>)this["Columns"]; }
                set { this["Columns"] = value; }
            }

            [UserScopedSetting()]
            [DefaultSettingValue("")]
            public string SortColumn
            {
                get { return (string)this["SortColumn"]; }
                set { this["SortColumn"] = value; }
            }

            [UserScopedSetting()]
            [DefaultSettingValue("True")]
            public bool SortOrder
            {
                get { return (bool)this["SortOrder"]; }
                set { this["SortOrder"] = value; }
            }

            [UserScopedSetting()]
            [DefaultSettingValue("")]
            public List<int> ColumnWidth
            {
                get { return (List<int>)this["ColumnWidth"]; }
                set { this["ColumnWidth"] = value; }
            }
#endif

            public static Settings Default = (Settings)Synchronized(new Settings());
        }

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

        [Serializable]
        public class Title
        {
            public static readonly Dictionary<string, uint> SystemUpdate = new()
            {
                { "4f8133e5b3657334e507c8e704011886.cnmt.nca", 450 },       // 1.0.0
                { "734d85b19c5f281e100407d84e8cbfb2.cnmt.nca", 65796 },     // 2.0.0
                { "a88bff745e9631e1bbe3ead2e1b8985e.cnmt.nca", 131162 },    // 2.1.0
                { "3ffb630a6ea3842dc6995e61944436ff.cnmt.nca", 196628 },    // 2.2.0
                { "01cca46a5c854c9240568cb0cce0cfd4.cnmt.nca", 262164 },    // 2.3.0
                { "7bef244b45bf63efb4bf47a236975ec6.cnmt.nca", 201327002 }, // 3.0.0
                { "9a78e13d48ca44b1987412352a1183a1.cnmt.nca", 201392178 }, // 3.0.1
                { "16729a20392d179306720f202f37990e.cnmt.nca", 201457684 }, // 3.0.2
                { "6602cb7e2626e61b86d8017b8102011a.cnmt.nca", 268501002 }, // 4.0.1
                { "27478e35cc6872b4b4e508b3c03a4c8f.cnmt.nca", 269484082 }, // 4.1.0
                { "faa857ad6e82f472863e97f810de036a.cnmt.nca", 335544750 }, // 5.0.0
                { "7f5529b7a092b77bf093bdf2f9a3bf96.cnmt.nca", 335609886 }, // 5.0.1
                { "df2b1a655168750bd19458fadac56439.cnmt.nca", 335675432 }, // 5.0.2
                { "de702a1b297bf45f15222e09ebd652b7.cnmt.nca", 336592976 }, // 5.1.0
                { "68649ec371f03e30d981796e516ff38e.cnmt.nca", 402653544 }, // 6.0.0
                { "e4d205cd07c87946566980c78e2f9577.cnmt.nca", 402718730 }, // 6.0.1
                { "455d71f72ea0e91e038c9844cd62efbc.cnmt.nca", 404750376 }, // 6.2.0
                { "b1ff802ffd764cc9a06382207a59409b.cnmt.nca", 469762248 }, // 7.0.0
                { "a39e61c1cce0c86e6e4292d9e5e254e7.cnmt.nca", 469827614 }, // 7.0.1
                { "f4698de5525da797c76740f38a1c08a0.cnmt.nca", 536871502 }, // 8.0.0
                { "197d36b9f1564710dae6edb9a73f03b7.cnmt.nca", 536936528 }, // 8.0.1
                { "68173bf86aa0884f2c989acc4102072f.cnmt.nca", 537919608 }, // 8.1.0
                { "9bde0122ff0c7611460165d3a7adb795.cnmt.nca", 603980216 }, // 9.0.0
                { "9684add4b199811749665b84d27c8cd9.cnmt.nca", 604045412 }, // 9.0.1
                { "7f12839dea0870d71187d0ebeed53270.cnmt.nca", 605028592 }, // 9.1.0
                { "8dec844718aae2464fa9f96865582c08.cnmt.nca", 606076948 }, // 9.2.0
                { "d508702ca7c50d1233662ed6b4993a09.cnmt.nca", 671089000 }, // 10.0.0
                { "2847d2f1dfeb7cd1bde2a7dcf2b67397.cnmt.nca", 671154196 }, // 10.0.1
                { "4ba2c6ae6f8f40f1c44fb82f12af2dde.cnmt.nca", 671219752 }, // 10.0.2
                { "fd7c2112250b321fe1e278dfaf11cd8d.cnmt.nca", 671285268 }, // 10.0.3
                { "8fcd9e3c1938ead201dc790138b595d3.cnmt.nca", 671350804 }, // 10.0.4
                { "528d5b06298ba4c5b656ab6472240821.cnmt.nca", 672137336 }, // 10.1.0
                { "7746448930b5db17c75227dd4a9b2f20.cnmt.nca", 673185852 }, // 10.2.0
            };

            public static readonly string[] Properties =
            [
                "Title ID",
                "Base Title ID",
                "Title Name",
                "Display Version",
                "Version",
                "Latest Version",
                "System Update",
                "System Version",
                "Application Version",
                "Masterkey",
                "Title Key",
                "Publisher",
                "Languages",
                "Filename",
                "Filesize",
                "Type",
                "Distribution",
                "Structure",
                "Signature",
                "Permission",
                "Error",
            ];

            public static readonly string[] LanguageCode =
            [
                "en-US",
                "en-GB",
                "ja",
                "fr",
                "de",
                "es-419",
                "es",
                "it",
                "nl",
                "fr-CA",
                "pt",
                "ru",
                "ko",
                "zh-TW",
                "zh-CN",
            ];

            public enum Distribution
            {
                Digital,
                Cartridge,
                Homebrew,
                Filesystem,
                Invalid = -1
            }

            public enum Structure
            {
                CnmtXml,
                CnmtNca,
                Cert,
                Tik,
                LegalinfoXml,
                NacpXml,
                PrograminfoXml,
                CardspecXml,
                AuthoringtoolinfoXml,
                RootPartition,
                UpdatePartition,
                NormalPartition,
                SecurePartition,
                LogoPartition,
                Invalid = -1
            }

            public enum Permission
            {
                Safe,
                Unsafe,
                Dangerous,
                Invalid = -1
            }

            public Title() { }

            [JsonProperty("TitleID")]
            [XmlElement("TitleID")]
            public string titleID { get; set; } = "";

            [JsonProperty("BaseTitleID")]
            [XmlElement("BaseTitleID")]
            public string baseTitleID { get; set; } = "";

            [JsonProperty("TitleName")]
            [XmlElement("TitleName")]
            public string titleName { get; set; } = "";

            [JsonProperty("DisplayVersion")]
            [XmlElement("DisplayVersion")]
            public string displayVersion { get; set; } = "";

            [JsonProperty("Version")]
            [XmlElement("Version")]
            public uint version { get; set; } = unchecked((uint)-1);

            [JsonIgnore]
            public string versionString => version != unchecked((uint)-1) ? version.ToString() + (version >= 65536 ? " (" + (version / 65536).ToString() + ")" : "") : "";


            [JsonProperty("LatestVersion")]
            [XmlElement("LatestVersion")]
            public uint latestVersion { get; set; } = unchecked((uint)-1);

            [JsonIgnore]
            public string latestVersionString => latestVersion != unchecked((uint)-1) ? latestVersion.ToString() + (latestVersion >= 65536 ? " (" + (latestVersion / 65536).ToString() + ")" : "") : "";


            [JsonProperty("SystemUpdate")]
            [XmlElement("SystemUpdate")]
            public uint systemUpdate { get; set; } = unchecked((uint)-1);

            [JsonIgnore]
            public string systemUpdateString => systemUpdate switch
            {
                0 => "0",
                <= 450 => "1.0.0",
                <= 65796 => "2.0.0",
                <= 131162 => "2.1.0",
                <= 196628 => "2.2.0",
                <= 262164 => "2.3.0",
                unchecked((uint)-1) => "",
                _ => (systemUpdate >> 26 & 0x3F) + "." + (systemUpdate >> 20 & 0x3F) + "." + (systemUpdate >> 16 & 0x0F)
            };

            [JsonProperty("SystemVersion")]
            [XmlElement("SystemVersion")]
            public uint systemVersion { get; set; } = unchecked((uint)-1);

            [JsonIgnore]
            public string systemVersionString => systemVersion switch
            {
                0 => "0",
                <= 450 => "1.0.0",
                <= 65796 => "2.0.0",
                <= 131162 => "2.1.0",
                <= 196628 => "2.2.0",
                <= 262164 => "2.3.0",
                unchecked((uint)-1) => "",
                _ => ((systemVersion >> 26) & 0x3F) + "." + ((systemVersion >> 20) & 0x3F) + "." + ((systemVersion >> 16) & 0x0F)
            };

            [JsonProperty("ApplicationVersion")]
            [XmlElement("ApplicationVersion")]
            public uint applicationVersion { get; set; } = unchecked((uint)-1);

            [JsonIgnore]
            public string applicationVersionString => applicationVersion != unchecked((uint)-1) ? applicationVersion.ToString() : "";


            [JsonProperty("Masterkey")]
            [XmlElement("Masterkey")]
            public uint masterkey { get; set; } = unchecked((uint)-1);

            [JsonIgnore]
            public string masterkeyString => masterkey switch
            {
                0 => masterkey.ToString() + " (1.0.0-2.3.0)",
                1 => masterkey.ToString() + " (3.0.0)",
                2 => masterkey.ToString() + " (3.0.1-3.0.2)",
                3 => masterkey.ToString() + " (4.0.0-4.1.0)",
                4 => masterkey.ToString() + " (5.0.0-5.1.0)",
                5 => masterkey.ToString() + " (6.0.0-6.1.0)",
                6 => masterkey.ToString() + " (6.2.0)",
                7 => masterkey.ToString() + " (7.0.0-8.0.1)",
                8 => masterkey.ToString() + " (8.1.0)",
                9 => masterkey.ToString() + " (9.0.0-9.0.1)",
                10 => masterkey.ToString() + " (9.1.0-10.2.0)",
                unchecked((uint)-1) => "",
                _ => masterkey.ToString(),
            };

            [JsonProperty("TitleKey")]
            [XmlElement("TitleKey")]
            public string titleKey { get; set; } = "";

            [JsonProperty("Publisher")]
            [XmlElement("Publisher")]
            public string publisher { get; set; } = "";

            [JsonProperty("Languages")]
            [XmlElement("Languages")]
            public HashSet<string> languages { get; set; } = new HashSet<string>();

            [JsonIgnore]
            public string languagesString =>
                string.Join(",", languages.Where(x => !string.IsNullOrEmpty(x)));

            [JsonProperty("Filename")]
            [XmlElement("Filename")]
            public string filename { get; set; } = "";

            [JsonProperty("Filesize")]
            [XmlElement("Filesize")]
            public long filesize { get; set; } = 0;

            [JsonIgnore]
            public string filesizeString
            {
                get
                {
#if WINDOWS
                    StringBuilder builder = new StringBuilder(20); StrFormatByteSize(filesize, builder, 20); return builder.ToString();
#elif MACOS
                    return NSByteCountFormatter.Format(filesize, NSByteCountFormatterCountStyle.File);
#else
                    return GetBytesReadable(filesize);
#endif
                }
            }

            [JsonProperty("Type")]
            [XmlElement("Type")]
            public TitleType type { get; set; } = TitleType.Application;

            [JsonIgnore]
            public string typeString => type switch
            {
                TitleType.Application => "Base",
                TitleType.Patch => "Update",
                TitleType.AddOnContent => "DLC",
                _ => "",
            };

            [JsonProperty("Distribution")]
            [XmlElement("Distribution")]
            public Distribution distribution { get; set; } = Distribution.Invalid;

            [JsonProperty("Structure")]
            [XmlElement("Structure")]
            public HashSet<Structure> structure { get; set; } = new HashSet<Structure>();

            [JsonIgnore]
            public string structureString => distribution switch
            {
                Distribution.Cartridge => structure switch
                {
                    var s when new HashSet<Structure>([Structure.UpdatePartition, Structure.SecurePartition]).All(s.Contains) &&
                               new HashSet<Structure>([Structure.RootPartition, Structure.NormalPartition]).Any(s.Contains) => "Scene",
                    var s when new HashSet<Structure>([Structure.SecurePartition]).All(s.Contains) => "Converted",
                    _ => "Not complete"
                },
                Distribution.Digital => structure switch
                {
                    var s when new HashSet<Structure>([Structure.LegalinfoXml, Structure.NacpXml, Structure.PrograminfoXml, Structure.CardspecXml]).All(s.Contains) => "Scene",
                    var s when new HashSet<Structure>([Structure.AuthoringtoolinfoXml]).All(s.Contains) => "Homebrew",
                    var s when new HashSet<Structure>([Structure.Cert, Structure.Tik]).All(s.Contains) => "CDN",
                    var s when new HashSet<Structure>([Structure.CnmtXml]).All(s.Contains) => "Converted",
                    _ => "Not complete"
                },
                Distribution.Filesystem => "Filesystem",
                _ => ""
            };

            [JsonProperty("Signature")]
            [XmlElement("Signature")]
            public bool? signature { get; set; } = null;

            [JsonIgnore]
            public string signatureString => signature == null ? "" : (bool)signature ? "Passed" : "Not Passed";


            [JsonProperty("Permission")]
            [XmlElement("Permission")]
            public Permission permission { get; set; } = Permission.Invalid;

            [JsonIgnore]
            public string permissionString => permission == Permission.Invalid ? "" : permission.ToString();


            [JsonProperty("Error")]
            [XmlElement("Error")]
            public string error { get; set; } = "";
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
