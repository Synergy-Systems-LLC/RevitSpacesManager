using RevitSpacesManager.Models.Services;
using System.Collections.Generic;
using System.Linq;
using System.Windows;

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


        public override void CreateAll()
        {
            MessageBox.Show("СОЗДАНИЕ...");
        }

        public override void CreateSelected()
        {
            MessageBox.Show("СОЗДАНИЕ...");
        }

        public override void DeleteAll()
        {
            List<RevitElement> elements = _revitDocument.Rooms.Cast<RevitElement>().ToList();
            string transactionName = "Delete All Rooms";
            RevitServices.DeleteElements(_revitDocument.Document, elements, transactionName);
            _revitDocument.RefreshPhasesRoomsAndSpaces();
        }

        public override void DeleteSelected(PhaseElement phaseElement)
        {
            List<RevitElement> elements = phaseElement.Rooms.Cast<RevitElement>().ToList();
            string phaseName = phaseElement.Name;
            string transactionName = $"Delete '{phaseName}' phase Rooms";
            RevitServices.DeleteElements(_revitDocument.Document, elements, transactionName);
            _revitDocument.RefreshPhasesRoomsAndSpaces();
        }

        internal override List<PhaseElement> GetPhases() => _revitDocument.Phases.Where(p => p.NumberOfRooms > 0).ToList();
    }
}
