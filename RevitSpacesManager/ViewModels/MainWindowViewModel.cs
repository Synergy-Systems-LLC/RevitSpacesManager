using RevitSpacesManager.Models;
using RevitSpacesManager.Revit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Input;

namespace RevitSpacesManager.ViewModels
{
    internal class MainWindowViewModel : ViewModel
    {
        #region AreSpacesChecked Property
        private bool _areSpacesChecked;
        public bool AreSpacesChecked
        {
            get => _areSpacesChecked;
            set
            {
                Set(ref _areSpacesChecked, value);

                DefineActiveModel(value);
                OnPropertyChanged(nameof(CurrentDocumentPhases));
                OnPropertyChanged(nameof(CurrentPhaseDisplayPath));
            }
        }
        #endregion

        #region CurrentDocumentPhaseSelected Property
        private PhaseElement _currentDocumentPhaseSelected;
        public PhaseElement CurrentDocumentPhaseSelected
        {
            get => _currentDocumentPhaseSelected;
            set => Set(ref _currentDocumentPhaseSelected, value);
        }
        #endregion

        #region CurrentDocumentPhases Property
        public List<PhaseElement> CurrentDocumentPhases => _activeModel.GetPhases();
        #endregion

        #region CurrentPhaseDisplayPath Property
        public string CurrentPhaseDisplayPath => _activeModel.SelectedPhaseDisplayPath;
        #endregion

        #region LinkedDocuments Property
        private List<RevitDocument> _linkedDocuments;
        public List<RevitDocument> LinkedDocuments
        {
            get => _linkedDocuments;
            set => Set(ref _linkedDocuments, value);
        }
        #endregion

        #region LinkedDocumentSelected Property
        private RevitDocument _linkedDocumentSelected;
        public RevitDocument LinkedDocumentSelected
        {
            get => _linkedDocumentSelected;
            set
            {
                Set(ref _linkedDocumentSelected, value);
                OnPropertyChanged(nameof(LinkedDocumentPhases));
                LinkedDocumentPhaseSelected = LinkedDocumentPhases[0];
            }
        }
        #endregion

        #region LinkedDocumentPhases Property
        public List<PhaseElement> LinkedDocumentPhases { get => _linkedDocumentSelected.Phases.Where(p => p.NumberOfRooms > 0).ToList(); }
        #endregion

        #region LinkedDocumentPhaseSelected Property
        private PhaseElement _linkedDocumentPhaseSelected;
        public PhaseElement LinkedDocumentPhaseSelected
        {
            get => _linkedDocumentPhaseSelected;
            set => Set(ref _linkedDocumentPhaseSelected, value);
        }
        #endregion


        #region ExitCommand
        public ICommand ExitCommand { get; }
        private void OnExitCommandExecuted(object p)
        {
            if (p is Window mainWindow)
            {
                mainWindow.Close();
            }
        }
        #endregion

        #region HelpCommand
        public ICommand HelpCommand { get; }
        private void OnHelpCommandExecuted(object p)
        {
            ShowReadmeMessage();
        }
        #endregion

        public Command DeleteAllCommand { get; }
        public Command DeleteSelectedCommand { get; }
        public Command CreateAllCommand { get; }
        public Command CreateSelectedCommand { get; }


        private IModel _activeModel;

        private readonly RevitDocument _currentDocument;
        private readonly SpacesModel _spacesModel;
        private readonly RoomsModel _roomsModel;

        public MainWindowViewModel()
        {
            _currentDocument = new RevitDocument(RevitManager.Document);
            LinkedDocuments = _currentDocument.GetRevitLinkDocuments();

            _spacesModel = new SpacesModel(_currentDocument);
            _roomsModel = new RoomsModel(_currentDocument);

            ExitCommand = new LambdaCommand(OnExitCommandExecuted);
            HelpCommand = new LambdaCommand(OnHelpCommandExecuted);

            DeleteAllCommand = new DeleteAllCommand(this);
            DeleteSelectedCommand = new DeleteSelectedCommand(this);
            CreateAllCommand = new CreateAllCommand(this);
            CreateSelectedCommand = new CreateSelectedCommand(this);

            AreSpacesChecked = true;

            //TODO
            // на view выделять цветом Space Room
        }

        internal void ShowNothingDeleteMessage()
        {
            string message = $"There are no {_activeModel.ObjectName}s to Delete in the Current Project";
            ShowInformationMessage(message);
        }
        internal void ShowPhaseNotSelectedMessage()
        {
            string message = "Please define Project phase to work with.";
            ShowInformationMessage(message);
        }
        internal void ShowLinkNotSelectedMessage()
        {
            string message = "Please define Linked Model to work with.";
            ShowInformationMessage(message);
        }
        internal void ShowNothingCreateMessage()
        {
            string message = $"There are no {_activeModel.ObjectName}s to Create from the selected Linked Model";
            ShowInformationMessage(message);
        }
        internal void ShowReportMessage(string reportMessage)
        {
            string title = "Report";
            MessageBox.Show(reportMessage, title);
        }
        internal MessageBoxResult ShowConfirmationDialog(string message)
        {
            string title = "Information";
            MessageBoxResult result = MessageBox.Show(message, title, MessageBoxButton.OKCancel);
            return result;
        }

        internal int GetCurrentSelectedPhaseNumberOfElements()
        {
            if (AreSpacesChecked)
                return CurrentDocumentPhaseSelected.NumberOfSpaces;
            return CurrentDocumentPhaseSelected.NumberOfRooms;
        }

        internal bool IsNothingToDelete() => _activeModel.NumberOfElements == 0;
        internal bool IsCurrentPhaseNotSelected() => CurrentDocumentPhaseSelected == null;
        internal bool IsLinkNotSelected() => LinkedDocumentSelected == null;
        internal bool IsNothingToCreate() => LinkedDocumentSelected.NumberOfRooms == 0;
        internal bool IsLinkedPhaseNotSelected() => LinkedDocumentPhaseSelected == null;

        private void DefineActiveModel(bool value)
        {
            if (value)
            {
                _activeModel = _spacesModel;
            }
            else
            {
                _activeModel = _roomsModel;
            }

            DeleteAllCommand.Model = _activeModel;
            DeleteSelectedCommand.Model = _activeModel;
            CreateAllCommand.Model = _activeModel;
            CreateSelectedCommand.Model = _activeModel;
        }
        private void ShowReadmeMessage()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("  Алгоритм работы плагина:\n");
            sb.Append("  - При запуске считываются пространства и помещения из текущей открытой модели для дальнейших действий с ними (полного и частичного удаления). Удаление осуществляется нажатием кнопок Delete All или Delete Selected.\n\n");
            sb.Append("  - Считываются подгруженные линки и количество помещений в них для дальнейшего создания аналогичных пространств или помещений в текущей модели (полного и частичного создания). Создание осуществляется нажатием кнопок Create All или Create Selected для конкретного линка или конкретной фазы выбранного линка.\n\n");
            sb.Append("  - Перед созданием пространств и помещений производится проверка на наличие в модели рабочего набора 'Model Spaces' или 'Model Rooms'.\n\n");
            sb.Append("  - Перед созданием пространств и помещений производится проверка на корректность размещения помещений в выбранном линке, на наличие совпадающих по имени и отметке уровней, содержащих помещения, в линке и текущей модели. Помещения, не прошедшие проверку, не создаются, выводятся в информационном окне подтверждения создания новых пространств или помещений с рекомендациями по устранению ошибок.\n\n");
            sb.Append("  - При создании новых пространств и помещений производится перенос данных об уровне, координатах расположения, верхней и нижней границе из модели линка. Созданные пространства и помещения автоматически попадают в рабочие наборы 'Model Spaces' и 'Model Rooms'.\n\n");
            sb.Append("                                                           Молодец, читаешь инструкцию <3");
            string message = sb.ToString();
            MessageBox.Show(message, "Readme");
        }
        private void ShowInformationMessage(string informationMessage)
        {
            string title = "Information";
            MessageBox.Show(informationMessage, title);
        }
    }
}
