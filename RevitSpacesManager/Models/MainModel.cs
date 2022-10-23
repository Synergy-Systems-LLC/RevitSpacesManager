using Autodesk.Revit.DB;
using RevitSpacesManager.Revit;
using RevitSpacesManager.Revit.Services;
using System.Collections.Generic;
using System.Windows;

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

            ShowReportMessage();
        }

        private void ShowReportMessage()
        {
            string report = "Current Model:\n" +
                            $"Levels : {CurrentRevitDocument.Levels.Count}\n" +
                            $"Links : {LinkedRevitDocuments.Count}\n" +
                            $"Spaces : {CurrentRevitDocument.Spaces.Count}\n" +
                            $"Rooms : {CurrentRevitDocument.Rooms.Count}\n\n";
            foreach (RevitDocument linkRevitDocument in LinkedRevitDocuments)
            {
                report = report +
                         $"{linkRevitDocument.Title}\n" +
                         $"Levels : {linkRevitDocument.Levels.Count}\n" +
                         $"Spaces : {linkRevitDocument.Spaces.Count}\n" +
                         $"Rooms : {linkRevitDocument.Rooms.Count}\n\n";
            }

            MessageBox.Show(report, "REPORT");
        }
    }
}
