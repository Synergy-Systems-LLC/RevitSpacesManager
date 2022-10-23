using RevitSpacesManager.Models;
using RevitSpacesManager.Revit.Services;
using System.Collections.Generic;

namespace RevitSpacesManager.ViewModels
{
    internal class MainWindowViewModel : ViewModel
    {
        #region CurrentDocumentPhases Property
        private List<PhaseElement> _currentDocumentPhases;
        public List<PhaseElement> CurrentDocumentPhases
        {
            get => _currentDocumentPhases;
            set => Set(ref _currentDocumentPhases, value);
        }
        #endregion

        #region CurrentDocumentPhaseSelected Property
        private PhaseElement _currentDocumentPhaseSelected;
        public PhaseElement CurrentDocumentPhaseSelected
        {
            get => _currentDocumentPhaseSelected;
            set => Set(ref _currentDocumentPhaseSelected, value);
        }
        #endregion

        #region CurrentDocumentIsSpace Property
        private bool _currentDocumentIsSpace = true;
        public bool CurrentDocumentIsSpace
        {
            get => _currentDocumentIsSpace;
            set => Set(ref _currentDocumentIsSpace, value);
        }
        #endregion

        #region LinkedDocuments Property
        private List<RevitDocument> _linkedDocuments;
        public List<RevitDocument> LinkedDocuments
        {
            get => _linkedDocuments;
            set => Set(ref _linkedDocuments, value);
        }
        #endregion

        #region LinkedDocumentSelected Property
        private RevitDocument _linkedDocumentSelected;
        public RevitDocument LinkedDocumentSelected
        {
            get => _linkedDocumentSelected;
            set
            {
                Set(ref _linkedDocumentSelected, value);
                OnPropertyChanged("LinkedDocumentPhases");
                LinkedDocumentPhaseSelected = LinkedDocumentPhases[0];
            }
        }
        #endregion

        #region LinkedDocumentPhases Property
        public List<PhaseElement> LinkedDocumentPhases
        {
            get => _linkedDocumentSelected.Phases;
        }
        #endregion

        #region LinkedDocumentPhaseSelected Property
        private PhaseElement _linkedDocumentPhaseSelected;
        public PhaseElement LinkedDocumentPhaseSelected
        {
            get => _linkedDocumentPhaseSelected;
            set => Set(ref _linkedDocumentPhaseSelected, value);
        }
        #endregion

        private readonly MainModel _mainModel;

        public MainWindowViewModel()
        {
            _mainModel = new MainModel();
            CurrentDocumentPhases = _mainModel.CurrentRevitDocument.Phases;
            LinkedDocuments = _mainModel.LinkedRevitDocuments;
        }
    }
}
