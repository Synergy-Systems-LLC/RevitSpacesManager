using RevitSpacesManager.Models;
using System.Windows;

namespace RevitSpacesManager.ViewModels
{
    internal class CreateAllCommand : Command
    {
        public ICreating Model { get; set; }

        private readonly MainWindowViewModel _viewModel;


        public CreateAllCommand(MainWindowViewModel mainWindowViewModel, ICreating model)
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

            if (Model.IsWorksetNotAvailable())
            {
                _viewModel.ShowMissingWorksetMessage();
                return;
            }

            RoomsVerificationReport verificationReport = Model.VerifyLinkRooms(_viewModel.LinkedDocumentSelected);
            // TODO ReportMessage

            MessageGenerator messageGenerator = new MessageGenerator(
                _viewModel.ActiveObject,
                _viewModel.LinkedDocumentSelected.NumberOfRooms,
                _viewModel.LinkedDocumentPhases
                );
            MessageBoxResult result = _viewModel.ShowConfirmationDialog(messageGenerator.MessageCreateAll());

            if (result == MessageBoxResult.Cancel)
            {
                return;
            }

            Model.CreateAllByLinkedDocument(_viewModel.LinkedDocumentSelected);

            _viewModel.ShowReportMessage(messageGenerator.ReportCreateAll());
            _viewModel.OnPropertyChanged(nameof(_viewModel.LinkedDocumentSelected));
        }
    }
}
