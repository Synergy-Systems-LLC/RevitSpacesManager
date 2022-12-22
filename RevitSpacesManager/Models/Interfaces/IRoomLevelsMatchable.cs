using System.Collections.Generic;

namespace RevitSpacesManager.Models
{
    internal interface IRoomLevelsMatchable
    {
        List<RoomElement> Rooms { get; }
    }
}
