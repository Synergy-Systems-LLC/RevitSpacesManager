using System;
using System.IO;
using System.Windows;

using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using Autodesk.Revit.Attributes;

using RevitSpacesManager.Views;
using RevitSpacesManager.ViewModels;

namespace SynSys.RevitSpacesManager.Revit
{
    [Transaction(TransactionMode.Manual)]
    [Regeneration(RegenerationOption.Manual)]
    internal class Command : IExternalCommand
    {
        // Диагностика краша Revit 2025: пишет шаги и фатальные исключения в %APPDATA%\RevitSpacesManager.log
        static Command() => Log("Command type loaded");

        internal static void Log(string text)
        {
            try { File.AppendAllText(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "RevitSpacesManager.log"), $"{DateTime.Now:HH:mm:ss.fff} {text}{Environment.NewLine}"); }
            catch { }
        }

        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {
            try
            {
                AppDomain.CurrentDomain.UnhandledException += (s, e) => Log($"FATAL: {e.ExceptionObject}");
                Log("Execute start");
                RevitManager.CommandData = commandData;

                if (IsCorrectActiveView())
                {
                    ShowMainWindow();
                }
                else
                {
                    ShowActiveViewError();
                }

                return Result.Succeeded;
            }
            catch (Exception ex)
            {
                Log($"CATCH: {ex}");
                MessageBox.Show($"{ex.Message}\n\n{ex.StackTrace}", "ERROR!");
                return Result.Failed;
            }
        }

        private void ShowMainWindow()
        {
            Log("ViewModel");
            var viewModel = new MainWindowViewModel();
            Log("Window");
            MainWindow mainWindow = new MainWindow { DataContext = viewModel };
            Log("ShowDialog");
            // Revit 2025+ (.NET 8) падает на необработанных исключениях диспетчера окна
            mainWindow.Dispatcher.UnhandledException += (s, e) =>
            {
                Log($"DISPATCHER: {e.Exception}");
                MessageBox.Show($"{e.Exception.Message}\n\n{e.Exception.StackTrace}", "ERROR!");
                e.Handled = true;
            };
            mainWindow.ShowDialog();
        }

        private bool IsCorrectActiveView()
        {
            View activeView = RevitManager.Document.ActiveView;
            Parameter activeViewPhase = activeView.get_Parameter(BuiltInParameter.VIEW_PHASE);
            if (activeViewPhase == null)
                return false;
            return true;
        }

        private void ShowActiveViewError()
        {
            string message = "There is no special Phase in the currently active View.\nPlease open definite View and relaunch the Revit Spaces\nManager Add-In.";
            string title = "ERROR!";
            MessageBox.Show(message, title);
        }
    }
}
