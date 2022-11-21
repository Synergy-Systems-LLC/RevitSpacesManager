namespace RevitSpacesManager.Models
{
    internal interface IDeleting
    {
        void DeleteAll(string activeObject);
        void DeleteSelected(string activeObject, PhaseElement phaseElement);
    }
}
