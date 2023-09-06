﻿using System;
using System.IO;
using System.Reflection;
using WixSharp;
using WixSharp.CommonTasks;
using WixSharp.Controls;

namespace WixSharpSetup
{
    internal class Program
    {
        private static void Main()
        {
#if SynSys
            var customer = "SynSys";
            var helpLink = "info@synsys.co";
            var guid = new Guid("9879281f-1f82-42a1-bf75-177e77f929c2");
#endif
            var developCompany = "Synergy Systems";
            var pluginName = "RevitSpacesManager";
            var revitVersion = "2023";
            var pathToMainPluginAssembly =
                $@"..\{customer}.{pluginName}\bin\{customer}_R{revitVersion}\{customer}.{pluginName}.dll";

            // Это нужно чтобы не зависеть от проектов заказчиков
            AssemblyName pluginAssemblyName = System.Reflection.Assembly
                .LoadFrom(pathToMainPluginAssembly)
                .GetName();

            DirectoryInfo solutionDir = Directory.GetParent(Directory.GetCurrentDirectory());
            var tempDir = new DirectoryInfo(Path.Combine(solutionDir.FullName, "setup", "temp"));
            string setupDir = Path.Combine(solutionDir.FullName, "setup");

            var project = new Project
            {
                Name = pluginAssemblyName.Name,
                Version = pluginAssemblyName.Version,
                OutFileName = $"{pluginAssemblyName.Name}-{pluginAssemblyName.Version}",
                OutDir = setupDir,
                GUID = guid,
                Platform = Platform.x64,
                UI = WUI.WixUI_InstallDir,
                InstallScope = InstallScope.perUser,
                MajorUpgrade = MajorUpgrade.Default,
                ControlPanelInfo =
                {
                    Manufacturer = developCompany,
                    HelpLink = helpLink
                },
                Dirs = new Dir[]
                {
                    new InstallDir(
                        @"%AppDataFolder%\Autodesk\Revit\Addins\",
                        GenerateWixEntities(tempDir.FullName)
                    )
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
