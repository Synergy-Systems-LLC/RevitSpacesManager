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

            RevitDocument selectedLinkedDocument = _viewModel.LinkedDocumentSelected;
            var verificationReport = Model.VerifyRooms(selectedLinkedDocument);
            var messageGenerator = new MessageGenerator(_viewModel, verificationReport);
            string createAllMessage = messageGenerator.MessageCreateAll();
            MessageBoxResult result = _viewModel.ShowConfirmationDialog(createAllMessage);

            if (result == MessageBoxResult.Cancel)
                return;

            Model.CreateAllByRooms(verificationReport.VerifiedRooms);

            string createAllReport = messageGenerator.ReportCreateAll();
            _viewModel.ShowReportMessage(createAllReport);

            _viewModel.OnPropertyChanged(nameof(_viewModel.CurrentDocumentPhases));
        }
    }
}
