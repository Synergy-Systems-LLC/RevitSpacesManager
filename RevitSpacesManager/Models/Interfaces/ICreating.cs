namespace RevitSpacesManager.Models
{
    internal interface ICreating
    {
        bool IsWorksetNotAvailable();
        RoomsVerificationReport VerifyLinkRooms(RevitDocument linkDocument);
        RoomsVerificationReport VerifyPhaseRooms(PhaseElement phaseElement);
        void CreateAllByLinkedDocument(RevitDocument linkDocument); 
        void CreateByLinkedDocumentPhase(PhaseElement phaseElement);
    }
}
