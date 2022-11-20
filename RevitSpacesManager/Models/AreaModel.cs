using System.Collections.Generic;

namespace RevitSpacesManager.Models
{
    internal abstract class AreaModel : ICreating, IDeleting
    {
        public abstract void DeleteAll();
        public abstract void DeleteSelected();
        public abstract void CreateAll();
        public abstract void CreateSelected();

        internal abstract List<PhaseElement> GetPhases();
        internal abstract int NumberOfElements { get; }
    }
}
