using Autodesk.Revit.DB;
using System.Collections.Generic;

namespace RevitSpacesManager.Models
{
    internal class PhaseElement
    {
        public string Name { get; set; }
        public int NumberOfSpaces { get => Spaces.Count; }
        public int NumberOfRooms { get => Rooms.Count; }
        public string SpacesItemName { get => $"{NumberOfSpaces} Room{IsPlural(NumberOfSpaces)} - {Name}"; } 
        public string RoomsItemName { get => $"{NumberOfRooms} Room{IsPlural(NumberOfRooms)} - {Name}"; } 

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

        private string IsPlural(int number)
        {
            if (number == 1)
                return " ";
            return "s";
        }
    }
}
