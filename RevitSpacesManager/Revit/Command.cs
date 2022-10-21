using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using RevitSpacesManager.Views;

namespace RevitSpacesManager.Revit
{
    [TransactionAttribute(TransactionMode.Manual)]
    [RegenerationAttribute(RegenerationOption.Manual)]
    internal class Command : IExternalCommand
    {
        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {
            RevitManager.CommandData = commandData;

            MainWindow mainWindow = new MainWindow();
            mainWindow.ShowDialog();

            return Result.Succeeded;
        }
    }
}
