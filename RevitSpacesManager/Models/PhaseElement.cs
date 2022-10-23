using Autodesk.Revit.DB;

namespace RevitSpacesManager.Models
{
    internal class PhaseElement
    {
        Phase Phase { get; set; }
        internal int Id { get; set; }
        internal string Name { get; set; }

        internal PhaseElement(Phase phase)
        {
            Phase = phase;
            Id = Phase.Id.IntegerValue;
            Name = Phase.Name;
        }
    }
}
