﻿using Autodesk.Revit.DB;
using System.Collections.Generic;

namespace RevitSpacesManager.Models
{
    internal class PhaseElement
    {
        public string SpacesItemName => $"{NumberOfSpaces} Space{PluralSuffix(NumberOfSpaces)} - {Name}"; 
        public string RoomsItemName => $"{NumberOfRooms} Room{PluralSuffix(NumberOfRooms)} - {Name}";

        internal string Name { get; set; }
        internal List<SpaceElement> Spaces { get; set; } = new List<SpaceElement>();
        internal List<RoomElement> Rooms { get; set; } = new List<RoomElement>();
        internal int NumberOfSpaces => Spaces.Count;
        internal int NumberOfRooms => Rooms.Count;

        internal PhaseElement(Phase phase)
        {
            Name = phase.Name;
        }

        private string PluralSuffix(int number)
        {
            if (number == 1)
                return " ";
            return "s";
        }
    }
}
