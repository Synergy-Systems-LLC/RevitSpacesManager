using RevitSpacesManager.Models;
using RevitSpacesManager.Revit.Services;
using System.Collections.Generic;

namespace RevitSpacesManager.ViewModels
{
    internal class MainWindowViewModel : ViewModel
    {
        #region CurrentRevitDocument Property
        private RevitDocument _currentRevitDocument;
        public RevitDocument CurrentRevitDocument
        {
            get => _currentRevitDocument;
            set => Set(ref _currentRevitDocument, value);
        }
        #endregion

        #region LinkedRevitDocuments Property
        private List<RevitDocument> _linkedRevitDocuments;
        public List<RevitDocument> LinkedRevitDocuments
        {
            get => _linkedRevitDocuments;
            set => Set(ref _linkedRevitDocuments, value);
        }
        #endregion

        private readonly MainModel _mainModel;

        public MainWindowViewModel()
        {
            _mainModel = new MainModel();
            CurrentRevitDocument = _mainModel.CurrentRevitDocument;
            LinkedRevitDocuments = _mainModel.LinkedRevitDocuments;
        }
    }
}
