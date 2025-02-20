using System.Configuration;
#if MACOS
using System.IO;
#endif
#if MACOS
using Foundation;
#endif

#pragma warning disable IDE1006 // Naming rule violation: These words must begin with upper case characters

namespace NX_Game_Info
{
    public static partial class Common
    {
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

            public static Settings Default = (Settings)Synchronized(new Settings());
        }
    }
}
