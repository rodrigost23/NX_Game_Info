using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using LibHac;
using OfficeOpenXml;
using Newtonsoft.Json;
using FsTitle = LibHac.Title;
using Title = NX_Game_Info.Common.Title;
using System.CommandLine;

namespace NX_Game_Info
{
    class Program
    {
        static async Task<int> Main(string[] args)
        {
            Console.OutputEncoding = Encoding.UTF8;

            var infoHeader = GetInfoHeader(doubleLines: true);
            Console.WriteLine(infoHeader);

            Console.WriteLine();
            var pathsArgument = new Argument<List<string>>(
                "paths",
                "paths to files or directories");
            var sdcardOption = new Option<bool>(
                ["-c", "--sdcard"],
                "open path as sdcard");
            var sortOption = new Option<string>(
                ["-s", "--sort"],
                () => "filename",
                "sort by titleid, titlename or filename")
                .FromAmong("titleid", "titlename", "filename");
            var exportOption = new Option<string>(
                ["-x", "--export"],
                "export filename, only *.csv, *.json or *.xlsx supported")
            {
                ArgumentHelpName = "filename.json|filename.csv|filename.xlsx",
            };
            var delimiterOption = new Option<char>(
                ["-l", "--delimiter"],
                () => Common.Settings.Default.CsvSeparator,
                "csv delimiter character");
            var nszOption = new Option<bool>(
                ["-z", "--nsz"],
                "enable nsz extension");
            var debugOption = new Option<bool>(
                ["-d", "--debug"],
                "enable debug log");

            var rootCommand = new RootCommand(Assembly.GetEntryAssembly().GetCustomAttribute<AssemblyDescriptionAttribute>().Description)
            {
                pathsArgument,
                sdcardOption,
                sortOption,
                exportOption,
                delimiterOption,
                nszOption,
                debugOption,
            };

            bool init = Process.initialize(out List<string> messages);

            Console.ForegroundColor = ConsoleColor.DarkRed;
            foreach (var message in messages)
            {
                Console.WriteLine(message);
            }
            Console.ResetColor();

            if (!init)
            {
                Environment.Exit(-1);
            }

            rootCommand.SetHandler((paths, sort, export, sdcard, delimiter, nsz, debug) =>
            {
                Common.Settings.Default.CsvSeparator = delimiter;
                Common.Settings.Default.NszExtension = nsz;
                Common.Settings.Default.DebugLog = debug;
                ProcessPaths(paths, sort, export, sdcard);
            }, pathsArgument, sortOption, exportOption, sdcardOption, delimiterOption, nszOption, debugOption);

            return await rootCommand.InvokeAsync(args);
        }

        static string GetInfoHeader(bool doubleLines = false)
        {
            var info = FileVersionInfo.GetVersionInfo(Assembly.GetExecutingAssembly().Location);
            if (info != null)
            {

                var header = $"{info.ProductName} {info.ProductVersion}";
                if (doubleLines)
                {
                    header += $"\n{info.LegalCopyright} {info.CompanyName}";
                }
                return header;
            }

            AssemblyName assemblyName = Assembly.GetExecutingAssembly().GetName();
            return $"{assemblyName.Name} {assemblyName.Version}";
        }

        static void ProcessPaths(List<string> paths, string sort, string export, bool sdcard)
        {
            List<Title> titles = [];

            foreach (string path in paths)
            {
                if (Directory.Exists(path))
                {
                    titles.AddRange(sdcard ? OpenSDCard(path) : OpenDirectory(path));
                }
                else if (File.Exists(path) && IsValidFile(path) && !sdcard)
                {
                    Title title = OpenFile(path);
                    if (title != null)
                    {
                        titles.Add(title);
                    }
                }
                else
                {
                    Console.ForegroundColor = ConsoleColor.DarkYellow;
                    Console.Error.WriteLine($"{path} is not supported or not a valid path");
                    Console.ResetColor();
                }
            }

            titles = titles.Distinct().OrderBy(x => sort switch
            {
                "titleid" => x.titleID,
                "titlename" => x.titleName,
                _ => x.filename
            }).ToList();

            foreach (Title title in titles)
            {
                Console.ForegroundColor = title.permission switch
                {
                    Title.Permission.Dangerous => ConsoleColor.DarkRed,
                    Title.Permission.Unsafe => ConsoleColor.DarkMagenta,
                    _ => ConsoleColor.Green,
                };

                Console.WriteLine($"\n{title.filename}");
                Console.ResetColor();

                List<dynamic> properties =
                [
                    title.titleID,
                    title.baseTitleID,
                    title.titleName,
                    title.displayVersion,
                    title.versionString,
                    title.latestVersionString,
                    title.systemUpdateString,
                    title.systemVersionString,
                    title.applicationVersionString,
                    title.masterkeyString,
                    title.titleKey,
                    title.publisher,
                    title.languagesString,
                    title.filename,
                    title.filesizeString,
                    title.typeString,
                    title.distribution,
                    title.structureString,
                    title.signatureString,
                    title.permissionString,
                    title.error
                ];

                for (int i = 0; i < properties.Count; i++)
                {
                    var prefix = i == properties.Count - 1 ? "└" : "├";
                    Console.WriteLine($"{prefix} {Title.Properties[i]}: {properties[i]}");
                }
            }

            Process.log?.WriteLine($"\n{titles.Count} titles processed");
            Console.Error.WriteLine($"\n{titles.Count} titles processed");

            if (!string.IsNullOrEmpty(export))
            {
                ExportTitles(titles, export);
            }
        }

        static bool IsValidFile(string path)
        {
            var validExtensions = Common.Settings.Default.NszExtension
                ? new[] { ".xci", ".nsp", ".xcz", ".nsz", ".nro" }
                : [".xci", ".nsp", ".nro"];

            return validExtensions.Any(ext => ext.Equals(Path.GetExtension(path).ToLower()));
        }

        static Title OpenFile(string filename)
        {
            Process.log?.WriteLine("\nOpen File");

            Process.log?.WriteLine("File selected");
            Console.Error.WriteLine($"Opening file {filename}");

            Title title = Process.processFile(filename);

            Process.log?.WriteLine("\nTitle processed");

            return title;
        }

        static List<Title> OpenDirectory(string path)
        {
            Process.log?.WriteLine("\nOpen Directory");

            List<string> filenames = Directory.EnumerateFiles(path, "*.*", SearchOption.AllDirectories)
                .Where(IsValidFile)
                .OrderBy(f => f)
                .ToList();

            Process.log?.WriteLine($"{filenames.Count} files selected");
            Console.Error.WriteLine($"Opening {filenames.Count} files from directory {path}");

            List<Title> titles = [];

            foreach (string filename in filenames)
            {
                Title title = Process.processFile(filename);
                if (title != null)
                {
                    titles.Add(title);
                }
            }

            Process.log?.WriteLine($"\n{titles.Count} titles processed");

            return titles;
        }

        static List<Title> OpenSDCard(string pathSd)
        {
            Process.log?.WriteLine("\nOpen SD Card");

            List<FsTitle> fsTitles = Process.processSd(pathSd);

            List<Title> titles = [];

            if (fsTitles != null)
            {
                foreach (var fsTitle in fsTitles)
                {
                    Title title = Process.processTitle(fsTitle);
                    if (title != null)
                    {
                        titles.Add(title);
                    }
                }

                Process.log?.WriteLine($"\n{titles.Count} titles processed");
            }
            else
            {
                string error = "SD card \"Contents\" directory could not be found";
                Process.log?.WriteLine(error);

                Console.ForegroundColor = ConsoleColor.DarkYellow;
                Console.Error.WriteLine(error);
                Console.ResetColor();
            }

            return titles;
        }

        static void ExportTitles(List<Title> titles, string filename)
        {

            try
            {
                Directory.CreateDirectory(Path.GetDirectoryName(filename));
            }
            catch
            {
                Console.Error.WriteLine($"\n{filename} is not supported or not a valid path");
                return;
            }

            switch (Path.GetExtension(filename).ToLower())
            {
                case ".csv":
                    ExportToCsv(titles, filename);
                    break;
                case ".xlsx":
                    ExportToXlsx(titles, filename);
                    break;
                case ".json":
                    ExportToJson(titles, filename);
                    break;
                default:
                    Process.log?.WriteLine($"\nExport to {Path.GetExtension(filename)} file type is not supported");
                    Console.Error.WriteLine($"\nExport to {Path.GetExtension(filename)} file type is not supported");
                    break;
            }
        }

        static void ExportToCsv(List<Title> titles, string filename)
        {
            var infoHeader = GetInfoHeader();
            using var writer = new StreamWriter(filename);
            char separator = Common.Settings.Default.CsvSeparator;
            if (separator != '\0')
            {
                writer.WriteLine($"sep={separator}");
            }
            else
            {
                separator = ',';
            }

            writer.WriteLine($"# publisher {infoHeader}");
            writer.WriteLine($"# updated {$"{DateTime.Now:F}"}");

            writer.WriteLine(String.Join(separator.ToString(), Title.Properties));

            uint index = 0, count = (uint)titles.Count;

            foreach (var title in titles)
            {
                index++;

                writer.WriteLine(String.Join(separator.ToString(), new string[] {
                    title.titleID.Quote(separator),
                    title.baseTitleID.Quote(separator),
                    title.titleName.Quote(separator),
                    title.displayVersion.Quote(separator),
                    title.versionString.Quote(separator),
                    title.latestVersionString.Quote(separator),
                    title.systemUpdateString.Quote(separator),
                    title.systemVersionString.Quote(separator),
                    title.applicationVersionString.Quote(separator),
                    title.masterkeyString.Quote(separator),
                    title.titleKey.Quote(separator),
                    title.publisher.Quote(separator),
                    title.languagesString.Quote(separator),
                    title.filename.Quote(separator),
                    title.filesizeString.Quote(separator),
                    title.typeString.Quote(separator),
                    title.distribution.ToString().Quote(separator),
                    title.structureString.Quote(separator),
                    title.signatureString.Quote(separator),
                    title.permissionString.Quote(separator),
                    title.error.Quote(separator),
                }));
            }

            Process.log?.WriteLine($"\n{index} of {titles.Count} titles exported to {filename}");
            Console.Error.WriteLine($"\n{index} of {titles.Count} titles exported to {filename}");
        }

        static void ExportToXlsx(List<Title> titles, string filename)
        {
            using var excel = new ExcelPackage();
            ExcelWorksheet worksheet = excel.Workbook.Worksheets.Add(DateTime.Now.ToString("dd MMMM yyyy HH:mm:ss"));

            worksheet.Cells[1, 1, 1, Title.Properties.Length].LoadFromArrays(new List<string[]> { Title.Properties });
            worksheet.Cells["1:1"].Style.Font.Bold = true;
            worksheet.Cells["1:1"].Style.Font.Color.SetColor(Color.White);
            worksheet.Cells["1:1"].Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
            worksheet.Cells["1:1"].Style.Fill.BackgroundColor.SetColor(Color.MidnightBlue);

            uint index = 0, count = (uint)titles.Count;

            foreach (var title in titles)
            {
                index++;

                List<string[]> data =
                    [
                        [
                            title.titleID,
                            title.baseTitleID,
                            title.titleName,
                            title.displayVersion,
                            title.versionString,
                            title.latestVersionString,
                            title.systemUpdateString,
                            title.systemVersionString,
                            title.applicationVersionString,
                            title.masterkeyString,
                            title.titleKey,
                            title.publisher,
                            title.languagesString,
                            title.filename,
                            title.filesizeString,
                            title.typeString,
                            title.distribution.ToString(),
                            title.structureString,
                            title.signatureString,
                            title.permissionString,
                            title.error,
                        ]
                    ];

                worksheet.Cells[(int)index + 1, 1].LoadFromArrays(data);

                string titleID = title.type == TitleType.AddOnContent ? title.titleID : title.baseTitleID ?? "";

                Process.latestVersions.TryGetValue(titleID, out uint latestVersion);
                Process.versionList.TryGetValue(titleID, out uint version);
                Process.titleVersions.TryGetValue(titleID, out uint titleVersion);

                if (latestVersion < version || latestVersion < titleVersion)
                {
                    worksheet.Cells[(int)index + 1, 1, (int)index + 1, Title.Properties.Length].Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                    worksheet.Cells[(int)index + 1, 1, (int)index + 1, Title.Properties.Length].Style.Fill.BackgroundColor.SetColor(title.signature != true ? Color.OldLace : Color.LightYellow);
                }
                else if (title.signature != true)
                {
                    worksheet.Cells[(int)index + 1, 1, (int)index + 1, Title.Properties.Length].Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                    worksheet.Cells[(int)index + 1, 1, (int)index + 1, Title.Properties.Length].Style.Fill.BackgroundColor.SetColor(Color.WhiteSmoke);
                }

                if (title.permission == Title.Permission.Dangerous)
                {
                    worksheet.Cells[(int)index + 1, 1, (int)index + 1, Title.Properties.Length].Style.Font.Color.SetColor(Color.DarkRed);
                }
                else if (title.permission == Title.Permission.Unsafe)
                {
                    worksheet.Cells[(int)index + 1, 1, (int)index + 1, Title.Properties.Length].Style.Font.Color.SetColor(Color.Indigo);
                }
            }

            ExcelRange range = worksheet.Cells[1, 1, (int)count + 1, Title.Properties.Length];
            range.Style.Border.Top.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
            range.Style.Border.Left.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
            range.Style.Border.Right.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
            range.Style.Border.Bottom.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;

            worksheet.Column(1).Width = 18;
            worksheet.Column(2).Width = 18;
            worksheet.Column(3).AutoFit();
            worksheet.Column(3).Width = Math.Max(worksheet.Column(3).Width, 30);
            worksheet.Column(4).Width = 16;
            worksheet.Column(5).Width = 16;
            worksheet.Column(6).Width = 16;
            worksheet.Column(7).Width = 16;
            worksheet.Column(8).Width = 16;
            worksheet.Column(9).Width = 16;
            worksheet.Column(10).Width = 16;
            worksheet.Column(11).AutoFit();
            worksheet.Column(11).Width = Math.Max(worksheet.Column(11).Width, 36);
            worksheet.Column(12).AutoFit();
            worksheet.Column(12).Width = Math.Max(worksheet.Column(12).Width, 30);
            worksheet.Column(13).Width = 18;
            worksheet.Column(14).AutoFit();
            worksheet.Column(14).Width = Math.Max(worksheet.Column(14).Width, 54);
            worksheet.Column(15).Width = 10;
            worksheet.Column(16).Width = 10;
            worksheet.Column(17).Width = 12;
            worksheet.Column(18).Width = 12;
            worksheet.Column(19).Width = 10;
            worksheet.Column(20).Width = 10;
            worksheet.Column(21).Width = 40;

            excel.SaveAs(new FileInfo(filename));

            Process.log?.WriteLine($"\n{titles.Count} titles exported to {filename}");
            Console.Error.WriteLine($"\n{titles.Count} titles exported to {filename}");
        }

        static void ExportToJson(List<Title> titles, string filename)
        {
            var json = JsonConvert.SerializeObject(titles, Formatting.Indented);
            File.WriteAllText(filename, json);

            Process.log?.WriteLine($"\n{titles.Count} titles exported to {filename}");
            Console.Error.WriteLine($"\n{titles.Count} titles exported to {filename}");
        }
    }
}
