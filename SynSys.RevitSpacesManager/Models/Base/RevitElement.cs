using Autodesk.Revit.DB;

namespace RevitSpacesManager.Models
{
    internal abstract class RevitElement
    {
        internal abstract ElementId ElementId { get; }
    }
}
