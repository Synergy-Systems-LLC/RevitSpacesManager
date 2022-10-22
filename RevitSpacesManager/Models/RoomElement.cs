using Autodesk.Revit.DB;
using Autodesk.Revit.DB.Architecture;

namespace RevitSpacesManager.Models
{
    internal class RoomElement
    {
        internal Room Room { get; set; }
        internal int Id { get; set; }
        internal string Name { get; set; }
        internal string PhaseName { get; set; }


        internal RoomElement(Room room)
        {
            Room = room;
            Id = room.Id.IntegerValue;
            Name = room.Name;
            PhaseName = room.get_Parameter(BuiltInParameter.ROOM_PHASE).AsValueString();
        }
    }
}
