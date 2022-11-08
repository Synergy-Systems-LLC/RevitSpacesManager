using Autodesk.Revit.DB;
using RevitSpacesManager.Revit;
using RevitSpacesManager.Revit.Services;
using System.Collections.Generic;

namespace RevitSpacesManager.Models
{
    internal class MainModel
    {
        internal readonly RevitDocument CurrentRevitDocument;
        internal readonly List<RevitDocument> LinkedRevitDocuments;

        private readonly Document _currentDocument;

        internal MainModel()
        {
            _currentDocument = RevitManager.Document;
            CurrentRevitDocument = new RevitDocument(_currentDocument);
            LinkedRevitDocuments = CurrentRevitDocument.LinkDocuments;
        }

        internal void DeleteAllSpaces()
        {
            foreach(SpaceElement spaceElement in CurrentRevitDocument.Spaces)
            {
                //_currentDocument.Delete(spaceElement.Space.Id);
            }
            
        }

        internal void DeleteAllRooms()
        {

            
        }

        internal void DeleteSelectedSpaces(PhaseElement currentDocumentPhaseSelected)
        {
            
        }

        internal void DeleteSelectedRooms(PhaseElement currentDocumentPhaseSelected)
        {
            
        }

        internal void CreateAllSpacesByLinkRooms(RevitDocument linkedDocumentSelected)
        {
            
        }

        internal void CreateAllRoomsByLinkRooms(RevitDocument linkedDocumentSelected)
        {
            
        }

        internal void CreateSelectedSpacesByLinkRooms(PhaseElement linkedDocumentPhaseSelected)
        {
            
        }

        internal void CreateSelectedRoomsByLinkRooms(PhaseElement linkedDocumentPhaseSelected)
        {
            
        }
    }
}
