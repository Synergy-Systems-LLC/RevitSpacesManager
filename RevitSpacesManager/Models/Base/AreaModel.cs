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
        public abstract void CreateByRooms(List<RoomElement> roomElements);

        public RoomsVerificationReport VerifyRooms(IRoomLevelsMatchable levelMatchable)
        {
            return new RoomsVerificationReport(RevitDocument, levelMatchable);
        }

        internal abstract RevitDocument RevitDocument { get; }
        internal abstract int NumberOfElements { get; }
        internal abstract List<PhaseElement> GetPhases();
        internal abstract string GetAreaName();
    }
}
