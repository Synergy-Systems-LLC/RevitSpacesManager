using Autodesk.Revit.DB;
using Autodesk.Revit.DB.Architecture;
using Autodesk.Revit.DB.Mechanical;
using System;
using System.Collections.Generic;
using System.Linq;

namespace RevitSpacesManager.Models
{
    internal class RevitDocument : IRoomLevelsMatchable
    {
        public string RoomsItemName => $"{NumberOfRooms} Room{PluralSuffix(NumberOfRooms)} - {Title}";

        internal string Title => _document.Title;
        internal string ActiveViewPhaseName => _document.ActiveView.get_Parameter(BuiltInParameter.VIEW_PHASE).AsValueString();
        internal List<PhaseElement> Phases { get; set; } 
        internal List<SpaceElement> Spaces => GetSpaces(Phases);
        public List<RoomElement> Rooms => GetRooms(Phases);
        internal int NumberOfSpaces => Spaces.Count;
        internal int NumberOfRooms => Rooms.Count;
        
        private readonly Document _document;
        private readonly List<LevelElement> _levels;


        internal RevitDocument(Document document)
        {
            _document = document;
            _levels = GetLevels(document);
            Phases = GetPhasesWithRoomsAndSpaces(_document);
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
                if(linkDocument != null)
                {
                    RevitDocument revitLinkDocument = new RevitDocument(linkDocument);
                    revitLinkDocuments.Add(revitLinkDocument);
                }
            }
            return revitLinkDocuments;
        }

        internal void RefreshPhasesRoomsAndSpaces()
        {
            Phases = GetPhasesWithRoomsAndSpaces(_document);
        }

        internal bool AreNotAllElementsEditable(List<RevitElement> elementsList)
        {
            return AreNotAllRevitElementsEditable(_document, elementsList); ;
        }

        internal void DeleteElements(List<RevitElement> elementsList, string transactionName)
        {
            DocumentTransaction(elementsList, DeleteRevitElements, transactionName);
        }

        internal bool DoesUserWorksetExist(string worksetName)
        {
            List<Workset> userWorksets = GetUserWorksets(_document);
            foreach (Workset workset in userWorksets)
            {
                if (workset.Name == worksetName)
                {
                    return true;
                }
            }
            return false;
        }
        
        internal void MatchLevels(List<RoomElement> roomElements)
        {
            ClearLevelsMatching();
            List<LevelElement> linkedMatchingLevels = GetLevelsForMatching(roomElements);

            foreach (LevelElement linkedLevel in linkedMatchingLevels)
            {
                IEnumerable<LevelElement> currentModelMatchingLevels = _levels.Where(l => l.Name == linkedLevel.Name);
                if (currentModelMatchingLevels.Any())
                {
                    LevelElement currentModelLevel = currentModelMatchingLevels.First();
                    currentModelLevel.CompareByElevationWith(linkedLevel);
                    linkedLevel.CompareByElevationWith(currentModelLevel);
                }
            }
        }

        internal bool IsLevelNotAvailable(LevelElement linkedLevel)
        {
            IEnumerable<LevelElement> availableLevels = _levels.Where(l => l.MatchedLevelId == linkedLevel.Id);
            if (availableLevels.Any())
                return false;
            return true;
        }

        internal void CreateSpacesByRooms(List<RevitElement> elementsList, string transactionName)
        {
            DocumentTransaction(elementsList, CreateSpacesByRevitElements, transactionName);
        }

        internal void CreateRoomsByRooms(List<RevitElement> elementsList, string transactionName)
        {
            DocumentTransaction(elementsList, CreateRoomsByRevitElements, transactionName);
        }
        
        internal int GetUserWorksetIntegerIdByName(string worksetName)
        {
            List<Workset> userWorksets = GetUserWorksets(_document);
            foreach (Workset workset in userWorksets)
            {
                if (workset.Name == worksetName)
                {
                    return workset.Id.IntegerValue;
                }
            }
            return 0;
        }


        private void CreateSpacesByRevitElements(Document document, List<RevitElement> elementsList)
        {
            foreach (RevitElement element in elementsList)
            {
                RoomElement room = element as RoomElement;

                //element = document.Create.NewSpace(level, UV(roomLocationPoint.X, roomLocationPoint.Y));
                //element.get_Parameter(BuiltInParameter.ELEM_PARTITION_PARAM).Set(worksetSpacesId);
                //document.Regenerate()
            }
        }

        private void CreateRoomsByRevitElements(Document document, List<RevitElement> elementsList)
        {
            foreach (RevitElement element in elementsList)
            {
                RoomElement room = element as RoomElement;

                //element = document.Create.NewRoom(level, UV(roomLocationPoint.X, roomLocationPoint.Y));
                //element.get_Parameter(BuiltInParameter.ELEM_PARTITION_PARAM).Set(self.worksetRoomsId);
                //document.Regenerate()
            }
        }

        private void DeleteRevitElements(Document document, List<RevitElement> elementsList)
        {
            foreach (RevitElement element in elementsList)
            {
                document.Delete(element.ElementId);
            }
        }

        private void DocumentTransaction(
            List<RevitElement> elementsList,
            Action<Document, List<RevitElement>> action,
            string transactionName = "RevitSpaceManager Transaction"
            )
        {
            using (Transaction transaction = new Transaction(_document, transactionName))
            {
                transaction.Start();
                action(_document, elementsList);
                transaction.Commit();
            }
        }

        private void ClearLevelsMatching()
        {
            foreach (LevelElement levelElement in _levels)
            {
                levelElement.ClearMatching();
            }
        }

        private List<LevelElement> GetLevelsForMatching(List<RoomElement> roomElements)
        {
            List<LevelElement> roomLevelsForMatching = roomElements.Select(r => r.Level).ToList();
            List<LevelElement> upperLimitsForMatching = roomElements.Select(r => r.UpperLimit).ToList();
            roomLevelsForMatching.AddRange(upperLimitsForMatching);
            List<LevelElement> levelsForMatching = roomLevelsForMatching.GroupBy(l => l.Id).Select(l => l.First()).ToList();
            return levelsForMatching;
        }

        private bool AreNotAllRevitElementsEditable(Document document, List<RevitElement> elementsList)
        {
            foreach (RevitElement element in elementsList)
            {
                if (IsRevitElementNotEditable(document, element))
                    return true;
            }
            return false;
        }

        private bool IsRevitElementNotEditable(Document document, RevitElement element)
        {
            ElementId elementId = element.ElementId;
            CheckoutStatus status = WorksharingUtils.GetCheckoutStatus(document, elementId);
            if (status == CheckoutStatus.OwnedByOtherUser)
                return true;
            return false;
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
        
        private string PluralSuffix(int number)
        {
            if (number == 1)
                return " ";
            return "s";
        }
    }
}
