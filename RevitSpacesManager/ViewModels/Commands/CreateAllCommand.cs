using RevitSpacesManager.Models;
using System.Windows;

namespace RevitSpacesManager.ViewModels
{
    internal class CreateAllCommand : Command
    {
        private readonly MainWindowViewModel _viewModel;
        private readonly MainModel _mainModel;

        public CreateAllCommand(MainWindowViewModel mainWindowViewModel, MainModel mainModel)
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

            MessageGenerator messageGenerator = new MessageGenerator(
                _viewModel.LinkedObject(),
                _viewModel.LinkedDocumentSelected.NumberOfRooms,
                _viewModel.LinkedDocumentPhases,
                Actions.Create
                );
            MessageBoxResult result = _viewModel.ShowConfirmationDialog(messageGenerator.MessageAll);

            if (result == MessageBoxResult.Cancel)
            {
                return;
            }

            if (_viewModel.LinkedDocumentSpaceChecked)
                _mainModel.CreateAllSpacesByLinkRooms(_viewModel.LinkedDocumentSelected);
            else
                _mainModel.CreateAllRoomsByLinkRooms(_viewModel.LinkedDocumentSelected);

            _viewModel.ShowReportMessage(messageGenerator.ReportAll);
            _viewModel.OnPropertyChanged(nameof(_viewModel.LinkedDocumentSelected));
        }
    }
}
