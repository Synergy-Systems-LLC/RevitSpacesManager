using RevitSpacesManager.Models;
using System.Collections.Generic;
using System.Text;

namespace RevitSpacesManager.ViewModels
{
    internal class MessageGenerator
    {
        private readonly int _phasesNumber;
        private readonly int _documentElementsNumber;
        private readonly int _selectedPhaseElementsNumber;
        private readonly int _incorrectlyPlacedElementsNumber;
        private readonly int _incorrectLevelElementsNumber;
        private readonly string _areaObjectName;
        private readonly string _selectedPhaseName;
        private readonly string _phases;
        private readonly string _linkedDocumentTitle;
        private readonly RoomsVerificationReport _report;


        internal MessageGenerator(MainWindowViewModel viewModel)
        {
            _areaObjectName = viewModel.GetModelAreaName();
            _documentElementsNumber = viewModel.GetCurrentNumberOfElements();

            if (viewModel.CurrentDocumentPhaseSelected != null)
            {
                _selectedPhaseName = viewModel.CurrentDocumentPhaseSelected.Name;
                _selectedPhaseElementsNumber = viewModel.GetCurrentSelectedPhaseNumberOfElements();
            }

            var phases = viewModel.CurrentDocumentPhases;
            _phasesNumber = phases.Count;
            _phases = GetPhasesString(phases);
        }

        internal MessageGenerator(MainWindowViewModel viewModel, RoomsVerificationReport report)
        {
            _areaObjectName = viewModel.GetModelAreaName();
            _linkedDocumentTitle = viewModel.LinkedDocumentSelected.Title;
            _selectedPhaseName = viewModel.LinkedDocumentPhaseSelected.Name;
            _phasesNumber = viewModel.LinkedDocumentPhases.Count;

            _report = report;
            _documentElementsNumber = _report.VerifiedRooms.Count;
            _selectedPhaseElementsNumber = _report.VerifiedRooms.Count;

            _incorrectlyPlacedElementsNumber = _report.IncorrectyPlacedRooms.Count;
            _incorrectLevelElementsNumber = _report.IncorrectLevelRooms.Count;
        }


        internal string MessageCreateAll()
        {
            StringBuilder sb = new StringBuilder();
            if (IsAnyIncorrectlyPlacedElement() || IsAnyIncorrectLevelElement())
            {
                DefineWarningStrings(sb);
                sb.Append($"in the '{_linkedDocumentTitle}' linked model.\n");
                sb.Append($"Please contact Architecture team.\n\n\n");
            }

            sb.Append($"You are going to create ");
            sb.Append($"{_documentElementsNumber} ");
            sb.Append($"{_areaObjectName}{PluralSuffix(_documentElementsNumber)} by Rooms from ");
            sb.Append($"{_phasesNumber} ");
            sb.Append($"Phase{PluralSuffix(_phasesNumber)} of the ");
            sb.Append($"'{_linkedDocumentTitle}' linked Model");
            return sb.ToString();
        }
        internal string ReportCreateAll()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append($"{_documentElementsNumber} ");
            sb.Append($"{_areaObjectName}{PluralSuffix(_documentElementsNumber)} ");
            sb.Append($"ha{HaveSuffix(_documentElementsNumber)} been created by Rooms from ");
            sb.Append($"{_phasesNumber} ");
            sb.Append($"Phase{PluralSuffix(_phasesNumber)} \nof the ");
            sb.Append($"'{_linkedDocumentTitle}' linked Model");
            return sb.ToString();
        }
        internal string MessageCreateSelected()
        {
            StringBuilder sb = new StringBuilder();
            if (IsAnyIncorrectlyPlacedElement() || IsAnyIncorrectLevelElement())
            {
                DefineWarningStrings(sb);
                sb.Append($"in the '{_selectedPhaseName}' phase of the '{_linkedDocumentTitle}' linked model.\n");
                sb.Append($"Please contact Architecture team.\n\n\n");
            }

            sb.Append($"You are going to create ");
            sb.Append($"{_selectedPhaseElementsNumber} ");
            sb.Append($"{_areaObjectName}{PluralSuffix(_selectedPhaseElementsNumber)} by Rooms from the ");
            sb.Append($"'{_selectedPhaseName}' phase of the ");
            sb.Append($"'{_linkedDocumentTitle}' linked Model");
            return sb.ToString();
        }
        internal string ReportCreateSelected()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append($"{_selectedPhaseElementsNumber} ");
            sb.Append($"{_areaObjectName}{PluralSuffix(_selectedPhaseElementsNumber)} ");
            sb.Append($"ha{HaveSuffix(_selectedPhaseElementsNumber)} been crated by Rooms from the ");
            sb.Append($"'{_selectedPhaseName}' phase \nof the ");
            sb.Append($"'{_linkedDocumentTitle}' linked Model");
            return sb.ToString();
        }
        internal string MessageDeleteAll()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append($"You are going to delete ");
            sb.Append($"{_documentElementsNumber} ");
            sb.Append($"{_areaObjectName}{PluralSuffix(_documentElementsNumber)} in ");
            sb.Append($"{_phasesNumber} ");
            sb.Append($"Phase{PluralSuffix(_phasesNumber)} of the current Model:\n");
            sb.Append($"{_phases}");
            return sb.ToString();
        }
        internal string ReportDeleteAll()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append($"{_documentElementsNumber} ");
            sb.Append($"{_areaObjectName}{PluralSuffix(_documentElementsNumber)} ");
            sb.Append($"ha{HaveSuffix(_documentElementsNumber)} been deleted in ");
            sb.Append($"{_phasesNumber} ");
            sb.Append($"Phase{PluralSuffix(_phasesNumber)} of the current Model");
            return sb.ToString();
        }
        internal string MessageDeleteSelected()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append($"You are going to delete ");
            sb.Append($"{_selectedPhaseElementsNumber} ");
            sb.Append($"{_areaObjectName}{PluralSuffix(_selectedPhaseElementsNumber)} in the ");
            sb.Append($"'{_selectedPhaseName}' phase \nof the current Model");
            return sb.ToString();
        }
        internal string ReportDeleteSelected()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append($"{_selectedPhaseElementsNumber} ");
            sb.Append($"{_areaObjectName}{PluralSuffix(_selectedPhaseElementsNumber)} ");
            sb.Append($"ha{HaveSuffix(_selectedPhaseElementsNumber)} been deleted in the ");
            sb.Append($"'{_selectedPhaseName}' phase \nof the current Model");
            return sb.ToString();
        }


        private string GetPhasesString(List<PhaseElement> phases)
        {
            string phasesString = string.Empty;
            foreach (PhaseElement phase in phases)
                phasesString += $"   - {phase.Name}\n";
            return phasesString;
        }
        private string ToBe(int number)
        {
            if (number == 1)
                return "is";
            return "are";
        }
        private string PluralSuffix(int number)
        {
            if (number == 1)
                return "";
            return "s";
        }
        private string HaveSuffix(int number)
        {
            if (number == 1)
                return "s";
            return "ve";
        }

        private bool IsAnyIncorrectlyPlacedElement()
        {
            return _incorrectlyPlacedElementsNumber != 0;
        }
        private bool IsAnyIncorrectLevelElement()
        {
            return _incorrectLevelElementsNumber != 0;
        }

        private StringBuilder DefineWarningStrings(StringBuilder sb)
        {
            sb.Append($"WARNING(!)\n");
            if (IsAnyIncorrectlyPlacedElement())
            {
                sb.Append($" - {_incorrectlyPlacedElementsNumber} ");
                sb.Append($"{_areaObjectName}{PluralSuffix(_documentElementsNumber)} ");
                sb.Append($"{ToBe(_incorrectlyPlacedElementsNumber)} Not placed correctly\n");
                sb.Append($"{GetRoomsIdsString(_report.IncorrectyPlacedRooms)}\n");
            }

            if (IsAnyIncorrectLevelElement())
            {
                sb.Append($" - {_incorrectLevelElementsNumber} ");
                sb.Append($"{_areaObjectName}{PluralSuffix(_documentElementsNumber)} ");
                sb.Append($"ha{HaveSuffix(_incorrectLevelElementsNumber)} Not matching Levels\n\n");
            }
            return sb;
        }
        private string GetRoomsIdsString(List<RoomElement> roomElements)
        {
            string roomsIds = string.Empty;
            foreach (RoomElement roomElement in roomElements)
            {
                roomsIds += $"       {roomElement.Id}\n";
            }
            return roomsIds;
        }
    }
}
