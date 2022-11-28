using System;
using System.Collections.Generic;
using Autodesk.Revit.DB;

namespace RevitSpacesManager.Models.Services
{
    internal static class RevitServices
    {
        internal static void DeleteElements(
            Document document, 
            List<RevitElement> elementsList, 
            string transactionName
            )
        {
            DocumentTransaction(document, elementsList, DeleteRevitElements, transactionName);
        }

        internal static void CreateSpacesByRooms(
            Document document, 
            List<RevitElement> elementsList, 
            string transactionName
            )
        {
            DocumentTransaction(document, elementsList, CreateSpacesByRevitElements, transactionName);
        }

        internal static void CreateRoomsByRooms(
            Document document, 
            List<RevitElement> elementsList, 
            string transactionName
            )
        {
            DocumentTransaction(document, elementsList, CreateRoomsByRevitElements, transactionName);
        }


        private static void CreateSpacesByRevitElements(Document document, List<RevitElement> elementsList)
        {
            foreach (RevitElement element in elementsList)
            {
                RoomElement room = element as RoomElement;

                //element = document.Create.NewSpace(level, UV(roomLocationPoint.X, roomLocationPoint.Y));
                //element.get_Parameter(BuiltInParameter.ELEM_PARTITION_PARAM).Set(worksetSpacesId);
                //document.Regenerate()
            }
        }

        private static void CreateRoomsByRevitElements(Document document, List<RevitElement> elementsList)
        {
            foreach (RevitElement element in elementsList)
            {
                RoomElement room = element as RoomElement;

                //element = document.Create.NewRoom(level, UV(roomLocationPoint.X, roomLocationPoint.Y));
                //element.get_Parameter(BuiltInParameter.ELEM_PARTITION_PARAM).Set(self.worksetRoomsId);
                //document.Regenerate()
            }
        }

        private static void DeleteRevitElements(Document document, List<RevitElement> elementsList)
        {
            foreach (RevitElement element in elementsList)
            {
                document.Delete(element.ElementId);
            }
        }

        private static void DocumentTransaction(
            Document document, 
            List<RevitElement> elementsList, 
            Action<Document, List<RevitElement>> action, 
            string transactionName = "RevitSpaceManager"
            )
        {
            using (Transaction transaction = new Transaction(document, transactionName))
            {
                transaction.Start();
                action(document, elementsList);
                transaction.Commit();
            }
        }
    }
}
