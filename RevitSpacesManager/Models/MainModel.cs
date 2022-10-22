using Autodesk.Revit.DB;
using RevitSpacesManager.Revit;
using RevitSpacesManager.Revit.Services;
using System.Collections.Generic;
using System.Windows;

namespace RevitSpacesManager.Models
{
    internal class MainModel
    {
        private readonly Document _currentDocument;
        private readonly View _activeView;
        private readonly string _activeViewPhaseName;

        private readonly RevitDocument _currentRevitDocument;
        private readonly List<RevitDocument> _linkRevitDocuments;

        internal MainModel()
        {
            //TODO
            //+ get active view phase and check
            //+ launch window
            //+ get levels
            //+ get link documents
            //+ get spaces by phase
            //+ get rooms by phase
            //- get rooms by link and phase
            //- get spaces workset id
            //- get rooms workset id

            _currentDocument = RevitManager.Document;
            _activeView = _currentDocument.ActiveView;
            _activeViewPhaseName = _activeView.get_Parameter(BuiltInParameter.VIEW_PHASE).AsValueString();
            _currentRevitDocument = new RevitDocument(_currentDocument);
            _linkRevitDocuments = _currentRevitDocument.GetRevitLinkDocuments();

            //string report = $"Levels : {_currentDocumentLevelElements.Count}\nLinks : {_revitLinkElements.Count}\nSpaces : {_currentDocumentSpaceElements.Count}\nRooms : {_currentDocumentRoomElements.Count}";
            //MessageBox.Show(report, "REPORT");
        }
    }
}
