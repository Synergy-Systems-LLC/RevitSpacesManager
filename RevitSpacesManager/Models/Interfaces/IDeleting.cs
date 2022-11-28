namespace RevitSpacesManager.Models
{
    internal interface IDeleting
    {
        void DeleteAll();
        void DeleteByPhase(PhaseElement phaseElement);
    }
}
