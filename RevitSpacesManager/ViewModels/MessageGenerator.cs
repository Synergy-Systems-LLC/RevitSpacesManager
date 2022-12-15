using RevitSpacesManager.Models;
using System.Collections.Generic;
using System.Text;

namespace RevitSpacesManager.ViewModels
{
    internal class MessageGenerator
    {
        private readonly int _number;
        private readonly string _objectType;
        private readonly int _phasesNumber;
        private readonly string _phases;
        private readonly string _phaseName;

        internal MessageGenerator(string objectType, int number, List<PhaseElement> phases)
        {
            _number = number;
            _objectType = objectType;
            _phasesNumber = phases.Count;
            _phases = GetPhasesString(phases);
        }

        internal MessageGenerator(string objectType, int number, PhaseElement phaseElement)
        {
            _number = number;
            _objectType = objectType;
            _phaseName = phaseElement.Name;
        }


        internal string MessageCreateAll()
        {
            StringBuilder sb = new StringBuilder();
            return sb.ToString();
        }

        internal string ReportCreateAll()
        {
            StringBuilder sb = new StringBuilder();
            return sb.ToString();
        }

        internal string MessageCreateSelected()
        {
            StringBuilder sb = new StringBuilder();
            return sb.ToString();
        }

        internal string ReportCreateSelected()
        {
            StringBuilder sb = new StringBuilder();
            return sb.ToString();
        }

        internal string MessageDeleteAll()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append($"You are going to delete ");
            sb.Append($"{_number} ");
            sb.Append($"{_objectType}{PluralSuffix(_number)} in ");
            sb.Append($"{_phasesNumber} ");
            sb.Append($"Phase{PluralSuffix(_phasesNumber)} of the current Model:\n");
            sb.Append($"{_phases}");
            return sb.ToString();
        }

        internal string ReportDeleteAll()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append($"{_number} ");
            sb.Append($"{_objectType}{PluralSuffix(_number)} ");
            sb.Append($"ha{HaveSuffix(_number)} been deleted in ");
            sb.Append($"{_phasesNumber} ");
            sb.Append($"Phase{PluralSuffix(_phasesNumber)} of the current Model");
            return sb.ToString();
        }

        internal string MessageDeleteSelected()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append($"You are going to delete ");
            sb.Append($"{_number} ");
            sb.Append($"{_objectType}{PluralSuffix(_number)} in the ");
            sb.Append($"'{_phaseName}' Phase \nof the current Model");
            return sb.ToString();
        }

        internal string ReportDeleteSelected()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append($"{_number} ");
            sb.Append($"{_objectType}{PluralSuffix(_number)} ");
            sb.Append($"ha{HaveSuffix(_number)} been deleted in the ");
            sb.Append($"'{_phaseName}' Phase \nof the current Model");
            return sb.ToString();
        }


        private string GetPhasesString(List<PhaseElement> phases)
        {
            string phasesString = string.Empty;
            foreach (PhaseElement phase in phases)
                phasesString += $"   - {phase.Name}\n";
            return phasesString;
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
