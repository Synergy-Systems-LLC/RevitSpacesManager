using RevitSpacesManager.Models;
using RevitSpacesManager.Revit.Services;
using System.Collections.Generic;
using System.Linq;
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
                OnPropertyChanged("CurrentPhaseDisplayPath");
                OnPropertyChanged("CurrentDocumentPhases");
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
                OnPropertyChanged("LinkedDocumentPhases");
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
        private bool CanExitCommandExecute(object p) => true;
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
        private bool CanHelpCommandExecute(object p) => true;
        private void OnHelpCommandExecuted(object p)
        {
            ShowReadmeMessage();
        }
        #endregion

        #region DeleteAllCommand
        public ICommand DeleteAllCommand { get; }
        private bool CanDeleteAllCommandExecute(object p) => true;
        private void OnDeleteAllCommandExecuted(object p)
        {
            if (IsAnythingToDelete())
            {
                MessageBoxResult result = ShowDeleteAllDialog();
                if (result == MessageBoxResult.OK)
                {
                    string reportMessage = DeleteAll();
                    ShowReportMessage(reportMessage);
                    CurrentDocumentSpaceChecked = CurrentDocumentSpaceChecked;
                }
            }
            else
            {
                ShowNothingDeleteMessage();
            }
        }
        #endregion

        #region DeleteSelectedCommand
        public ICommand DeleteSelectedCommand { get; }
        private bool CanDeleteSelectedCommandExecute(object p) => true;
        private void OnDeleteSelectedCommandExecuted(object p)
        {
            if (IsAnythingToDelete())
            {
                if (IsCurrentPhaseSelected())
                {
                    MessageBoxResult result = ShowDeleteSelectedDialog();
                    if (result == MessageBoxResult.OK)
                    {
                        string reportMessage = DeleteSelected();
                        ShowReportMessage(reportMessage);
                        CurrentDocumentSpaceChecked = CurrentDocumentSpaceChecked;
                    }
                }
                else
                {
                    ShowPhaseNotSelectedMessage();
                }
            }
            else
            {
                ShowNothingDeleteMessage();
            }
        }
        #endregion

        #region CreateAllCommand
        public ICommand CreateAllCommand { get; }
        private bool CanCreateAllCommandExecute(object p) => true;
        private void OnCreateAllCommandExecuted(object p)
        {
            if(IsLinkSelected())
            {
                if (IsAnythingToCreate())
                {
                    MessageBoxResult result = ShowCreateAllDialog();
                    if (result == MessageBoxResult.OK)
                    {
                        string reportMessage = CreateAll();
                        ShowReportMessage(reportMessage);
                        LinkedDocumentSelected = LinkedDocumentSelected;
                    }
                }
                else
                {
                    ShowNothingCreateMessage();
                }
            }
            else
            {
                ShowLinkNotSelectedMessage();
            }
        }
        #endregion

        #region CreateSelectedCommand
        public ICommand CreateSelectedCommand { get; }
        private bool CanCreateSelectedCommandExecute(object p) => true;
        private void OnCreateSelectedCommandExecuted(object p)
        {
            if (IsLinkSelected())
            {
                if (IsAnythingToCreate())
                {
                    if (IsLinkedPhaseSelected())
                    {
                        MessageBoxResult result = ShowCreateSelectedDialog();
                        if (result == MessageBoxResult.OK)
                        {
                            string reportMessage = CreateSelected();
                            ShowReportMessage(reportMessage);
                            LinkedDocumentSelected = LinkedDocumentSelected;
                        }
                    }
                }
                else
                {
                    ShowNothingCreateMessage();
                }
            }
            else
            {
                ShowLinkNotSelectedMessage();
            }
        }
        #endregion


        private readonly MainModel _mainModel;


        public MainWindowViewModel()
        {
            _mainModel = new MainModel();

            LinkedDocuments = _mainModel.LinkedRevitDocuments;
            CurrentDocumentSpaceChecked = true;
            LinkedDocumentSpaceChecked = true;

            ExitCommand = new LambdaCommand(OnExitCommandExecuted, CanExitCommandExecute);
            HelpCommand = new LambdaCommand(OnHelpCommandExecuted, CanHelpCommandExecute);
            DeleteAllCommand = new LambdaCommand(OnDeleteAllCommandExecuted, CanDeleteAllCommandExecute);
            DeleteSelectedCommand = new LambdaCommand(OnDeleteSelectedCommandExecuted, CanDeleteSelectedCommandExecute);
            CreateAllCommand = new LambdaCommand(OnCreateAllCommandExecuted, CanCreateAllCommandExecute);
            CreateSelectedCommand = new LambdaCommand(OnCreateSelectedCommandExecuted, CanCreateSelectedCommandExecute);
        }

        private void ShowReadmeMessage()
        {
            string message = "  Алгоритм работы плагина:\n" +
                 "  - При запуске считываются пространства и помещения из текущей открытой модели для дальнейших действий с ними (полного и частичного удаления). Удаление осуществляется нажатием кнопок Delete All или Delete Selected.\n\n" +
                 "  - Считываются подгруженные линки и количество помещений в них для дальнейшего создания аналогичных пространств или помещений в текущей модели (полного и частичного создания). Создание осуществляется нажатием кнопок Create All или Create Selected для конкретного линка или конкретной фазы выбранного линка.\n\n" +
                 "  - Перед созданием пространств и помещений производится проверка на наличие в модели рабочего набора 'Model Spaces' или 'Model Rooms'.\n\n" +
                 "  - Перед созданием пространств и помещений производится проверка на корректность размещения помещений в выбранном линке, на наличие совпадающих по имени и отметке уровней, содержащих помещения, в линке и текущей модели. Помещения, не прошедшие проверку, не создаются, выводятся в информационном окне подтверждения создания новых пространств или помещений с рекомендациями по устранению ошибок.\n\n" +
                 "  - При создании новых пространств и помещений производится перенос данных об уровне, координатах расположения, верхней и нижней границе из модели линка. Созданные пространства и помещения автоматически попадают в рабочие наборы 'Model Spaces' и 'Model Rooms'.\n\n" +
                 "                                                           Молодец, читаешь инструкцию <3";
            MessageBox.Show(message, "Readme");
        }
        private void ShowNothingDeleteMessage()
        {
            string message = $"There is no {CurrentObject()}s to Delete in the Current Project";
            ShowInformationMessage(message);
        }
        private void ShowPhaseNotSelectedMessage()
        {
            string message = "Please define Project phase to work with.";
            ShowInformationMessage(message);
        }
        private MessageBoxResult ShowDeleteAllDialog()
        {
            int number = CurrentNumber();
            string currentObject = CurrentObject();
            int currentPhasesNumber = CurrentDocumentPhases.Count;
            string message = $"You are going to delete {number} {currentObject}{IsPlural(number)} in {currentPhasesNumber} Phase{IsPlural(currentPhasesNumber)}:\n";
            foreach (PhaseElement phase in CurrentDocumentPhases)
                message += $"   - {phase.Name}\n";

            MessageBoxResult result = ShowInformationDialog(message);
            return result;
        }
        private MessageBoxResult ShowDeleteSelectedDialog()
        {
            int number = CurrentSelectedNumber();
            string currentObject = CurrentObject();
            string phaseName = CurrentDocumentPhaseSelected.Name;
            string message = $"You are going to delete {number} {currentObject}{IsPlural(number)} in the '{phaseName}' Phase";

            MessageBoxResult result = ShowInformationDialog(message);
            return result;
        }
        private void ShowLinkNotSelectedMessage()
        {
            string message = "Please define Linked Model to work with.";
            ShowInformationMessage(message);
        }
        private void ShowNothingCreateMessage()
        {
            string message = $"There is no {CurrentObject()}s to Create from the Linked Model";
            ShowInformationMessage(message);
        }
        private MessageBoxResult ShowCreateAllDialog()
        {
            int number = LinkedNumber();
            string currentObject = LinkedObject();
            int linkedPhasesNumber = LinkedDocumentPhases.Count;
            string message = $"You are going to create {number} {currentObject}{IsPlural(number)} from {linkedPhasesNumber} Phase{IsPlural(linkedPhasesNumber)}:\n";
            foreach (PhaseElement phase in LinkedDocumentPhases)
                message += $"   - {phase.Name}\n";

            MessageBoxResult result = ShowInformationDialog(message);
            return result;
        }
        private MessageBoxResult ShowCreateSelectedDialog()
        {
            int number = LinkedSelectedNumber();
            string currentObject = LinkedObject();
            string phaseName = LinkedDocumentPhaseSelected.Name;
            string message = $"You are going to delete {number} {currentObject}{IsPlural(number)} in the '{phaseName}' Phase";

            MessageBoxResult result = ShowInformationDialog(message);
            return result;
        }
        private MessageBoxResult ShowInformationDialog(string informationMessage)
        {
            string title = "Information";
            MessageBoxResult result = MessageBox.Show(informationMessage, title, MessageBoxButton.OKCancel);
            return result;
        }
        private void ShowInformationMessage(string informationMessage)
        {
            string title = "Information";
            MessageBox.Show(informationMessage, title);
        }
        private void ShowReportMessage(string reportMessage)
        {
            string title = "Report";
            MessageBox.Show(reportMessage, title);
        }

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
        private string CurrentObject()
        {
            if (CurrentDocumentSpaceChecked)
                return "Space";
            return "Room";
        }
        private int CurrentNumber()
        {
            int number;
            if (CurrentDocumentSpaceChecked)
                number = _mainModel.CurrentRevitDocument.NumberOfSpaces;
            else
                number = _mainModel.CurrentRevitDocument.NumberOfRooms;
            return number;
        }
        private int CurrentSelectedNumber()
        {
            int number;
            if (CurrentDocumentSpaceChecked)
                number = CurrentDocumentPhaseSelected.NumberOfSpaces;
            else
                number = CurrentDocumentPhaseSelected.NumberOfRooms;
            return number;
        }
        private string LinkedObject()
        {
            if (LinkedDocumentSpaceChecked)
                return "Space";
            return "Room";
        }
        private int LinkedNumber()
        {
            int number;
            if (LinkedDocumentSpaceChecked)
                number = LinkedDocumentSelected.NumberOfSpaces;
            else
                number = LinkedDocumentSelected.NumberOfRooms;
            return number;
        }
        private int LinkedSelectedNumber()
        {
            int number;
            if (LinkedDocumentSpaceChecked)
                number = LinkedDocumentPhaseSelected.NumberOfSpaces;
            else
                number = LinkedDocumentPhaseSelected.NumberOfRooms;
            return number;
        }

        private bool IsAnythingToDelete() => CurrentNumber() > 0;
        private bool IsCurrentPhaseSelected() => CurrentDocumentPhaseSelected != null;
        private bool IsLinkSelected() => LinkedDocumentSelected != null;
        private bool IsAnythingToCreate() => LinkedDocumentSelected != null;
        private bool IsLinkedPhaseSelected() => LinkedDocumentPhaseSelected != null;
        private string IsPlural(int number)
        {
            if (number == 1)
                return "";
            return "s";
        }

        private string DeleteAll()
        {
            if (CurrentDocumentSpaceChecked)
                return _mainModel.DeleteAllSpaces();
            return _mainModel.DeleteAllRooms();
        }
        private string DeleteSelected()
        {
            if (CurrentDocumentSpaceChecked)
                return _mainModel.DeleteSelectedSpaces(CurrentDocumentPhaseSelected);
            return _mainModel.DeleteSelectedRooms(CurrentDocumentPhaseSelected);
        }
        private string CreateAll()
        {
            if (LinkedDocumentSpaceChecked)
                return _mainModel.CreateAllSpaces(LinkedDocumentSelected);
            return _mainModel.CreateAllRooms(LinkedDocumentSelected);
        }
        private string CreateSelected()
        {
            if (CurrentDocumentSpaceChecked)
                return _mainModel.CreateSelectedSpaces(LinkedDocumentPhaseSelected);
            return _mainModel.CreateSelectedRooms(LinkedDocumentPhaseSelected);
        }
    }
}
