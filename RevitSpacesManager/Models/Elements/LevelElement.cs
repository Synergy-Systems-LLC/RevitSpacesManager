using Autodesk.Revit.DB;

namespace RevitSpacesManager.Models
{
    internal class LevelElement
    {
        internal string Name => _level.Name;
        internal double ProjectElevation => _level.ProjectElevation;

        private readonly Level _level;

        internal LevelElement(Level level)
        {
            _level = level;
        }
    }
}
