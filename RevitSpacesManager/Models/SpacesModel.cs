using System;
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


        public override void CreateAll()
        {
            throw new NotImplementedException();
        }

        public override void CreateSelected()
        {
            throw new NotImplementedException();
        }

        public override void DeleteAll()
        {
            throw new NotImplementedException();
        }

        public override void DeleteSelected()
        {
            throw new NotImplementedException();
        }

        internal override List<PhaseElement> GetPhases() => _revitDocument.Phases.Where(p => p.NumberOfSpaces > 0).ToList();
    }
}
