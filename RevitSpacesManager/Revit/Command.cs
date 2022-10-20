using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;

namespace RevitSpacesManager.Revit
{
    [TransactionAttribute(TransactionMode.Manual)]
    [RegenerationAttribute(RegenerationOption.Manual)]
    internal class Command : IExternalCommand
    {
        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {
            RevitManager.CommandData = commandData;

            if (UserRefusedRun())
            {
                return Result.Cancelled;
            }
            TaskDialog.Show("info", $"Тестовый запуск из документа {RevitManager.Document.Title}");

            return Result.Succeeded;
        }

        /// <summary>
        /// Данное окно является предохранителем от случайного нажатия на кнопку плагина на панели в Revit.
        /// Оно является обязательным для плагинов без GUI.
        /// Если в плагине есть GUI данное окно должно быть удалено.
        /// </summary>
        /// <returns></returns>
        private bool UserRefusedRun()
        {
            var dialog = new TaskDialog("Start")
            {
                MainInstruction = "Хотите запустить плагин?",
                CommonButtons = TaskDialogCommonButtons.Yes | TaskDialogCommonButtons.No
            };

            if (dialog.Show() == TaskDialogResult.No)
            {
                return true;
            }

            return false;
        }
    }
}
