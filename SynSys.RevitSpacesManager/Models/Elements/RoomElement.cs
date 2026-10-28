using Autodesk.Revit.DB;
using Autodesk.Revit.DB.Architecture;
using System.Collections.Generic;
using System.Linq;

namespace RevitSpacesManager.Models
{
    internal class RoomElement : RevitElement
    {
        internal override ElementId ElementId => _room.Id;
        internal int Id => ElementId.IntegerValue;
        internal string PhaseName => PhaseParameter.AsValueString();
        internal int PhaseId => PhaseParameter.AsElementId().IntegerValue;

        internal double Area => _room.Area;
        internal LevelElement? Level => _level;
        internal LevelElement? UpperLimit => _upperLimit;
        internal XYZ LocationPoint => GetLocationPoint();
        internal double BaseOffset => _room.BaseOffset;
        internal double LimitOffset => _room.LimitOffset;
        internal string Number => _room.Number;
        internal string Name => _room.get_Parameter(BuiltInParameter.ROOM_NAME).AsString();


        private readonly Room _room;
        private readonly LevelElement? _level;
        private readonly LevelElement? _upperLimit;
        private readonly Autodesk.Revit.DB.Transform _transform;
        private Parameter PhaseParameter => _room.get_Parameter(BuiltInParameter.ROOM_PHASE);


        internal RoomElement(Room room, List<LevelElement> documentLevels, Autodesk.Revit.DB.Transform transform)
        {
            _room = room;
            _transform = transform;
            _level = GetLevel(documentLevels);
            _upperLimit = GetUpperLimit(documentLevels);
        }


        private XYZ GetLocationPoint()
        {
            LocationPoint location = (LocationPoint)_room.Location;
            XYZ point = location.Point;
            return _transform.OfPoint(point);
        }

        private LevelElement? GetLevel(List<LevelElement> documentLevels)
        {
            Level level = _room.Level;
            if (level == null) return null;
            return documentLevels.FirstOrDefault(l => l.Id == _room.Level.Id.IntegerValue);
        }

        private LevelElement? GetUpperLimit(List<LevelElement> documentLevels)
        {
            Level upperLimit = _room.UpperLimit;
            if (upperLimit == null) return _level;
            return documentLevels.FirstOrDefault(l => l.Id == upperLimit.Id.IntegerValue);
        }
    }
}
