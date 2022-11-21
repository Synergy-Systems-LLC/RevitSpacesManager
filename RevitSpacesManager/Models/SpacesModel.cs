using System.Collections.Generic;
using System.Linq;
using System.Windows;

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


        public override void CreateAll()
        {
            MessageBox.Show("СОЗДАНИЕ...");
        }

        public override void CreateSelected()
        {
            MessageBox.Show("СОЗДАНИЕ...");
        }

        public override void DeleteAll(string activeObject)
        {
            List<RevitElement> elements = _revitDocument.Spaces.Cast<RevitElement>().ToList();
            string transactionName = $"Delete All {activeObject}s";
            RevitServices.DeleteElements(_revitDocument.Document, elements, transactionName);
            _revitDocument.RefreshPhasesRoomsAndSpaces();
        }

        public override void DeleteSelected(string activeObject, PhaseElement phaseElement)
        {
            List<RevitElement> elements = phaseElement.Spaces.Cast<RevitElement>().ToList();
            string phaseName = phaseElement.Name;
            string transactionName = $"Delete '{phaseName}' phase {activeObject}s";
            RevitServices.DeleteElements(_revitDocument.Document, elements, transactionName);
            _revitDocument.RefreshPhasesRoomsAndSpaces();
        }

        internal override List<PhaseElement> GetPhases() => _revitDocument.Phases.Where(p => p.NumberOfSpaces > 0).ToList();
    }
}
