using Autodesk.Revit.DB;

namespace RevitPlugin.Library;

/// <summary>
///     Provides extension methods for Revit <see cref="Autodesk.Revit.DB.ElementId"/> objects.
/// </summary>
public static class ElementIdExtensions {
#if REVIT2024_OR_GREATER
    /// <summary>
    ///     Gets the internal representation value of the element ID.
    /// </summary>
    /// <param name="elementId">The element ID.</param>
    /// <returns>The 64-bit integer representation of the element ID.</returns>
    public static long GetIdValue(this ElementId elementId) {
        return elementId.Value;
    }
#else
    /// <summary>
    ///     Gets the internal representation value of the element ID.
    /// </summary>
    /// <param name="elementId">The element ID.</param>
    /// <returns>The 32-bit integer representation of the element ID.</returns>
        public static int GetIdValue(this ElementId elementId) {
            return elementId.IntegerValue;
        }
#endif
}