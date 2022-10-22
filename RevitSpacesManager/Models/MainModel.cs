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

            ShowReportMessage();
        }

        private void ShowReportMessage()
        {
            string report = "Current Model:\n" +
                            $"Levels : {_currentRevitDocument.Levels.Count}\n" +
                            $"Links : {_linkRevitDocuments.Count}\n" +
                            $"Spaces : {_currentRevitDocument.Spaces.Count}\n" +
                            $"Rooms : {_currentRevitDocument.Rooms.Count}\n\n";
            foreach (RevitDocument linkRevitDocument in _linkRevitDocuments)
            {
                report = report +
                         linkRevitDocument.Title +
                         $"Levels : {linkRevitDocument.Levels.Count}\n" +
                         $"Spaces : {linkRevitDocument.Spaces.Count}\n" +
                         $"Rooms : {linkRevitDocument.Rooms.Count}\n\n";
            }

            MessageBox.Show(report, "REPORT");
        }
    }
}
