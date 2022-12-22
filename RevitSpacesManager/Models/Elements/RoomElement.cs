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
        internal LevelElement Level => _level;
        internal LevelElement UpperLimit => _upperLimit;
        internal XYZ LocationPoint => GetLocationPoint();
        internal double BaseOffset => _room.BaseOffset;
        internal double LimitOffset => _room.LimitOffset;
        internal string Number => _room.Number;
        internal string Name => _room.get_Parameter(BuiltInParameter.ROOM_NAME).AsString();


        private readonly Room _room;
        private readonly LevelElement _level;
        private readonly LevelElement _upperLimit;
        private Parameter PhaseParameter => _room.get_Parameter(BuiltInParameter.ROOM_PHASE);


        internal RoomElement(Room room)
        {
            _room = room;
            _level = new LevelElement(room.Level);
            _upperLimit = GetUpperLimit();
        }


        private XYZ GetLocationPoint()
        {
            LocationPoint location = _room.Location as LocationPoint;
            XYZ point = location.Point;
            return point;
        }

        private LevelElement GetUpperLimit()
        {
            Level upperLimit = _room.UpperLimit;
            if (upperLimit == null)
                return _level;
            return new LevelElement(upperLimit);
        }
    }
}
