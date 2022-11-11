using Autodesk.Revit.DB;
using Autodesk.Revit.DB.Mechanical;

namespace RevitSpacesManager.Models
{
    internal class SpaceElement
    {
        internal int Id => _space.Id.IntegerValue;
        internal string Name => _space.Name;
        internal string PhaseName => PhaseParameter.AsValueString();
        internal int PhaseId => PhaseParameter.AsElementId().IntegerValue;


        private readonly Space _space;
        private Parameter PhaseParameter => _space.get_Parameter(BuiltInParameter.ROOM_PHASE);

        internal SpaceElement(Space space)
        {
            _space = space;
        }
    }
}
