using System;
using System.Collections.Generic;
using System.Linq;

namespace RevitSpacesManager.Models
{
    internal class RoomsModel : IModel
    {
        public RevitDocument RevitDocument { get; set; }
        public int NumberOfElements => RevitDocument.NumberOfRooms;
        public string ObjectName { get; }
        public string SelectedPhaseDisplayPath { get; }


        internal RoomsModel(RevitDocument revitDocument)
        {
            RevitDocument = revitDocument;
            ObjectName = "Room";
            SelectedPhaseDisplayPath = $"{ObjectName}sItemName";
        }


        public List<PhaseElement> GetPhases() => RevitDocument.Phases.Where(p => p.NumberOfRooms > 0).ToList();

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
