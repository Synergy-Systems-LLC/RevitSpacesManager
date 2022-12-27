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

            if (Model.AreNotAllElementsEditable())
            {
                _viewModel.ShowNoAccessMessage();
                return;
            }

            var messageGenerator = new MessageGenerator(_viewModel);
            string deleteAllMessage = messageGenerator.MessageDeleteAll();
            MessageBoxResult result = _viewModel.ShowConfirmationDialog(deleteAllMessage);

            if (result == MessageBoxResult.Cancel)
                return;

            Model.DeleteAll();

            string deleteAllReport = messageGenerator.ReportDeleteAll();
            _viewModel.ShowReportMessage(deleteAllReport);

            _viewModel.OnPropertyChanged(nameof(_viewModel.CurrentDocumentPhases));
        }
    }
}
