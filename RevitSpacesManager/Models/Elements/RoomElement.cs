using Autodesk.Revit.DB;
using Autodesk.Revit.DB.Architecture;

namespace RevitSpacesManager.Models
{
    internal class RoomElement : RevitElement
    {
        internal override ElementId ElementId => _room.Id;
        internal int Id => ElementId.IntegerValue;
        internal string Name => _room.Name;    
        internal string PhaseName => PhaseParameter.AsValueString();
        internal int PhaseId => PhaseParameter.AsElementId().IntegerValue;

        private readonly Room _room;
        private Parameter PhaseParameter => _room.get_Parameter(BuiltInParameter.ROOM_PHASE);

        internal RoomElement(Room room)
        {
            _room = room;
        }
    }
}
