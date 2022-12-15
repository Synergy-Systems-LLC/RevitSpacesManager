using System.Collections.Generic;
using System.Linq;

namespace RevitSpacesManager.Models
{
    internal abstract class AreaModel : ICreating, IDeleting
    {
        public abstract void DeleteAll();
        public abstract void DeleteByPhase(PhaseElement phaseElement);
        public abstract bool AreAllNotEditable();
        public abstract bool ArePhaseElementsNotEditable(PhaseElement phaseElement);
        public abstract void CreateAllByLinkedDocument(RevitDocument linkDocument);
        public abstract void CreateByLinkedDocumentPhase(PhaseElement phaseElement);
        public abstract bool IsWorksetNotAvailable();
        public RoomsVerificationReport VerifyLinkRooms(RevitDocument linkDocument)
        {
            // TODO Check room correct
            // TODO - area !=0 (placed and enclosed)
            // TODO - level available
            // TODO - level same elevation
            // TODO - level upper limit available
            // TODO - level upper limit same elevation
            // TODO Report with IDs

            List<RevitElement> elements = linkDocument.Rooms.Cast<RevitElement>().ToList();
            RoomsVerificationReport report = new RoomsVerificationReport();

            return report;
        }

        public RoomsVerificationReport VerifyPhaseRooms(PhaseElement phaseElement)
        {
            List<RevitElement> elements = phaseElement.Rooms.Cast<RevitElement>().ToList();
            RoomsVerificationReport report = new RoomsVerificationReport();

            return report;
        }

        internal abstract int NumberOfElements { get; }
        internal abstract List<PhaseElement> GetPhases();
    }
}
