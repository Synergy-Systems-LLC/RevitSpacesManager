using System.Collections.Generic;

namespace RevitSpacesManager.Models
{
    internal interface ICreating
    {
        bool IsWorksetNotAvailable();
        RoomsVerificationReport VerifyRooms(IRoomLevelsMatchable levelMatchable);
        void CreateAllByRooms(List<RoomElement> roomElements); 
        void CreateByPhaseRooms(List<RoomElement> roomElements, PhaseElement phaseElement);
    }
}
