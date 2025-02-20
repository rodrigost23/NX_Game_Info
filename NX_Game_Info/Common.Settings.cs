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
    }
}
