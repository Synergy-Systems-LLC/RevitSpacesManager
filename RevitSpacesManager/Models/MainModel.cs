using Autodesk.Revit.DB;
using RevitSpacesManager.Revit;
using RevitSpacesManager.Revit.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RevitSpacesManager.Models
{
    internal class MainModel
    {
        private Document _currentDocument;
        private View _activeView;
        private string _activeViewPhaseName;
        private RevitDocumentServices _currentModelServices;

        internal MainModel()
        {
            //TODO
            //+ get active view phase and check
            //+ launch window
            //- get spaces workset id
            //- get rooms workset id
            //- get levels
            //- get link documents
            //- get spaces by phase
            //- get rooms by phase
            //- get rooms by link and phase

            DefineInitialRevitModelData();
        }

        private void DefineInitialRevitModelData()
        {
            _currentDocument = RevitManager.Document;
            _activeView = _currentDocument.ActiveView;
            _activeViewPhaseName = _activeView.get_Parameter(BuiltInParameter.VIEW_PHASE).AsValueString();
            _currentModelServices = new RevitDocumentServices(_currentDocument);
        }
    }
}
