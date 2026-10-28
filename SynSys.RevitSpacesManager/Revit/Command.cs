using System;
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
        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {
            try
            {
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
                MessageBox.Show($"{ex.Message}\n\n{ex.StackTrace}", "ERROR!");
                return Result.Failed;
            }
        }

        private void ShowMainWindow()
        {
            MainWindow mainWindow = new MainWindow { DataContext = new MainWindowViewModel() };
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
