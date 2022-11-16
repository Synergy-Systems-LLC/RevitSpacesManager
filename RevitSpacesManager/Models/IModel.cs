using System.Collections.Generic;

namespace RevitSpacesManager.Models
{
    internal interface IModel
    {
        RevitDocument RevitDocument { get; set; }
        int NumberOfElements { get; }
        string ObjectName { get; }
        string SelectedPhaseDisplayPath { get; }

        List<PhaseElement> GetPhases();
        void DeleteAll();
        void DeleteSelected(); 
        void CreateAll();
        void CreateSelected();
    }
}
