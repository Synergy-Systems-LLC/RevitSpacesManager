﻿using RevitSpacesManager.Models;
using System.Windows;

namespace RevitSpacesManager.ViewModels
{
    internal class CreateAllCommand : Command
    {
        public override IModel Model { get; set; }

        private readonly MainWindowViewModel _viewModel;


        public CreateAllCommand(MainWindowViewModel mainWindowViewModel)
        {
            _viewModel = mainWindowViewModel;
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

            MessageGenerator messageGenerator = new MessageGenerator(
                Model.ObjectName,
                _viewModel.LinkedDocumentSelected.NumberOfRooms,
                _viewModel.LinkedDocumentPhases,
                Actions.Create
                );
            MessageBoxResult result = _viewModel.ShowConfirmationDialog(messageGenerator.MessageAll);

            if (result == MessageBoxResult.Cancel)
            {
                return;
            }

            Model.CreateAll();

            _viewModel.ShowReportMessage(messageGenerator.ReportAll);
            _viewModel.OnPropertyChanged(nameof(_viewModel.LinkedDocumentSelected));
        }
    }
}
