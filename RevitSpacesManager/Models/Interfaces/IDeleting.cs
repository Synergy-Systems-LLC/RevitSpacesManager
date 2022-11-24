namespace RevitSpacesManager.Models
{
    internal interface IDeleting
    {
        void DeleteAll();
        void DeleteSelected(PhaseElement phaseElement);
    }
}
