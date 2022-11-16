using System;
using System.Collections.Generic;
using System.Linq;

namespace RevitSpacesManager.Models
{
    internal class SpacesModel : IModel
    {
        public RevitDocument RevitDocument { get; set; }
        public int NumberOfElements => RevitDocument.NumberOfSpaces;
        public string ObjectName { get; }
        public string SelectedPhaseDisplayPath { get; }


        internal SpacesModel(RevitDocument revitDocument)
        {
            RevitDocument = revitDocument;
            ObjectName = "Space";
            SelectedPhaseDisplayPath = $"{ObjectName}sItemName";
        }


        public List<PhaseElement> GetPhases() => RevitDocument.Phases.Where(p => p.NumberOfSpaces > 0).ToList();

        public void CreateAll()
        {
            throw new NotImplementedException();
        }

        public void CreateSelected()
        {
            throw new NotImplementedException();
        }

        public void DeleteAll()
        {
            throw new NotImplementedException();
        }

        public void DeleteSelected()
        {
            throw new NotImplementedException();
        }
    }
}
