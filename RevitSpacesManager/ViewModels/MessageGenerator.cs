using RevitSpacesManager.Models;
using System.Collections.Generic;
using System.Text;

namespace RevitSpacesManager.ViewModels
{
    internal class MessageGenerator
    {
        internal string MessageAll
        {
            get
            {
                StringBuilder sb = new StringBuilder();
                sb.Append($"You are going to {_action} ");
                sb.Append($"{_number} ");
                sb.Append($"{_objectType}{PluralSuffix(_number)} ");
                sb.Append($"{_preposition} ");
                sb.Append($"{_phasesNumber} ");
                sb.Append($"Phase{PluralSuffix(_phasesNumber)} ");
                sb.Append($"{_suffix}:\n");
                sb.Append($"{_phases}");
                return sb.ToString();
            }

        }

        internal string ReportAll
        {
            get
            {
                StringBuilder sb = new StringBuilder();
                sb.Append($"{_number} ");
                sb.Append($"{_objectType}{PluralSuffix(_number)} ");
                sb.Append($"ha{HaveSuffix(_number)} been ");
                sb.Append($"{_action}d ");
                sb.Append($"{_preposition} ");
                sb.Append($"{_phasesNumber} ");
                sb.Append($"Phase{PluralSuffix(_phasesNumber)} ");
                sb.Append($"{_suffix}");
                return sb.ToString();
            }

        }

        internal string MessageSelected
        {
            get
            {
                StringBuilder sb = new StringBuilder();
                sb.Append($"You are going to {_action} ");
                sb.Append($"{_number} ");
                sb.Append($"{_objectType}{PluralSuffix(_number)} ");
                sb.Append($"{_preposition} the ");
                sb.Append($"'{_phaseName}' Phase ");
                sb.Append($"\n{_suffix}");
                return sb.ToString();
            }

        }

        internal string ReportSelected
        {
            get
            {
                StringBuilder sb = new StringBuilder();
                sb.Append($"{_number} ");
                sb.Append($"{_objectType}{PluralSuffix(_number)} ");
                sb.Append($"ha{HaveSuffix(_number)} been ");
                sb.Append($"{_action}d ");
                sb.Append($"{_preposition} the ");
                sb.Append($"'{_phaseName}' Phase ");
                sb.Append($"\n{_suffix}");
                return sb.ToString();
            }
        }

        private readonly string _action;
        private readonly int _number;
        private readonly string _objectType;
        private readonly string _preposition;
        private readonly int _phasesNumber;
        private readonly string _phases;
        private readonly string _suffix;
        private readonly string _phaseName;

        internal MessageGenerator(string objectType, int number, List<PhaseElement> phases, Actions action)
        {
            _action = GetActionDescription(action);
            _number = number;
            _objectType = objectType;
            _preposition = GetActionPreposition(action);
            _phasesNumber = phases.Count;
            _phases = GetPhasesString(phases);
            _suffix = GetActionSuffix(action);
        }

        internal MessageGenerator(string objectType, int number, PhaseElement phaseElement, Actions action)
        {
            _action = GetActionDescription(action);
            _number = number;
            _objectType = objectType;
            _preposition = GetActionPreposition(action);
            _phaseName = phaseElement.Name;
            _suffix = GetActionSuffix(action);
        }

        private string GetPhasesString(List<PhaseElement> phases)
        {
            string phasesString = string.Empty;
            foreach (PhaseElement phase in phases)
                phasesString += $"   - {phase.Name}\n";
            return phasesString;
        }

        private string GetActionDescription(Actions action)
        {
            if (action == Actions.Create)
                return "create";
            else
                return "delete";
        }

        private string GetActionPreposition(Actions action)
        {
            if (action == Actions.Create)
                return "from";
            else
                return "in";
        }

        private string GetActionSuffix(Actions action)
        {
            if (action == Actions.Create)
                return "of the selected Linked Model";
            else
                return "of the current Model";
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
    }
}
