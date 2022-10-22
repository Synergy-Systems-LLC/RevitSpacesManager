using Autodesk.Revit.DB;
using Autodesk.Revit.DB.Architecture;
using Autodesk.Revit.DB.Mechanical;
using System.Collections.Generic;

namespace RevitSpacesManager.Revit.Services
{
    internal class RevitDocumentServices
    {
        private readonly Document _document;
        private readonly FilteredWorksetCollector _worksetCollector;
        private readonly FilteredWorksetCollector _userWorksetCollector;


        internal RevitDocumentServices(Document doc)
        {
            _document = doc;
            _worksetCollector = new FilteredWorksetCollector(_document);
            _userWorksetCollector = _worksetCollector.OfKind(WorksetKind.UserWorkset);
        }

        internal bool DoesUserWorksetExist(string worksetName)
        {
            foreach (Workset workset in _userWorksetCollector)
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
            FilteredElementCollector elementCollector = new FilteredElementCollector(_document);
            IList<Element> elements = elementCollector.OfClass(typeof(Level)).WhereElementIsNotElementType().ToElements();
            List<Level> levels = new List<Level>();
            foreach (Element element in elements)
            {
                Level level = element as Level;
                levels.Add(level);
            }
            return levels;
        }

        internal List<RevitLinkInstance> GetRevitLinkInstances()
        {
            FilteredElementCollector elementCollector = new FilteredElementCollector(_document);
            IList<Element> elements = elementCollector.OfClass(typeof(RevitLinkInstance)).WhereElementIsNotElementType().ToElements();
            List<RevitLinkInstance> revitLinkInstances = new List<RevitLinkInstance>();
            foreach (Element element in elements)
            {
                RevitLinkInstance revitLinkInstance = element as RevitLinkInstance;
                revitLinkInstances.Add(revitLinkInstance);
            }
            return revitLinkInstances;
        }

        internal List<Space> GetMEPSpaces()
        {
            FilteredElementCollector elementCollector = new FilteredElementCollector(_document);
            IList<Element> elements = elementCollector.OfCategory(BuiltInCategory.OST_MEPSpaces).WhereElementIsNotElementType().ToElements();
            List<Space> spaces = new List<Space>();
            foreach (Element element in elements)
            {
                Space space = element as Space;
                spaces.Add(space);
            }
            return spaces;
        }

        internal List<Room> GetRooms()
        {
            FilteredElementCollector elementCollector = new FilteredElementCollector(_document);
            IList<Element> elements = elementCollector.OfCategory(BuiltInCategory.OST_Rooms).WhereElementIsNotElementType().ToElements();
            List<Room> rooms = new List<Room>();
            foreach (Element element in elements)
            {
                Room room = element as Room;
                rooms.Add(room);
            }
            return rooms;
        }
    }
}
