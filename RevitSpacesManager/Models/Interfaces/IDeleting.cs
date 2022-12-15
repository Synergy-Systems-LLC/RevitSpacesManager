namespace RevitSpacesManager.Models
{
    internal interface IDeleting
    {
        void DeleteAll();
        void DeleteByPhase(PhaseElement phaseElement);
        bool AreAllNotEditable();
        bool ArePhaseElementsNotEditable(PhaseElement phaseElement);
    }
}
