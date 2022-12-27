using System.Collections.Generic;
using System.Linq;

namespace RevitSpacesManager.Models
{
    internal class SpacesModel : AreaModel
    {
        internal override int NumberOfElements => _revitDocument.NumberOfSpaces;
        internal override RevitDocument RevitDocument => _revitDocument;
        private readonly RevitDocument _revitDocument;
        private const string _areaName = "Space";


        internal SpacesModel(RevitDocument revitDocument)
        {
            _revitDocument = revitDocument;
        }


        public override void CreateAllByRooms(List<RoomElement> roomElements)
        {
            List<RevitElement> elements = roomElements.Cast<RevitElement>().ToList();
            string transactionName = "Create All Spaces";
            _revitDocument.CreateSpacesByRooms(elements, transactionName);
            _revitDocument.RefreshPhasesRoomsAndSpaces();
        }

        public override void CreateByPhaseRooms(List<RoomElement> roomElements, PhaseElement phaseElement)
        {
            List<RevitElement> elements = roomElements.Cast<RevitElement>().ToList();
            string phaseName = phaseElement.Name;
            string transactionName = $"Create Spaces by '{phaseName}' phase Rooms";
            _revitDocument.CreateSpacesByRooms(elements, transactionName);
            _revitDocument.RefreshPhasesRoomsAndSpaces();
        }

        public override void DeleteAll()
        {
            List<RevitElement> elements = _revitDocument.Spaces.Cast<RevitElement>().ToList();
            string transactionName = "Delete All Spaces";
            _revitDocument.DeleteElements(elements, transactionName);
            _revitDocument.RefreshPhasesRoomsAndSpaces();
        }

        public override void DeleteByPhase(PhaseElement phaseElement)
        {
            List<RevitElement> elements = phaseElement.Spaces.Cast<RevitElement>().ToList();
            string phaseName = phaseElement.Name;
            string transactionName = $"Delete '{phaseName}' phase Spaces";
            _revitDocument.DeleteElements(elements, transactionName);
            _revitDocument.RefreshPhasesRoomsAndSpaces();
        }

        public override bool IsWorksetNotAvailable() => !_revitDocument.DoesUserWorksetExist("Model Spaces");

        public override bool AreNotAllElementsEditable()
        {
            List<RevitElement> elements = _revitDocument.Spaces.Cast<RevitElement>().ToList();
            return _revitDocument.AreNotAllElementsEditable(elements);
        }

        public override bool AreNotAllPhaseElementsEditable(PhaseElement phaseElement)
        {
            List<RevitElement> elements = phaseElement.Spaces.Cast<RevitElement>().ToList();
            return _revitDocument.AreNotAllElementsEditable(elements);
        }

        internal override List<PhaseElement> GetPhases() => _revitDocument.Phases.Where(p => p.NumberOfSpaces > 0).ToList();
        internal override string GetAreaName() => _areaName;
    }
}
