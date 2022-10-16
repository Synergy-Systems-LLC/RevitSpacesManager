using System;
using System.IO;
using WixSharp;
using WixSharp.CommonTasks;
using WixSharp.Controls;

namespace WixSharpSetup
{
    internal class Program
    {
        private static void Main()
        {
            // Program заменить на тип из главного проекта и удалить комментарий.
            // Тип НЕ должен зависеть от сторонних библиотек, например typeof(RevitManager).
            var assemblyName = typeof(Program).Assembly.GetName();

            var outFileName = $"{assemblyName.Name}-{assemblyName.Version}";
            var version = assemblyName.Version;
            var projectName = assemblyName.Name;

            var guid = new Guid("51a63727-4b6e-4425-840e-d5343238cb99"); // Заменить гуид и удалить комментарий.

            var solutionDir = Directory.GetParent(Directory.GetCurrentDirectory());
            var tempDir = new DirectoryInfo(Path.Combine(solutionDir.FullName, "setup", "temp"));
            var setupDir = Path.Combine(solutionDir.FullName, "setup");

            var project = new Project
            {
                Name = projectName,
                OutDir = setupDir,
                Platform = Platform.x64,
                UI = WUI.WixUI_InstallDir,
                Version = version,
                OutFileName = outFileName,
                InstallScope = InstallScope.perUser,
                MajorUpgrade = MajorUpgrade.Default,
                GUID = guid,
                ControlPanelInfo =
                {
                    Manufacturer = "Synergy Systems",
                    HelpLink = "paltynnikov@synsys.co",
                },
                Dirs = new Dir[]
                {
                    new InstallDir(@"%AppDataFolder%\Autodesk\Revit\Addins\", GenerateWixEntities(tempDir.FullName))
                }
            };

            MajorUpgrade.Default.AllowSameVersionUpgrades = true;
            project.RemoveDialogsBetween(NativeDialogs.WelcomeDlg, NativeDialogs.VerifyReadyDlg);
            project.BuildMsi();

            Directory.Delete(tempDir.FullName, true);
        }

        private static WixEntity[] GenerateWixEntities(string releaseDir)
        {
            WriteLogInConsole(releaseDir);
            return new Files().GetAllItems(releaseDir);
        }

        private static void WriteLogInConsole(string releaseDir)
        {
            foreach (var dir in new DirectoryInfo(releaseDir).GetDirectories())
            {
                Console.WriteLine($"Added '{dir.Name}' version files: ");
                var filesInfo = dir.GetFiles("*", SearchOption.AllDirectories);
                foreach (var file in filesInfo)
                {
                    Console.WriteLine(file.FullName);
                }
            }
        }
    }
}
