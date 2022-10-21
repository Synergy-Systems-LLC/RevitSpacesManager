using Autodesk.Revit.DB;

namespace RevitSpacesManager.Models
{
    internal class RevitLinkElement
    {
        internal RevitLinkInstance RevitLinkInstance { get; set; }
        internal Document Document { get; set; }

        internal RevitLinkElement(RevitLinkInstance revitLinkInstance)
        {
            RevitLinkInstance = revitLinkInstance;
            Document = RevitLinkInstance.GetLinkDocument();
        }
    }
}
