using Autodesk.Revit.DB;
using Autodesk.Revit.DB.Architecture;
using Autodesk.Revit.DB.Mechanical;
using System.Collections.Generic;
using System.Linq;

namespace RevitSpacesManager.Models
{
    internal class RevitDocument
    {
        public string RoomsItemName => $"{NumberOfRooms} Room{PluralSuffix(NumberOfRooms)} - {Title}";

        internal string Title => _document.Title;
        internal string ActiveViewPhaseName => _document.ActiveView.get_Parameter(BuiltInParameter.VIEW_PHASE).AsValueString();
        internal List<PhaseElement> Phases { get; set; } 
        internal List<SpaceElement> Spaces => GetSpaces(Phases);
        internal List<RoomElement> Rooms => GetRooms(Phases);
        internal int NumberOfSpaces => Spaces.Count;
        internal int NumberOfRooms => Rooms.Count;

        private readonly Document _document;
        private List<Workset> UserWorksets => GetUserWorksets(_document); //TODO Refactor later
        private List<LevelElement> Levels => GetLevels(_document); //TODO Refactor later


        internal RevitDocument(Document document)
        {
            _document = document;
            Phases = GetPhasesWithRoomsAndSpaces(_document);
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


        private List<PhaseElement> GetPhasesWithRoomsAndSpaces(Document document)
        {
            List<PhaseElement> phases = GetPhases(document);
            List<SpaceElement> spaces = GetSpaces(document);
            List<RoomElement> rooms = GetRooms(document);

            foreach (PhaseElement phase in phases)
            {
                phase.Spaces = spaces.Where(s => s.PhaseId == phase.Id).ToList();
                phase.Rooms = rooms.Where(r => r.PhaseId == phase.Id).ToList();
            }
            return phases;
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

        private List<SpaceElement> GetSpaces(List<PhaseElement> phases)
        {
            List<SpaceElement> spaces = new List<SpaceElement>();
            foreach (PhaseElement phase in phases)
            {
                spaces.AddRange(phase.Spaces);
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

        private List<RoomElement> GetRooms(List<PhaseElement> phases)
        {
            List<RoomElement> rooms = new List<RoomElement>();
            foreach (PhaseElement phase in phases)
            {
                rooms.AddRange(phase.Rooms);
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
        
        private string PluralSuffix(int number)
        {
            if (number == 1)
                return " ";
            return "s";
        }
    }
}
