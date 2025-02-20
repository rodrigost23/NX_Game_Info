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
using System.Xml.Serialization;
#if MACOS
using Foundation;
#endif
using LibHac;
using Newtonsoft.Json;

#pragma warning disable IDE1006 // Naming rule violation: These words must begin with upper case characters

namespace NX_Game_Info
{
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
#if MACOS
                return NSByteCountFormatter.Format(filesize, NSByteCountFormatterCountStyle.File);
#else
                return Common.GetBytesReadable(filesize);
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
}
