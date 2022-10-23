using Autodesk.Revit.DB;
using System.Collections.Generic;

namespace RevitSpacesManager.Models
{
    internal class PhaseElement
    {
        internal Phase Phase { get; set; }
        internal int Id { get; set; }
        internal string Name { get; set; }
        internal List<SpaceElement> Spaces;
        internal List<RoomElement> Rooms;

        internal PhaseElement(Phase phase)
        {
            Phase = phase;
            Id = Phase.Id.IntegerValue;
            Name = Phase.Name;
        }
    }
}
