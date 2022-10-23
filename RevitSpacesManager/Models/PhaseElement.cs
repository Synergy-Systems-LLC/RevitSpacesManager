using Autodesk.Revit.DB;
using System.Collections.Generic;

namespace RevitSpacesManager.Models
{
    internal class PhaseElement
    {
        internal Phase Phase { get; set; }
        public int Id { get; set; }
        public string Name { get; set; }
        internal List<SpaceElement> Spaces { get; set; } = new List<SpaceElement>();
        internal List<RoomElement> Rooms { get; set; } = new List<RoomElement>();

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
