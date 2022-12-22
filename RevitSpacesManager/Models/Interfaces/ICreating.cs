namespace RevitSpacesManager.Models
{
    internal interface ICreating
    {
        bool IsWorksetNotAvailable();
        void MatchLevels(IRoomLevelsMatchable levelMatchable);
        RoomsVerificationReport VerifyLinkRooms(RevitDocument linkDocument);
        RoomsVerificationReport VerifyPhaseRooms(PhaseElement phaseElement);
        void CreateAllByLinkedDocument(RevitDocument linkDocument); 
        void CreateByLinkedDocumentPhase(PhaseElement phaseElement);
    }
}
