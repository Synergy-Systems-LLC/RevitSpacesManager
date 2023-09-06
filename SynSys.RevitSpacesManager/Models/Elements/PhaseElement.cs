using Autodesk.Revit.DB;
using System.Collections.Generic;

namespace RevitSpacesManager.Models
{
    internal class PhaseElement : IRoomLevelsMatchable
    {
        public string SpacesItemName => $"{NumberOfSpaces} Space{PluralSuffix(NumberOfSpaces)} - {Name}"; 
        public string RoomsItemName => $"{NumberOfRooms} Room{PluralSuffix(NumberOfRooms)} - {Name}";

        internal string Name => _phase.Name;
        internal int Id => _phase.Id.IntegerValue;
        internal List<SpaceElement> Spaces { get; set; } = new List<SpaceElement>();
        public List<RoomElement> Rooms { get; set; } = new List<RoomElement>();
        internal int NumberOfSpaces => Spaces.Count;
        internal int NumberOfRooms => Rooms.Count;

        private readonly Phase _phase;

        internal PhaseElement(Phase phase)
        {
            _phase = phase;
        }

        private string PluralSuffix(int number)
        {
            if (number == 1)
                return " ";
            return "s";
        }
    }
}
