using System.Collections.Generic;

namespace RevitSpacesManager.Models
{
    internal abstract class AreaModel : ICreating, IDeleting
    {
        public abstract void DeleteAll();
        public abstract void DeleteByPhase(PhaseElement phaseElement);
        public abstract void CreateAllByLinkedDocument(RevitDocument linkDocument);
        public abstract void CreateByLinkedDocumentPhase(PhaseElement phaseElement);
        public abstract bool IsWorksetNotAvailable();
        public abstract RoomsCreationVerificationReport VerifyDocumentRoomsForCreation();
        public abstract bool AreAllNotEditable();
        public abstract bool ArePhaseElementsNotEditable(PhaseElement phaseElement);

        internal abstract int NumberOfElements { get; }
        internal abstract List<PhaseElement> GetPhases();
    }
}
