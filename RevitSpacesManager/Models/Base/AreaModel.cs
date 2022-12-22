using System.Collections.Generic;
using System.Linq;

namespace RevitSpacesManager.Models
{
    internal abstract class AreaModel : ICreating, IDeleting
    {
        public abstract bool AreNotAllElementsEditable();
        public abstract bool AreNotAllPhaseElementsEditable(PhaseElement phaseElement);
        public abstract void DeleteAll();
        public abstract void DeleteByPhase(PhaseElement phaseElement);
        public abstract bool IsWorksetNotAvailable();
        public abstract void CreateAllByLinkedDocument(RevitDocument linkDocument);
        public abstract void CreateByLinkedDocumentPhase(PhaseElement phaseElement);

        public void MatchLevels(IRoomLevelsMatchable levelMatchable)
        {
            RevitDocument.MatchLevels(levelMatchable);
        }

        public RoomsVerificationReport VerifyLinkRooms(RevitDocument linkDocument)
        {
            List<RoomElement> roomElements = linkDocument.Rooms.ToList();
            return new RoomsVerificationReport(RevitDocument, roomElements);
        }
        public RoomsVerificationReport VerifyPhaseRooms(PhaseElement phaseElement)
        {
            List<RoomElement> roomElements = phaseElement.Rooms.ToList();
            return new RoomsVerificationReport(RevitDocument, roomElements);
        }

        internal abstract RevitDocument RevitDocument { get; }
        internal abstract int NumberOfElements { get; }
        internal abstract List<PhaseElement> GetPhases();
    }
}
