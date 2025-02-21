

using System.Reflection;

namespace NX_Game_Info.Windows
{
    class Process : NX_Game_Info.Process
    {

        public static new void migrateSettings()
        {
            int version = Settings.Default.Version;

            if (version < 00_06_00_00)
            {
                int columnIndex = Settings.Default.Columns.FindIndex(x => x.Equals("firmware"));
                if (columnIndex != -1)
                {
                    Settings.Default.Columns.RemoveAt(columnIndex);
                    Settings.Default.Columns.InsertRange(columnIndex, new string[] { "systemUpdateString", "systemVersionString", "applicationVersionString" });

                    Settings.Default.ColumnWidth.RemoveAt(columnIndex);
                    Settings.Default.ColumnWidth.InsertRange(columnIndex, new int[] { 100, 100, 100 });
                }
            }

            if (version < 00_07_00_00)
            {
                int columnIndex = Settings.Default.Columns.FindIndex(x => x.Equals("filename") || x.Equals("filesizeString") ||
                    x.Equals("typeString") || x.Equals("distribution") || x.Equals("structureString") || x.Equals("signatureString") || x.Equals("permissionString") || x.Equals("error"));
                if (columnIndex == -1)
                {
                    columnIndex = Settings.Default.Columns.Count;
                }
                Settings.Default.Columns.InsertRange(columnIndex, new string[] { "titleKey", "publisher" });
                Settings.Default.ColumnWidth.InsertRange(columnIndex, new int[] { 240, 200 });
            }

            if (version < 00_07_00_01)
            {
                int columnIndex = Settings.Default.Columns.FindIndex(x => x.Equals("filename") || x.Equals("filesizeString") ||
                    x.Equals("typeString") || x.Equals("distribution") || x.Equals("structureString") || x.Equals("signatureString") || x.Equals("permissionString") || x.Equals("error"));
                if (columnIndex == -1)
                {
                    columnIndex = Settings.Default.Columns.Count;
                }
                Settings.Default.Columns.InsertRange(columnIndex, new string[] { "languagesString" });
                Settings.Default.ColumnWidth.InsertRange(columnIndex, new int[] { 120 });
            }
            Settings.Default.Version = Assembly.GetExecutingAssembly().GetName().Version.ToInt();
        }
    }
}