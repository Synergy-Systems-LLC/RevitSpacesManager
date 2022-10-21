using Autodesk.Revit.DB;

namespace RevitSpacesManager.Revit.Services
{
    internal class RevitDocumentServices
    {
        private Document _document;
        private FilteredElementCollector _elementCollector;
        private FilteredWorksetCollector _worksetCollector;

        internal RevitDocumentServices(Document doc)
        {
            _document = doc;
            _elementCollector = new FilteredElementCollector(doc);
            _worksetCollector = new FilteredWorksetCollector(doc);
        }
    }
}
