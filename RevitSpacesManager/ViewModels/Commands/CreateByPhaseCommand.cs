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

            RoomsCreationVerificationReport verificationReport = Model.VerifyDocumentRoomsForCreation();

            MessageGenerator messageGenerator = new MessageGenerator(
                _viewModel.ActiveObject,
                _viewModel.LinkedDocumentPhaseSelected.NumberOfRooms,
                _viewModel.LinkedDocumentPhaseSelected
                );
            MessageBoxResult result = _viewModel.ShowConfirmationDialog(messageGenerator.MessageCreateSelected());

            if (result == MessageBoxResult.Cancel)
            {
                return;
            }

            Model.CreateByLinkedDocumentPhase(_viewModel.LinkedDocumentPhaseSelected);

            _viewModel.ShowReportMessage(messageGenerator.ReportCreateSelected());
            _viewModel.OnPropertyChanged(nameof(_viewModel.LinkedDocumentSelected));
        }
    }
}
