using Autodesk.Revit.DB;

namespace RevitSpacesManager.Revit.Services
{
    internal class RevitDocumentServices
    {
        private Document _document;
        private FilteredElementCollector _elementCollector;
        private FilteredWorksetCollector _worksetCollector;
        private FilteredWorksetCollector _userWorksetCollector;


        internal RevitDocumentServices(Document doc)
        {
            _document = doc;
            _elementCollector = new FilteredElementCollector(_document);
            _worksetCollector = new FilteredWorksetCollector(_document);
            _userWorksetCollector = _worksetCollector.OfKind(WorksetKind.UserWorkset);
        }

        internal bool DoesUserWorksetExist(string worksetName)
        {
            foreach(Workset workset in _userWorksetCollector)
            {
                if(workset.Name == worksetName)
                {
                    return true;
                }
            }
            return false;
        }
    }
}
