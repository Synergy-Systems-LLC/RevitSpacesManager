using Autodesk.Revit.ApplicationServices;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;

namespace SynSys.RevitSpacesManager.Revit
{
    public class RevitManager
    {
        public static ExternalCommandData CommandData { get; internal set; }
        public static UIApplication UIApplication { get => CommandData.Application; }
        public static Application Application { get => CommandData.Application.Application; }
        public static UIDocument UIDocument { get => CommandData.Application.ActiveUIDocument; }
        public static Document Document { get => CommandData.Application.ActiveUIDocument.Document; }
    }
}
