using RevitSpacesManager.Models;
using System.Windows;

namespace RevitSpacesManager.ViewModels
{
    internal class DeleteAllCommand : Command
    {
        private readonly MainWindowViewModel _viewModel;
        private readonly MainModel _mainModel;

        public DeleteAllCommand(MainWindowViewModel mainWindowViewModel, MainModel mainModel)
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

            MessageGenerator messageGenerator = new MessageGenerator(
                _viewModel.CurrentObject(),
                _viewModel.CurrentNumber(),
                _viewModel.CurrentDocumentPhases,
                Actions.Delete
                );
            MessageBoxResult result = _viewModel.ShowConfirmationDialog(messageGenerator.MessageAll);

            if (result == MessageBoxResult.Cancel)
            {
                return;
            }

            if (_viewModel.CurrentDocumentSpaceChecked)
                _mainModel.DeleteAllSpaces();
            else
                _mainModel.DeleteAllRooms();

            _viewModel.ShowReportMessage(messageGenerator.ReportAll);
            _viewModel.OnPropertyChanged(nameof(_viewModel.CurrentDocumentSpaceChecked));
        }
    }
}
