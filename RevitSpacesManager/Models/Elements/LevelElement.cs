using Autodesk.Revit.DB;
using System;

namespace RevitSpacesManager.Models
{
    internal class LevelElement
    {
        internal string Name => _level.Name;
        internal ElementId ElementId => _level.Id;
        internal int Id => ElementId.IntegerValue;
        internal double ProjectElevation => _level.ProjectElevation;
        internal double MatchedLevelId { get; set; } = 0;

        private readonly Level _level;
        private const int _roundingDigit = 2;


        internal LevelElement(Level level)
        {
            _level = level;
        }


        internal void ClearMatching()
        {
            MatchedLevelId = 0;
        }

        internal void CompareByElevationWith(LevelElement matchingLevelElement)
        {
            double matchingLevelElevation = Math.Round(matchingLevelElement.ProjectElevation, _roundingDigit);
            double curentLevelElevation = Math.Round(ProjectElevation, _roundingDigit);
            if (matchingLevelElevation == curentLevelElevation)
            {
                MatchedLevelId = matchingLevelElement.Id;
            }
        }
    }
}
