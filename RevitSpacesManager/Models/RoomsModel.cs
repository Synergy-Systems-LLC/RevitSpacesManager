using System.Collections.Generic;
using System.Linq;

namespace RevitSpacesManager.Models
{
    internal class RoomsModel : AreaModel
    {
        internal override int NumberOfElements => _revitDocument.NumberOfRooms;

        private readonly RevitDocument _revitDocument;
        

        internal RoomsModel(RevitDocument revitDocument)
        {
            _revitDocument = revitDocument;
        }


        public override void CreateAllByLinkedDocument(RevitDocument linkDocument)
        {
            // TODO Check workset
            // TODO Check room correct
            // TODO - area !=0 (placed and enclosed)
            // TODO - level available
            // TODO - level same elevation
            // TODO - level upper limit available
            // TODO - level upper limit same elevation
            // TODO Report with IDs
            List<RevitElement> elements = linkDocument.Rooms.Cast<RevitElement>().ToList();
            string transactionName = "Create All Rooms";
            _revitDocument.CreateRoomsByRooms(elements, transactionName);
            _revitDocument.RefreshPhasesRoomsAndSpaces();
        }

        public override void CreateByLinkedDocumentPhase(PhaseElement phaseElement)
        {
            List<RevitElement> elements = phaseElement.Rooms.Cast<RevitElement>().ToList();
            string phaseName = phaseElement.Name;
            string transactionName = $"Create Rooms by '{phaseName}' phase";
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

        internal override List<PhaseElement> GetPhases() => _revitDocument.Phases.Where(p => p.NumberOfRooms > 0).ToList();

        internal override bool IsWorksetAvailable() => _revitDocument.DoesUserWorksetExist("Model Rooms");
    }
}
