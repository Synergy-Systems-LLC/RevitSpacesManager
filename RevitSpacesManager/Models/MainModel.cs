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

        internal string DeleteAllSpaces()
        {
            foreach(SpaceElement space in CurrentRevitDocument.Spaces)
            {

            }
            return "report";
        }

        internal string DeleteAllRooms()
        {

            return "report";
        }

        internal string DeleteSelectedSpaces(PhaseElement currentDocumentPhaseSelected)
        {
            return "report";
        }

        internal string DeleteSelectedRooms(PhaseElement currentDocumentPhaseSelected)
        {
            return "report";
        }

        internal string CreateAllSpacesByLinkRooms(RevitDocument linkedDocumentSelected)
        {
            return "report";
        }

        internal string CreateAllRoomsByLinkRooms(RevitDocument linkedDocumentSelected)
        {
            return "report";
        }

        internal string CreateSelectedSpacesByLinkRooms(PhaseElement linkedDocumentPhaseSelected)
        {
            return "report";
        }

        internal string CreateSelectedRoomsByLinkRooms(PhaseElement linkedDocumentPhaseSelected)
        {
            return "report";
        }

        internal string CreateAllSpaces(RevitDocument linkedDocumentSelected)
        {
            return "report";
        }

        internal string CreateAllRooms(RevitDocument linkedDocumentSelected)
        {
            return "report";
        }

        internal string CreateSelectedSpaces(PhaseElement linkedDocumentPhaseSelected)
        {
            return "report";
        }

        internal string CreateSelectedRooms(PhaseElement linkedDocumentPhaseSelected)
        {
            return "report";
        }
    }
}
