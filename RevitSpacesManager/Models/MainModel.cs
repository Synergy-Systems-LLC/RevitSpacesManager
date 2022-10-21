using Autodesk.Revit.DB;
using RevitSpacesManager.Revit;
using RevitSpacesManager.Revit.Services;
using System.Collections.Generic;

namespace RevitSpacesManager.Models
{
    internal class MainModel
    {
        private Document _currentDocument;
        private View _activeView;
        private string _activeViewPhaseName;
        private RevitDocumentServices _currentModelServices;
        private List<LevelElement> _currentDocumentLevels;

        internal MainModel()
        {
            //TODO
            //+ get active view phase and check
            //+ launch window
            //- get levels
            //- get link documents
            //- get spaces by phase
            //- get rooms by phase
            //- get rooms by link and phase
            //- get spaces workset id
            //- get rooms workset id

            DefineInitialRevitModelData();
            DefineCurrentDocumentLevels();
        }

        private void DefineInitialRevitModelData()
        {
            _currentDocument = RevitManager.Document;
            _activeView = _currentDocument.ActiveView;
            _activeViewPhaseName = _activeView.get_Parameter(BuiltInParameter.VIEW_PHASE).AsValueString();
            _currentModelServices = new RevitDocumentServices(_currentDocument);
        }

        private void DefineCurrentDocumentLevels()
        {
            _currentDocumentLevels = new List<LevelElement>();
            List<Level> levels = _currentModelServices.GetLevels();
            foreach (Level level in levels)
            {
                LevelElement levelElement = new LevelElement(level);
                _currentDocumentLevels.Add(levelElement);
            }
        }
    }
}
