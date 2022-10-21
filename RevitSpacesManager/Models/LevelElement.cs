using Autodesk.Revit.DB;

namespace RevitSpacesManager.Models
{
    internal class LevelElement
    {
        internal Level Level { get; set; }
        internal string Name { get; set; }
        internal double ProjectElevation { get; set; }

        internal LevelElement(Level level)
        {
            Level = level;
            Name = level.Name;
            ProjectElevation = level.ProjectElevation;
        }
    }
}
