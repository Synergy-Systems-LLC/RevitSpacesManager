using RevitSpacesManager.Models;
using System.Windows;

namespace RevitSpacesManager.ViewModels
{
    internal class DeleteAllCommand : Command
    {
        internal IDeleting Model { get; set; }

        private readonly MainWindowViewModel _viewModel;

        public DeleteAllCommand(MainWindowViewModel mainWindowViewModel, IDeleting model)
        {
            _viewModel = mainWindowViewModel;
            Model = model;
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
                _viewModel.ActiveObject,
                _viewModel.GetCurrentNumberOfElements(),
                _viewModel.CurrentDocumentPhases,
                Actions.Delete
                );
            MessageBoxResult result = _viewModel.ShowConfirmationDialog(messageGenerator.MessageAll());

            if (result == MessageBoxResult.Cancel)
            {
                return;
            }

            Model.DeleteAll();

            _viewModel.ShowReportMessage(messageGenerator.ReportAll());
            _viewModel.OnPropertyChanged(nameof(_viewModel.CurrentDocumentPhases));
        }
    }
}
