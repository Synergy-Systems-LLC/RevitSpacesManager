using Autodesk.Revit.DB;
using Autodesk.Revit.DB.Mechanical;

namespace RevitSpacesManager.Models
{
    internal class SpaceElement
    {
        internal Space Space { get; set; }
        internal int Id { get; set; }
        internal string Name { get; set; }
        internal string PhaseName { get; set; }


        internal SpaceElement(Space space)
        {
            Space = space;
            Id = space.Id.IntegerValue;
            Name = space.Name;
            PhaseName = space.get_Parameter(BuiltInParameter.ROOM_PHASE).AsValueString();
        }
    }
}
