using System.Collections.Generic;

namespace RevitSpacesManager.Models
{
    internal interface ICreating
    {
        bool IsWorksetNotAvailable();
        RoomsVerificationReport VerifyRooms(IRoomLevelsMatchable levelMatchable);
        void CreateByRooms(List<RoomElement> roomElements); 
    }
}
