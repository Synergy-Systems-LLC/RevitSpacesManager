using System.Windows;
using System.Collections.Generic;

namespace RevitSpacesManager.Models
{
    internal class RoomsVerificationReport
    {
        internal List<RoomElement> VerifiedRooms { get; } = new List<RoomElement>();
        internal List<RoomElement> IncorrectyPlacedRooms { get; } = new List<RoomElement>();
        internal List<RoomElement> IncorrectLevelRooms { get; } = new List<RoomElement>();


        internal RoomsVerificationReport(RevitDocument revitDocument, IRoomLevelsMatchable levelMatchable)
        {
            List<RoomElement> roomElements = levelMatchable.Rooms;
            revitDocument.MatchLevels(roomElements);

            foreach (RoomElement roomElement in roomElements)
            {
                if (IsRoomNotPlacedCorrectly(roomElement))
                {
                    IncorrectyPlacedRooms.Add(roomElement);
                }
                else if (IsRoomLevelNotAvailableInRevitDocument(revitDocument, roomElement))
                {
                    IncorrectLevelRooms.Add(roomElement);
                }
                else
                {
                    VerifiedRooms.Add(roomElement);
                }
            }
        }


        private bool IsRoomNotPlacedCorrectly(RoomElement roomElement)
        {
            double area = roomElement.Area;
            if (area > 0) return false;

            var level = roomElement.Level;
            if (level is null) return false;

            return true;
        }

        private bool IsRoomLevelNotAvailableInRevitDocument(RevitDocument revitDocument, RoomElement roomElement)
        {
            var debug = false;
            LevelElement? roomLevel = roomElement.Level;
            var fullname = $"{roomElement.Number} {roomElement.Name}";
            if (roomLevel is null) {
                if(debug) MessageBox.Show($"{fullname}\n\nroomLevel is null\n\n{roomLevel?.Name}", "NULL!");
                return true;
            }
            if (revitDocument.IsLevelNotAvailable(roomLevel)) {
                if (debug) MessageBox.Show($"{fullname}\n\nroomLevel IsLevelNotAvailable true\n\n{roomLevel?.Name}", "NULL!");
                return true; 
            }
            LevelElement? roomUpperLimit = roomElement.UpperLimit;
            if (roomUpperLimit is null){
                if (debug) MessageBox.Show($"{fullname}\n\nroomUpperLimit is null\n\n{roomUpperLimit?.Name}", "NULL!");
                return true; 
            }
            if (revitDocument.IsLevelNotAvailable(roomUpperLimit)) {
                if (debug) MessageBox.Show($"{fullname}\n\nroomUpperLimit IsLevelNotAvailable true\n\n{roomUpperLimit?.Name}", "NULL!");
                return true; 
            }
            return false;
        }
    }
}
