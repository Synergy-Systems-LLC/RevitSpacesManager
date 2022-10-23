using RevitSpacesManager.Models;
using RevitSpacesManager.Revit.Services;
using System.Collections.Generic;

namespace RevitSpacesManager.ViewModels
{
    internal class MainWindowViewModel : ViewModel
    {
        #region Phases Property
        private List<PhaseElement> _phases;
        public List<PhaseElement> Phases
        {
            get => _phases;
            set => Set(ref _phases, value);
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
            Phases = _mainModel.CurrentRevitDocument.Phases;
            LinkedRevitDocuments = _mainModel.LinkedRevitDocuments;
        }
    }
}
