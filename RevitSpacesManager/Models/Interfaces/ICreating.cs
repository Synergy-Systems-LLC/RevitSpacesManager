namespace RevitSpacesManager.Models
{
    internal interface ICreating
    {
        void CreateAllByLinkedDocument(RevitDocument linkDocument); 
        void CreateByLinkedDocumentPhase(PhaseElement phaseElement);
        bool IsWorksetNotAvailable();
        RoomsVerificationReport VerifyLinkRooms(RevitDocument linkDocument);
        RoomsVerificationReport VerifyPhaseRooms(PhaseElement phaseElement);
    }
}
