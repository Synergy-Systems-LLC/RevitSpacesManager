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
        public List<PhaseElement> CurrentDocumentPhases
        {
            get
            {
                if (CurrentDocumentSpaceChecked)
                    return _mainModel.CurrentRevitDocument.Phases.Where(p => p.NumberOfSpaces > 0).ToList();
                return _mainModel.CurrentRevitDocument.Phases.Where(p => p.NumberOfRooms > 0).ToList();
            }
        }
        #endregion

        #region CurrentPhaseDisplayPath Property
        public string CurrentPhaseDisplayPath
        {
            get
            {
                if (CurrentDocumentSpaceChecked)
                    return "SpacesItemName";
                return "RoomsItemName";
            }
        }
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
        public List<PhaseElement> LinkedDocumentPhases
        {
            get => _linkedDocumentSelected.Phases.Where(p => p.NumberOfRooms > 0).ToList();
        }
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
            MessageBox.Show("DeleteAll");
        }
        #endregion

        #region DeleteSelectedCommand
        public ICommand DeleteSelectedCommand { get; }
        private bool CanDeleteSelectedCommandExecute(object p) => true;
        private void OnDeleteSelectedCommandExecuted(object p)
        {
            MessageBox.Show("DeleteSelected");
        }
        #endregion

        #region CreateAllCommand
        public ICommand CreateAllCommand { get; }
        private bool CanCreateAllCommandExecute(object p) => true;
        private void OnCreateAllCommandExecuted(object p)
        {
            MessageBox.Show("CreateAll");
        }
        #endregion

        #region CreateSelectedCommand
        public ICommand CreateSelectedCommand { get; }
        private bool CanCreateSelectedCommandExecute(object p) => true;
        private void OnCreateSelectedCommandExecuted(object p)
        {
            MessageBox.Show("CreateSelected");
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
    }
}
