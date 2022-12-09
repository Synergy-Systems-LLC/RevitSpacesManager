using System.Collections.Generic;
using System.Linq;

namespace RevitSpacesManager.Models
{
    internal class SpacesModel : AreaModel
    {
        internal override int NumberOfElements => _revitDocument.NumberOfSpaces;

        private readonly RevitDocument _revitDocument;


        internal SpacesModel(RevitDocument revitDocument)
        {
            _revitDocument = revitDocument;
        }


        public override void CreateAllByLinkedDocument(RevitDocument linkDocument)
        {
            List<RevitElement> elements = _revitDocument.Spaces.Cast<RevitElement>().ToList();
            string transactionName = "Create All Spaces";
            _revitDocument.CreateSpacesByRooms(elements, transactionName);
            _revitDocument.RefreshPhasesRoomsAndSpaces();
        }

        public override void CreateByLinkedDocumentPhase(PhaseElement phaseElement)
        {
            List<RevitElement> elements = phaseElement.Spaces.Cast<RevitElement>().ToList();
            string phaseName = phaseElement.Name;
            string transactionName = $"Create Spaces by '{phaseName}' phase";
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

        internal override List<PhaseElement> GetPhases() => _revitDocument.Phases.Where(p => p.NumberOfSpaces > 0).ToList();

        internal override bool IsWorksetAvailable() => _revitDocument.DoesUserWorksetExist("Model Spaces");
    }
}
