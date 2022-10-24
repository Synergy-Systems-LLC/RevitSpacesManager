using RevitSpacesManager.Models;
using RevitSpacesManager.Revit.Services;
using System.Collections.Generic;
using System.Linq;

namespace RevitSpacesManager.ViewModels
{
    internal class MainWindowViewModel : ViewModel
    {
        #region CurrentDocumentPhaseSelected Property
        private PhaseElement _currentDocumentPhaseSelected;
        public PhaseElement CurrentDocumentPhaseSelected
        {
            get => _currentDocumentPhaseSelected;
            set => Set(ref _currentDocumentPhaseSelected, value);
        }
        #endregion

        #region CurrentDocumentSpaceChecked Property
        private bool _currentDocumentSpaceChecked;
        public bool CurrentDocumentSpaceChecked
        {
            get => _currentDocumentSpaceChecked;
            set
            {
                Set(ref _currentDocumentSpaceChecked, value);
                OnPropertyChanged("CurrentPhaseDisplayPath");
                OnPropertyChanged("CurrentDocumentPhases");
            }
        }
        #endregion

        #region CurrentDocumentPhases Property
        public List<PhaseElement> CurrentDocumentPhases
        {
            get
            {
                if (CurrentDocumentSpaceChecked)
                    return _mainModel.CurrentRevitDocument.Phases.Where(p => p.NumberOfSpaces > 0).ToList();
                return _mainModel.CurrentRevitDocument.Phases.Where(p => p.NumberOfRooms > 0).ToList();
            }
        }
        #endregion

        #region CurrentPhaseDisplayPath Property
        public string CurrentPhaseDisplayPath
        {
            get
            {
                if (CurrentDocumentSpaceChecked)
                    return "SpacesItemName";
                return "RoomsItemName";
            }
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
            get => _linkedDocumentSelected.Phases.Where(p => p.NumberOfRooms > 0).ToList();
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

        #region LinkedDocumentSpaceChecked Property
        private bool _linkedDocumentSpaceChecked;
        public bool LinkedDocumentSpaceChecked
        {
            get => _linkedDocumentSpaceChecked;
            set => Set(ref _linkedDocumentSpaceChecked, value);
        }
        #endregion

        private readonly MainModel _mainModel;

        public MainWindowViewModel()
        {
            _mainModel = new MainModel();
            LinkedDocuments = _mainModel.LinkedRevitDocuments;
            CurrentDocumentSpaceChecked = true;
            LinkedDocumentSpaceChecked = true;
        }
    }
}
