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
            DocumentTransaction(
                document,
                elementsList,
                DeleteRevitElements,
                transactionName
                );
        }

        internal static void CreateSpacesByLinkDocumentRooms(Document document, List<RevitElement> elements, string transactionName)
        {
            throw new NotImplementedException();
        }

        internal static void CreateSpacesByLinkDocumentPhaseRooms(Document document, List<RevitElement> elements, string transactionName)
        {
            throw new NotImplementedException();
        }

        internal static void CreateRoomsByLinkDocumentRooms(Document document, List<RevitElement> elements, string transactionName)
        {
            throw new NotImplementedException();
        }

        internal static void CreateRoomsByLinkDocumentPhaseRooms(Document document, List<RevitElement> elements, string transactionName)
        {
            throw new NotImplementedException();
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
