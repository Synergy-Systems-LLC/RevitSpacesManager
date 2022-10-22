using Autodesk.Revit.DB;
using RevitSpacesManager.Revit;
using RevitSpacesManager.Revit.Services;
using System.Collections.Generic;
using System.Windows;

namespace RevitSpacesManager.Models
{
    internal class MainModel
    {
        private Document _currentDocument;
        private View _activeView;
        private string _activeViewPhaseName;

        private RevitDocument _currentRevitDocument;

        private List<LevelElement> _currentDocumentLevelElements;
        private List<RevitLinkElement> _revitLinkElements;
        private List<SpaceElement> _currentDocumentSpaceElements;
        private List<RoomElement> _currentDocumentRoomElements;

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

            DefineInitialRevitModelData();
            DefineRevitLinkElements();

            //string report = $"Levels : {_currentDocumentLevelElements.Count}\nLinks : {_revitLinkElements.Count}\nSpaces : {_currentDocumentSpaceElements.Count}\nRooms : {_currentDocumentRoomElements.Count}";
            //MessageBox.Show(report, "REPORT");
        }

        private void DefineInitialRevitModelData()
        {
            _currentDocument = RevitManager.Document;
            _activeView = _currentDocument.ActiveView;
            _activeViewPhaseName = _activeView.get_Parameter(BuiltInParameter.VIEW_PHASE).AsValueString();
            _currentRevitDocument = new RevitDocument(_currentDocument);
        }

        private void DefineRevitLinkElements()
        {
            _revitLinkElements = new List<RevitLinkElement>();
            List<RevitLinkInstance> revitLinkInstances = _currentModelServices.GetRevitLinkInstances();
            foreach (RevitLinkInstance revitLinkInstance in revitLinkInstances)
            {
                RevitLinkElement revitLinkElement = new RevitLinkElement(revitLinkInstance);
                _revitLinkElements.Add(revitLinkElement);
            }
        }
    }
}
