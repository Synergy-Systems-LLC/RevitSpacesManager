using RevitSpacesManager.Models;
using System.Windows;

namespace RevitSpacesManager.ViewModels
{
    internal class CreateSelectedCommand : Command
    {
        private readonly MainWindowViewModel _viewModel;
        private readonly MainModel _mainModel;

        public CreateSelectedCommand(MainWindowViewModel mainWindowViewModel, MainModel mainModel)
        {
            _viewModel = mainWindowViewModel;
            _mainModel = mainModel;
        }

        public override bool CanExecute(object parameter) => true;
        public override void Execute(object parameter)
        {
            if (_viewModel.IsLinkNotSelected())
            {
                _viewModel.ShowLinkNotSelectedMessage();
                return;
            }

            if (_viewModel.IsNothingToCreate())
            {
                _viewModel.ShowNothingCreateMessage();
                return;
            }

            if (_viewModel.IsLinkedPhaseNotSelected())
            {
                _viewModel.ShowPhaseNotSelectedMessage();
                return ;
            }

            MessageGenerator messageGenerator = new MessageGenerator(
                _viewModel.LinkedObject(),
                _viewModel.LinkedDocumentPhaseSelected.NumberOfRooms,
                _viewModel.LinkedDocumentPhaseSelected,
                Actions.Create
                );
            MessageBoxResult result = _viewModel.ShowConfirmationDialog(messageGenerator.MessageSelected);

            if (result == MessageBoxResult.Cancel)
            {
                return;
            }

            if (_viewModel.CurrentDocumentSpaceChecked)
                _mainModel.CreateSelectedSpacesByLinkRooms(_viewModel.LinkedDocumentPhaseSelected);
            else
                _mainModel.CreateSelectedRoomsByLinkRooms(_viewModel.LinkedDocumentPhaseSelected);

            _viewModel.ShowReportMessage(messageGenerator.ReportSelected);
            _viewModel.OnPropertyChanged(nameof(_viewModel.LinkedDocumentSelected));
        }
    }
}
