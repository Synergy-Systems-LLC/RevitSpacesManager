
using System.Collections.Generic;

namespace RevitSpacesManager.Models
{
    internal class MessageGenerator
    {
        internal string MessageAll
        {
            get => $"You are going to {_action} " +
                                    $"{_number} " +
                                    $"{_objectType}{IsPlural(_number)} " +
                                    $"{_preposition} " +
                                    $"{_phasesNumber} " +
                                    $"Phase{IsPlural(_phasesNumber)} " +
                                    $"{_suffix}:\n" +
                                    $"{_phases}";
        }

        internal string ReportAll
        {
            get => $"{_number} " +
                   $"{_objectType}{IsPlural(_number)} " +
                   $"ha{IsS(_number)} been " +
                   $"{_action}d " +
                   $"{_preposition} " +
                   $"{_phasesNumber} " +
                   $"Phase{IsPlural(_phasesNumber)} " +
                   $"{_suffix}";
        }

        internal string MessageSelected
        {
            get => $"You are going to {_action} " +
                                    $"{_number} " +
                                    $"{_objectType}{IsPlural(_number)} " +
                                    $"{_preposition} the " +
                                    $"'{_phaseName}' Phase " +
                                    $"\n{_suffix}";
        }

        internal string ReportSelected
        {
            get => $"{_number} " +
                   $"{_objectType}{IsPlural(_number)} " +
                   $"ha{IsS(_number)} been " +
                   $"{_action}d " +
                   $"{_preposition} the " +
                   $"'{_phaseName}' Phase " +
                   $"\n{_suffix}";
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

        private string IsPlural(int number)
        {
            if (number == 1)
                return "";
            return "s";
        }

        private string IsS(int number)
        {
            if (number == 1)
                return "s";
            return "ve";
        }
    }
}
