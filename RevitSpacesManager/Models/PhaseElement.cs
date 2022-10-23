using Autodesk.Revit.DB;
using System.Collections.Generic;

namespace RevitSpacesManager.Models
{
    internal class PhaseElement
    {
        internal Phase Phase { get; set; }
        internal int Id { get; set; }
        internal string Name { get; set; }
        internal List<SpaceElement> Spaces { get; set; }
        internal List<RoomElement> Rooms { get; set; }

        internal PhaseElement(Phase phase)
        {
            Phase = phase;
            Id = Phase.Id.IntegerValue;
            Name = Phase.Name;
        }

        internal void SyncSpaces(List<SpaceElement> spaces)
        {
            Spaces = spaces;
        }

        internal void SyncRooms(List<RoomElement> rooms)
        {
            Rooms = rooms;
        }
    }
}
