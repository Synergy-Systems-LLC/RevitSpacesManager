using RevitSpacesManager.Models;
using System.Windows;

namespace RevitSpacesManager.ViewModels
{
    internal class DeleteSelectedCommand : Command
    {
        private readonly MainWindowViewModel _viewModel;
        private readonly MainModel _mainModel;

        public DeleteSelectedCommand(MainWindowViewModel mainWindowViewModel, MainModel mainModel)
        {
            _viewModel = mainWindowViewModel;
            _mainModel = mainModel;
        }

        public override bool CanExecute(object parameter) => true;
        public override void Execute(object parameter)
        {
            if (_viewModel.IsNothingToDelete())
            {
                _viewModel.ShowNothingDeleteMessage();
                return;
            }

            if (_viewModel.IsCurrentPhaseNotSelected())
            {
                _viewModel.ShowPhaseNotSelectedMessage();
                return;
            }

            MessageGenerator messageGenerator = new MessageGenerator(
                _viewModel.CurrentObject(),
                _viewModel.CurrentSelectedNumber(),
                _viewModel.CurrentDocumentPhaseSelected,
                Actions.Delete
            );

            if (_viewModel.ShowConfirmationDialog(messageGenerator.MessageSelected) == MessageBoxResult.Cancel)
            {
                return;
            }

            if (_viewModel.CurrentDocumentSpaceChecked)
                _mainModel.DeleteSelectedSpaces(_viewModel.CurrentDocumentPhaseSelected);
            else
                _mainModel.DeleteSelectedRooms(_viewModel.CurrentDocumentPhaseSelected);

            _viewModel.ShowReportMessage(messageGenerator.ReportSelected);
            _viewModel.OnPropertyChanged(nameof(_viewModel.CurrentDocumentSpaceChecked));
        }
    }
}
