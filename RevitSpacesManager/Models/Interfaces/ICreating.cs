namespace RevitSpacesManager.Models
{
    internal interface ICreating
    {
        void CreateAllByLinkedDocument(RevitDocument linkDocument); 
        void CreateByLinkedDocumentPhase(PhaseElement phaseElement);
        bool IsWorksetNotAvailable();
    }
}
