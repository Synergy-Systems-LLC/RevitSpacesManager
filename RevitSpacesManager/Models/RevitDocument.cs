using Autodesk.Revit.DB;
using Autodesk.Revit.DB.Architecture;
using Autodesk.Revit.DB.Mechanical;
using RevitSpacesManager.Models;
using System.Collections.Generic;
using System.Linq;

namespace RevitSpacesManager.Revit.Services
{
    internal class RevitDocument
    {
        public string RoomsItemName => $"{NumberOfRooms} Room{PluralSuffix(NumberOfRooms)} - {Title}";
        internal string Title => _document.Title;
        internal int NumberOfSpaces => Spaces.Count;
        internal int NumberOfRooms => Rooms.Count;
        internal List<SpaceElement> Spaces { get; set; }
        internal List<RoomElement> Rooms { get; set; }
        internal List<PhaseElement> Phases { get; set; }

        private readonly Document _document;
        private readonly List<Workset> _userWorksets;
        private readonly List<LevelElement> _levels;

        internal RevitDocument(Document doc)
        {
            _document = doc;
            _userWorksets = GetUserWorksets(doc);
            _levels = GetLevels(doc);

            Spaces = GetSpaces(doc);
            Rooms = GetRooms(doc);
            Phases = GetPhases(doc);

            SortSpacesByPhase();
            SortRoomsByPhase();
        }

        internal List<RevitDocument> GetRevitLinkDocuments()
        {
            FilteredElementCollector elementCollector = new FilteredElementCollector(_document);
            IList<Element> elements = elementCollector.OfClass(typeof(RevitLinkInstance)).WhereElementIsNotElementType().ToElements();
            List<RevitDocument> revitLinkDocuments = new List<RevitDocument>();
            foreach (Element element in elements)
            {
                RevitLinkInstance revitLinkInstance = element as RevitLinkInstance;
                Document linkDocument = revitLinkInstance.GetLinkDocument();
                RevitDocument revitLinkDocument = new RevitDocument(linkDocument);
                revitLinkDocuments.Add(revitLinkDocument);
            }
            return revitLinkDocuments;
        }

        internal bool DoesUserWorksetExist(string worksetName)
        {
            foreach (Workset workset in _userWorksets)
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
            foreach (Workset workset in _userWorksets)
            {
                if (workset.Name == worksetName)
                {
                    return workset.Id.IntegerValue;
                }
            }
            return 0;
        }

        private List<Workset> GetUserWorksets(Document document)
        {
            List<Workset> userWorksets = new List<Workset>();
            FilteredWorksetCollector worksetCollector = new FilteredWorksetCollector(document);
            FilteredWorksetCollector userWorksetCollector = worksetCollector.OfKind(WorksetKind.UserWorkset);
            foreach (Workset workset in userWorksetCollector)
            {
                userWorksets.Add(workset);
            }
            return userWorksets;
        }

        private List<LevelElement> GetLevels(Document document)
        {
            FilteredElementCollector elementCollector = new FilteredElementCollector(document);
            IList<Element> elements = elementCollector.OfClass(typeof(Level)).WhereElementIsNotElementType().ToElements();
            List<LevelElement>  levels = new List<LevelElement>();
            foreach (Element element in elements)
            {
                Level level = element as Level;
                LevelElement levelElement = new LevelElement(level);
                levels.Add(levelElement);
            }
            return levels;
        }

        private List<SpaceElement> GetSpaces(Document document)
        {
            FilteredElementCollector elementCollector = new FilteredElementCollector(document);
            IList<Element> elements = elementCollector.OfCategory(BuiltInCategory.OST_MEPSpaces).WhereElementIsNotElementType().ToElements();
            List<SpaceElement>  spaces = new List<SpaceElement>();
            foreach (Element element in elements)
            {
                Space space = element as Space;
                SpaceElement spaceElement = new SpaceElement(space);
                spaces.Add(spaceElement);
            }
            return spaces;
        }

        private List<RoomElement> GetRooms(Document document)
        {
            FilteredElementCollector elementCollector = new FilteredElementCollector(document);
            IList<Element> elements = elementCollector.OfCategory(BuiltInCategory.OST_Rooms).WhereElementIsNotElementType().ToElements();
            List<RoomElement>  rooms = new List<RoomElement>();
            foreach (Element element in elements)
            {
                Room room = element as Room;
                RoomElement roomElement = new RoomElement(room);
                rooms.Add(roomElement);
            }
            return rooms;
        }

        private List<PhaseElement> GetPhases(Document document)
        {
            PhaseArray phaseArray = document.Phases;
            List<PhaseElement>  phases = new List<PhaseElement>();
            foreach (Phase phase in phaseArray)
            {
                PhaseElement phaseElement = new PhaseElement(phase);
                phases.Add(phaseElement);
            }
            return phases;
        }
        
        private void SortSpacesByPhase()
        {
            foreach (PhaseElement phase in Phases)
            {
                List<SpaceElement> phaseSpaces = Spaces.Where(s => s.PhaseName == phase.Name).ToList();
                phase.Spaces = phaseSpaces;
            }
        }

        private void SortRoomsByPhase()
        {
            foreach (PhaseElement phase in Phases)
            {
                List<RoomElement> phaseRooms = Rooms.Where(r => r.PhaseName == phase.Name).ToList();
                phase.Rooms = phaseRooms;
            }
        }

        private string PluralSuffix(int number)
        {
            if (number == 1)
                return " ";
            return "s";
        }
    }
}
