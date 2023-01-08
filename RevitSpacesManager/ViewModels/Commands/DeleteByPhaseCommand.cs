using RevitSpacesManager.Models;
using System.Windows;

namespace RevitSpacesManager.ViewModels
{
    internal class DeleteByPhaseCommand : Command
    {
        internal IDeleting Model { get; set; }

        private readonly MainWindowViewModel _viewModel;


        public DeleteByPhaseCommand(MainWindowViewModel mainWindowViewModel, IDeleting model)
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

            if (_viewModel.IsCurrentPhaseNotSelected())
            {
                _viewModel.ShowPhaseNotSelectedMessage();
                return;
            }

            PhaseElement selectedPhase = _viewModel.CurrentDocumentPhaseSelected;
            if (Model.AreNotAllPhaseElementsEditable(selectedPhase))
            {
                _viewModel.ShowNoAccessMessage();
                return;
            }

            var messageGenerator = new MessageGenerator(_viewModel);
            string deleteSelectedMessage = messageGenerator.MessageDeleteSelected();
            MessageBoxResult result = _viewModel.ShowConfirmationDialog(deleteSelectedMessage);

            if (result == MessageBoxResult.Cancel)
                return;

            Model.DeleteByPhase(selectedPhase);

            string deleteSelectedReport = messageGenerator.ReportDeleteSelected();
            _viewModel.ShowReportMessage(deleteSelectedReport);

            _viewModel.OnPropertyChanged(nameof(_viewModel.CurrentDocumentPhases));
        }
    }
}
