using Autodesk.Revit.DB;

namespace RevitSpacesManager.Models
{
    // В Revit 2026 удалены ElementId.IntegerValue и ElementId(int), используется Int64
    internal static class ElementIdExtensions
    {
        internal static int ToInt(this ElementId id)
        {
#if _2026
            return (int)id.Value;
#else
            return id.IntegerValue;
#endif
        }

        internal static ElementId ToElementId(this int id)
        {
#if _2026
            return new ElementId((long)id);
#else
            return new ElementId(id);
#endif
        }
    }
}
