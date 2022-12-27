using RevitSpacesManager.Models;
using System.Windows;

namespace RevitSpacesManager.ViewModels
{
    internal class CreateByPhaseCommand : Command
    {
        internal ICreating Model { get; set; }

        private readonly MainWindowViewModel _viewModel;


        public CreateByPhaseCommand(MainWindowViewModel mainWindowViewModel, ICreating model)
        {
            _viewModel = mainWindowViewModel;
            Model = model;
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
                return;
            }

            if (Model.IsWorksetNotAvailable())
            {
                _viewModel.ShowMissingWorksetMessage();
                return;
            }

            PhaseElement selectedPhase = _viewModel.LinkedDocumentPhaseSelected;
            var verificationReport = Model.VerifyRooms(selectedPhase);
            var messageGenerator = new MessageGenerator(_viewModel, verificationReport);
            string createSelectedMessage = messageGenerator.MessageCreateSelected();
            MessageBoxResult result = _viewModel.ShowConfirmationDialog(createSelectedMessage);

            if (result == MessageBoxResult.Cancel)
                return;

            Model.CreateByPhaseRooms(verificationReport.VerifiedRooms, selectedPhase);

            string createSelectedReport = messageGenerator.ReportCreateSelected();
            _viewModel.ShowReportMessage(createSelectedReport);

            _viewModel.OnPropertyChanged(nameof(_viewModel.CurrentDocumentPhases));
        }
    }
}
