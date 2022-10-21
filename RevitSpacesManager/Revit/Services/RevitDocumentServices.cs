using Autodesk.Revit.DB;
using System.Collections.Generic;

namespace RevitSpacesManager.Revit.Services
{
    internal class RevitDocumentServices
    {
        private readonly Document _document;
        private readonly FilteredElementCollector _elementCollector;
        private readonly FilteredWorksetCollector _worksetCollector;
        private readonly FilteredWorksetCollector _userWorksetCollector;


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

        internal List<RevitLinkInstance> GetRevitLinkInstances()
        {
            List<RevitLinkInstance> revitLinkInstances = new List<RevitLinkInstance>();
            IList<Element> elements = _elementCollector.OfClass(typeof(RevitLinkInstance)).ToElements();
            foreach (Element element in elements)
            {
                RevitLinkInstance revitLinkInstance = element as RevitLinkInstance;
                revitLinkInstances.Add(revitLinkInstance);
            }
            return revitLinkInstances;
        }
    }
}
