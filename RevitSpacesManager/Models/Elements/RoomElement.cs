using Autodesk.Revit.DB;
using Autodesk.Revit.DB.Architecture;

namespace RevitSpacesManager.Models
{
    internal class RoomElement : RevitElement
    {
        internal override ElementId ElementId => _room.Id;
        internal int Id => ElementId.IntegerValue;
        internal string PhaseName => PhaseParameter.AsValueString();
        internal int PhaseId => PhaseParameter.AsElementId().IntegerValue;

        internal double Area => _room.Area;
        internal XYZ LocationPoint => GetLocationPoint();
        internal string LevelName => _room.Level.Name;
        internal double BaseOffset => _room.BaseOffset;
        internal double LimitOffset => _room.LimitOffset;
        internal string Number => _room.Number;
        internal string Name => _room.get_Parameter(BuiltInParameter.ROOM_NAME).AsString();
        internal string UpperLimitName => _room.UpperLimit.Name;


        private readonly Room _room;
        private Parameter PhaseParameter => _room.get_Parameter(BuiltInParameter.ROOM_PHASE);


        internal RoomElement(Room room)
        {
            _room = room;
        }


        private XYZ GetLocationPoint()
        {
            LocationPoint location = _room.Location as LocationPoint;
            XYZ point = location.Point;
            return point;
        }
    }
}
