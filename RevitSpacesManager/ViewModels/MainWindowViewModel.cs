using RevitSpacesManager.Models;
using RevitSpacesManager.Revit.Services;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Input;

namespace RevitSpacesManager.ViewModels
{
    internal class MainWindowViewModel : ViewModel
    {
        #region CurrentDocumentPhaseSelected Property
        private PhaseElement _currentDocumentPhaseSelected;
        public PhaseElement CurrentDocumentPhaseSelected
        {
            get => _currentDocumentPhaseSelected;
            set => Set(ref _currentDocumentPhaseSelected, value);
        }
        #endregion

        #region CurrentDocumentSpaceChecked Property
        private bool _currentDocumentSpaceChecked;
        public bool CurrentDocumentSpaceChecked
        {
            get => _currentDocumentSpaceChecked;
            set
            {
                Set(ref _currentDocumentSpaceChecked, value);
                OnPropertyChanged(nameof(CurrentPhaseDisplayPath));
                OnPropertyChanged(nameof(CurrentDocumentPhases));
            }
        }
        #endregion

        #region CurrentDocumentPhases Property
        public List<PhaseElement> CurrentDocumentPhases { get => CurrentPhases(); }
        #endregion

        #region CurrentPhaseDisplayPath Property
        public string CurrentPhaseDisplayPath { get => CurrentDisplayPath(); }
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

        #region LinkedDocumentSpaceChecked Property
        private bool _linkedDocumentSpaceChecked;
        public bool LinkedDocumentSpaceChecked
        {
            get => _linkedDocumentSpaceChecked;
            set => Set(ref _linkedDocumentSpaceChecked, value);
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

        public ICommand DeleteAllCommand { get; }
        public ICommand DeleteSelectedCommand { get; }
        public ICommand CreateAllCommand { get; }
        public ICommand CreateSelectedCommand { get; }


        private readonly MainModel _mainModel;

        public MainWindowViewModel()
        {
            _mainModel = new MainModel();

            LinkedDocuments = _mainModel.LinkedRevitDocuments;
            CurrentDocumentSpaceChecked = true;
            LinkedDocumentSpaceChecked = true;

            ExitCommand = new LambdaCommand(OnExitCommandExecuted);
            HelpCommand = new LambdaCommand(OnHelpCommandExecuted);

            DeleteAllCommand = new DeleteAllCommand(this, _mainModel);
            DeleteSelectedCommand = new DeleteSelectedCommand(this, _mainModel);
            CreateAllCommand = new CreateAllCommand(this, _mainModel);
            CreateSelectedCommand = new CreateSelectedCommand(this, _mainModel);

            //TODO
            // обдумать: заменить выбор (рум\спейс)с условия на полиморфизм и тумблер один на всех
        }

        internal void ShowNothingDeleteMessage()
        {
            string message = $"There are no {CurrentObject()}s to Delete in the Current Project";
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
            string message = $"There are no {CurrentObject()}s to Create from the selected Linked Model";
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

        internal string CurrentObject()
        {
            if (CurrentDocumentSpaceChecked)
                return "Space";
            return "Room";
        }
        internal int CurrentNumber()
        {
            int number;
            if (CurrentDocumentSpaceChecked)
                number = _mainModel.CurrentRevitDocument.NumberOfSpaces;
            else
                number = _mainModel.CurrentRevitDocument.NumberOfRooms;
            return number;
        }
        internal int CurrentSelectedNumber()
        {
            int number;
            if (CurrentDocumentSpaceChecked)
                number = CurrentDocumentPhaseSelected.NumberOfSpaces;
            else
                number = CurrentDocumentPhaseSelected.NumberOfRooms;
            return number;
        }
        internal string LinkedObject()
        {
            if (LinkedDocumentSpaceChecked)
                return "Space";
            return "Room";
        }

        internal bool IsNothingToDelete() => CurrentNumber() == 0;
        internal bool IsCurrentPhaseNotSelected() => CurrentDocumentPhaseSelected == null;
        internal bool IsLinkNotSelected() => LinkedDocumentSelected == null;
        internal bool IsNothingToCreate() => LinkedDocumentSelected.NumberOfRooms == 0;
        internal bool IsLinkedPhaseNotSelected() => LinkedDocumentPhaseSelected == null;


        private List<PhaseElement> CurrentPhases()
        {
            if (CurrentDocumentSpaceChecked)
                return _mainModel.CurrentRevitDocument.Phases.Where(p => p.NumberOfSpaces > 0).ToList();
            return _mainModel.CurrentRevitDocument.Phases.Where(p => p.NumberOfRooms > 0).ToList();
        }
        private string CurrentDisplayPath()
        {
            if (CurrentDocumentSpaceChecked)
                return "SpacesItemName";
            return "RoomsItemName";
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
