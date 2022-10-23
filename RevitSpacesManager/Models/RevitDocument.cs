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
        public string Title { get; set; }
        public int NumberOfSpaces { get => Spaces.Count; }
        public int NumberOfRooms { get => Rooms.Count; }
        public string RoomsItemName
        {
            get
            {
                if (NumberOfRooms == 1)
                    return $"{Rooms.Count} Room  - {Title}";
                return $"{Rooms.Count} Rooms - {Title}";
            }
        }

        internal List<Workset> UserWorksets { get; set; }
        internal List<LevelElement> Levels { get; set; }
        internal List<SpaceElement> Spaces { get; set; }
        internal List<RoomElement> Rooms { get; set; }
        internal List<PhaseElement> Phases { get; set; }

        private readonly Document _document;

        internal RevitDocument(Document doc)
        {
            _document = doc;
            Title = _document.Title;

            GetUserWorksets();
            GetLevels();
            GetSpaces();
            GetRooms();
            GetPhases();
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
            foreach (Workset workset in UserWorksets)
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
            foreach (Workset workset in UserWorksets)
            {
                if (workset.Name == worksetName)
                {
                    return workset.Id.IntegerValue;
                }
            }
            return 0;
        }

        private void GetUserWorksets()
        {
            UserWorksets = new List<Workset>();
            FilteredWorksetCollector worksetCollector = new FilteredWorksetCollector(_document);
            FilteredWorksetCollector userWorksetCollector = worksetCollector.OfKind(WorksetKind.UserWorkset);
            foreach (Workset workset in userWorksetCollector)
            {
                UserWorksets.Add(workset);
            }
        }

        private void GetLevels()
        {
            FilteredElementCollector elementCollector = new FilteredElementCollector(_document);
            IList<Element> elements = elementCollector.OfClass(typeof(Level)).WhereElementIsNotElementType().ToElements();
            Levels = new List<LevelElement>();
            foreach (Element element in elements)
            {
                Level level = element as Level;
                LevelElement levelElement = new LevelElement(level);
                Levels.Add(levelElement);
            }
        }

        private void GetSpaces()
        {
            FilteredElementCollector elementCollector = new FilteredElementCollector(_document);
            IList<Element> elements = elementCollector.OfCategory(BuiltInCategory.OST_MEPSpaces).WhereElementIsNotElementType().ToElements();
            Spaces = new List<SpaceElement>();
            foreach (Element element in elements)
            {
                Space space = element as Space;
                SpaceElement spaceElement = new SpaceElement(space);
                Spaces.Add(spaceElement);
            }
        }

        private void GetRooms()
        {
            FilteredElementCollector elementCollector = new FilteredElementCollector(_document);
            IList<Element> elements = elementCollector.OfCategory(BuiltInCategory.OST_Rooms).WhereElementIsNotElementType().ToElements();
            Rooms = new List<RoomElement>();
            foreach (Element element in elements)
            {
                Room room = element as Room;
                RoomElement roomElement = new RoomElement(room);
                Rooms.Add(roomElement);
            }
        }

        private void GetPhases()
        {
            PhaseArray phaseArray = _document.Phases;
            Phases = new List<PhaseElement>();
            foreach (Phase phase in phaseArray)
            {
                PhaseElement phaseElement = new PhaseElement(phase);
                Phases.Add(phaseElement);
            }
        }
        
        private void SortSpacesByPhase()
        {
            foreach (PhaseElement phase in Phases)
            {
                List<SpaceElement> phaseSpaces = Spaces.Where(s => s.PhaseName == phase.Name).ToList();
                phase.SyncSpaces(phaseSpaces);
            }
        }

        private void SortRoomsByPhase()
        {
            foreach (PhaseElement phase in Phases)
            {
                List<RoomElement> phaseRooms = Rooms.Where(r => r.PhaseName == phase.Name).ToList();
                phase.SyncRooms(phaseRooms);
            }
        }
    }
}
