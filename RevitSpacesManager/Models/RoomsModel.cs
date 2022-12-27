using System.Collections.Generic;
using System.Linq;

namespace RevitSpacesManager.Models
{
    internal class RoomsModel : AreaModel
    {
        internal override int NumberOfElements => _revitDocument.NumberOfRooms;
        internal override RevitDocument RevitDocument => _revitDocument;
        private readonly RevitDocument _revitDocument;
        private const string _areaName = "Room";


        internal RoomsModel(RevitDocument revitDocument)
        {
            _revitDocument = revitDocument;
        }


        public override void CreateAllByRooms(List<RoomElement> roomElements)
        {
            List<RevitElement> elements = roomElements.Cast<RevitElement>().ToList();
            string transactionName = "Create All Rooms";
            _revitDocument.CreateRoomsByRooms(elements, transactionName);
            _revitDocument.RefreshPhasesRoomsAndSpaces();
        }

        public override void CreateByPhaseRooms(List<RoomElement> roomElements, PhaseElement phaseElement)
        {
            List<RevitElement> elements = roomElements.Cast<RevitElement>().ToList();
            string phaseName = phaseElement.Name;
            string transactionName = $"Create Rooms by '{phaseName}' phase Rooms";
            _revitDocument.CreateRoomsByRooms(elements, transactionName);
            _revitDocument.RefreshPhasesRoomsAndSpaces();
        }

        public override void DeleteAll()
        {
            List<RevitElement> elements = _revitDocument.Rooms.Cast<RevitElement>().ToList();
            string transactionName = "Delete All Rooms";
            _revitDocument.DeleteElements(elements, transactionName);
            _revitDocument.RefreshPhasesRoomsAndSpaces();
        }

        public override void DeleteByPhase(PhaseElement phaseElement)
        {
            List<RevitElement> elements = phaseElement.Rooms.Cast<RevitElement>().ToList();
            string phaseName = phaseElement.Name;
            string transactionName = $"Delete '{phaseName}' phase Rooms";
            _revitDocument.DeleteElements(elements, transactionName);
            _revitDocument.RefreshPhasesRoomsAndSpaces();
        }

        public override bool IsWorksetNotAvailable() => !_revitDocument.DoesUserWorksetExist("Model Rooms");

        public override bool AreNotAllElementsEditable()
        {
            List<RevitElement> elements = _revitDocument.Rooms.Cast<RevitElement>().ToList();
            return _revitDocument.AreNotAllElementsEditable(elements);
        }

        public override bool AreNotAllPhaseElementsEditable(PhaseElement phaseElement)
        {
            List<RevitElement> elements = phaseElement.Rooms.Cast<RevitElement>().ToList();
            return _revitDocument.AreNotAllElementsEditable(elements);
        }

        internal override List<PhaseElement> GetPhases() => _revitDocument.Phases.Where(p => p.NumberOfRooms > 0).ToList();
        internal override string GetAreaName() => _areaName;
    }
}
