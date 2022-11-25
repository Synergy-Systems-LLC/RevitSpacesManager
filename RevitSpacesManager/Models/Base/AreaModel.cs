using System.Collections.Generic;

namespace RevitSpacesManager.Models
{
    internal abstract class AreaModel : ICreating, IDeleting
    {
        public abstract void DeleteAll();
        public abstract void DeleteByPhase(PhaseElement phaseElement);
        public abstract void CreateAllByLinkedDocument(RevitDocument linkDocument);
        public abstract void CreateByLinkedDocumentPhase(PhaseElement phaseElement);

        internal abstract List<PhaseElement> GetPhases();
        internal abstract int NumberOfElements { get; }
    }
}
