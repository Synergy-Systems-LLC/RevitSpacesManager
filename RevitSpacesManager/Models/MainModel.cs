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
    }
}
