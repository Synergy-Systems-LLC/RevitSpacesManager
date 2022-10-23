using Autodesk.Revit.DB;
using System.Collections.Generic;

namespace RevitSpacesManager.Models
{
    internal class PhaseElement
    {
        public string Name { get; set; }
        public int NumberOfSpaces { get => Spaces.Count; }
        public int NumberOfRooms { get => Rooms.Count; }
        public string SpacesItemName 
        { 
            get
            {
                if(NumberOfSpaces == 1)
                    return $"{Spaces.Count} Space  - {Name}";
                return $"{Spaces.Count} Spaces - {Name}";
            }
        }
        public string RoomsItemName 
        { 
            get
            {
                if (NumberOfRooms == 1)
                    return $"{Rooms.Count} Room  - {Name}";
                return $"{Rooms.Count} Rooms - {Name}";
            }
        }

        internal Phase Phase { get; set; }
        internal int Id { get; set; }
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
