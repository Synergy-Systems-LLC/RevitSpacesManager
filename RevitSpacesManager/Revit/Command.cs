using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using RevitSpacesManager.Views;
using System.Windows;

namespace RevitSpacesManager.Revit
{
    [TransactionAttribute(TransactionMode.Manual)]
    [RegenerationAttribute(RegenerationOption.Manual)]
    internal class Command : IExternalCommand
    {
        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {
            RevitManager.CommandData = commandData;

            if(IsCorrectActiveView())
            {
                ShowMainWindow();
            }
            else
            {
                ShowActiveViewError();
            }

            return Result.Succeeded;
        }

        private void ShowMainWindow()
        {
            MainWindow mainWindow = new MainWindow();
            mainWindow.ShowDialog();
        }

        private bool IsCorrectActiveView()
        {
            View activeView = RevitManager.Document.ActiveView;
            Parameter activeViewPhase = activeView.get_Parameter(BuiltInParameter.VIEW_PHASE);
            if (activeViewPhase == null)
            {
                return false;
            }
            return true;
        }

        private void ShowActiveViewError()
        {
            string message = "There is no special Phase in the currently active View. Please open definite View and relaunch the Revit Spaces Manager addin.";
            string title = "ERROR!"; 
            MessageBox.Show(message, title);
        }
    }
}
