﻿using Autodesk.Revit.DB;
using Autodesk.Revit.DB.Architecture;
using Autodesk.Revit.DB.Mechanical;
using RevitSpacesManager.Models;
using System.Collections.Generic;

namespace RevitSpacesManager.Revit.Services
{
    internal class RevitDocument
    {
        internal List<Workset> UserWorksets;
        internal List<LevelElement> Levels;
        internal List<SpaceElement> Spaces;
        internal List<RoomElement> Rooms;

        private readonly Document _document;

        internal RevitDocument(Document doc)
        {
            _document = doc;
            GetUserWorksets();
            GetLevels();
            GetSpaces();
            GetRooms();
        }

        private void GetUserWorksets()
        {
            UserWorksets = new List<Workset>();
            FilteredWorksetCollector worksetCollector = new FilteredWorksetCollector(_document);
            FilteredWorksetCollector userWorksetCollector = worksetCollector.OfKind(WorksetKind.UserWorkset);
            foreach(Workset workset in userWorksetCollector)
            {
                UserWorksets.Add(workset);
            }
        }

        internal bool DoesUserWorksetExist(string worksetName)
        {
            foreach (Workset workset in UserWorksets)
            {
                if(workset.Name == worksetName)
                {
                    return true;
                }
            }
            return false;
        }

        internal int GetUserWorksetIntegerIdByName(string worksetName)
        {
            foreach (Workset workset in UserWorksets)
            {
                if (workset.Name == worksetName)
                {
                    return workset.Id.IntegerValue;
                }
            }
            return 0;
        }

        private void GetLevels()
        {
            FilteredElementCollector elementCollector = new FilteredElementCollector(_document);
            IList<Element> elements = elementCollector.OfClass(typeof(Level)).WhereElementIsNotElementType().ToElements();
            Levels = new List<LevelElement>();
            foreach (Element element in elements)
            {
                Level level = element as Level;
                LevelElement levelElement = new LevelElement(level);
                Levels.Add(levelElement);
            }
        }

        private void GetSpaces()
        {
            FilteredElementCollector elementCollector = new FilteredElementCollector(_document);
            IList<Element> elements = elementCollector.OfCategory(BuiltInCategory.OST_MEPSpaces).WhereElementIsNotElementType().ToElements();
            Spaces = new List<SpaceElement>();
            foreach (Element element in elements)
            {
                Space space = element as Space;
                SpaceElement spaceElement = new SpaceElement(space);
                Spaces.Add(spaceElement);
            }
        }

        private void GetRooms()
        {
            FilteredElementCollector elementCollector = new FilteredElementCollector(_document);
            IList<Element> elements = elementCollector.OfCategory(BuiltInCategory.OST_Rooms).WhereElementIsNotElementType().ToElements();
            Rooms = new List<RoomElement>();
            foreach (Element element in elements)
            {
                Room room = element as Room;
                RoomElement roomElement = new RoomElement(room);
                Rooms.Add(roomElement);
            }
        }

        internal List<RevitLinkInstance> GetRevitLinkInstances()
        {
            FilteredElementCollector elementCollector = new FilteredElementCollector(_document);
            IList<Element> elements = elementCollector.OfClass(typeof(RevitLinkInstance)).WhereElementIsNotElementType().ToElements();
            List<RevitLinkInstance> revitLinkInstances = new List<RevitLinkInstance>();
            foreach (Element element in elements)
            {
                RevitLinkInstance revitLinkInstance = element as RevitLinkInstance;
                revitLinkInstances.Add(revitLinkInstance);
            }
            return revitLinkInstances;
        }
    }
}
