using Autodesk.Revit.DB;
using RevitSpacesManager.Revit;
using RevitSpacesManager.Revit.Services;
using System;
using System.Collections.Generic;

namespace RevitSpacesManager.Models
{
    internal class MainModel
    {
        internal readonly RevitDocument CurrentRevitDocument;
        internal readonly List<RevitDocument> LinkedRevitDocuments;

        private readonly Document _currentDocument;
        private readonly View _activeView;
        private readonly string _activeViewPhaseName;

        internal MainModel()
        {
            _currentDocument = RevitManager.Document;
            _activeView = _currentDocument.ActiveView;
            _activeViewPhaseName = _activeView.get_Parameter(BuiltInParameter.VIEW_PHASE).AsValueString();
            CurrentRevitDocument = new RevitDocument(_currentDocument);
            LinkedRevitDocuments = CurrentRevitDocument.GetRevitLinkDocuments();
        }

        internal void DeleteAllSpaces()
        {
            throw new NotImplementedException();
        }

        internal void DeleteAllRooms()
        {
            throw new NotImplementedException();
        }

        internal void DeleteSelectedSpaces(PhaseElement currentDocumentPhaseSelected)
        {
            throw new NotImplementedException();
        }

        internal void DeleteSelectedRooms(PhaseElement currentDocumentPhaseSelected)
        {
            throw new NotImplementedException();
        }

        internal void CreateAllSpacesByLinkRooms(RevitDocument linkedDocumentSelected)
        {
            throw new NotImplementedException();
        }

        internal void CreateAllRoomsByLinkRooms(RevitDocument linkedDocumentSelected)
        {
            throw new NotImplementedException();
        }

        internal void CreateSelectedSpacesByLinkRooms(PhaseElement linkedDocumentPhaseSelected)
        {
            throw new NotImplementedException();
        }

        internal void CreateSelectedRoomsByLinkRooms(PhaseElement linkedDocumentPhaseSelected)
        {
            throw new NotImplementedException();
        }
    }
}
