using Autodesk.Revit.DB;
using System.Collections.Generic;

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

        internal int GetUserWorksetIntegerIdByName(string worksetName)
        {
            foreach (Workset workset in _userWorksetCollector)
            {
                if (workset.Name == worksetName)
                {
                    return workset.Id.IntegerValue;
                }
            }
            return 0;
        }

        internal List<Level> GetLevels()
        {
            List<Level> levels = new List<Level>();
            IList<Element> elements = _elementCollector.OfClass(typeof(Level)).WhereElementIsNotElementType().ToElements();
            foreach (Element element in elements)
            {
                Level level = element as Level;
                levels.Add(level);
            }
            return levels;
        }
    }
}
