using System.Collections.Generic;
using System.Configuration;
using System.Drawing;

namespace NX_Game_Info.Windows
{
    public class Settings : Common.Settings
    {

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

        public static new Settings Default = (Settings)Synchronized(new Settings());
    }
}