namespace RevitSpacesManager.Models
{
    internal interface IDeleting
    {
        bool AreNotAllElementsEditable();
        bool AreNotAllPhaseElementsEditable(PhaseElement phaseElement);
        void DeleteAll();
        void DeleteByPhase(PhaseElement phaseElement);
    }
}
