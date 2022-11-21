using System.Collections.Generic;

namespace RevitSpacesManager.Models
{
    internal abstract class AreaModel : ICreating, IDeleting
    {
        public abstract void DeleteAll(string activeObject);
        public abstract void DeleteSelected(string activeObject, PhaseElement phaseElement);
        public abstract void CreateAll();
        public abstract void CreateSelected();

        internal abstract List<PhaseElement> GetPhases();
        internal abstract int NumberOfElements { get; }
    }
}
