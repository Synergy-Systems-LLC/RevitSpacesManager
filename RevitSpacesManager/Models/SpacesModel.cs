using System;
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

        public override void DeleteAll()
        {
            MessageBox.Show("УДАЛЕНИЕ...");
        }

        public override void DeleteSelected()
        {
            MessageBox.Show("УДАЛЕНИЕ...");
        }

        internal override List<PhaseElement> GetPhases() => _revitDocument.Phases.Where(p => p.NumberOfSpaces > 0).ToList();
    }
}
