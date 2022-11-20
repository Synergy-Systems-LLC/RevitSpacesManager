using RevitSpacesManager.Models;
using System.Windows;

namespace RevitSpacesManager.ViewModels
{
    internal class CreateSelectedCommand : Command
    {
        internal AreaModel Model { get; set; }

        private readonly MainWindowViewModel _viewModel;


        public CreateSelectedCommand(MainWindowViewModel mainWindowViewModel, AreaModel model)
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
                return ;
            }

            MessageGenerator messageGenerator = new MessageGenerator(
                _viewModel.ActiveObject,
                _viewModel.LinkedDocumentPhaseSelected.NumberOfRooms,
                _viewModel.LinkedDocumentPhaseSelected,
                Actions.Create
                );
            MessageBoxResult result = _viewModel.ShowConfirmationDialog(messageGenerator.MessageSelected);

            if (result == MessageBoxResult.Cancel)
            {
                return;
            }

            Model.CreateSelected();

            _viewModel.ShowReportMessage(messageGenerator.ReportSelected);
            _viewModel.OnPropertyChanged(nameof(_viewModel.LinkedDocumentSelected));
        }
    }
}
