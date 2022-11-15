using System;

namespace RevitSpacesManager.Models
{
    internal class SpacesModel : IModel
    {
        public RevitDocument RevitDocument { get; set; }

        internal SpacesModel(RevitDocument revitDocument)
        {
            RevitDocument = revitDocument;
        }

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
