using Autodesk.Revit.DB;

namespace RevitPlugin.Library;

/// <summary>
///     Provides extension methods for Revit <see cref="Autodesk.Revit.DB.Element"/> objects.
/// </summary>
public static class ElementExtensions {
    /// <param name="element">The element.</param>
    extension(Element element) {
        /// <summary>
        ///     Gets the parameter value or a default value.
        /// </summary>
        /// <param name="builtInParameter">The built-in parameter.</param>
        /// <param name="default">The default value.</param>
        /// <returns>The parameter value, or the default value if the parameter does not exist.</returns>
        public object? GetParamValueOrDefault(BuiltInParameter builtInParameter,
            object? @default = null) {
            if(element is null) {
                throw new ArgumentNullException(nameof(element));
            }

            try {
                return element.GetParamValue(builtInParameter) ?? @default;
            } catch(ArgumentException) {
                return @default;
            }
        }

        /// <summary>
        ///     Gets the parameter value of the element.
        /// </summary>
        /// <param name="builtInParameter">The built-in parameter.</param>
        /// <returns>The parameter value cast to <typeparamref name="T" />.</returns>
        public T? GetParamValue<T>(BuiltInParameter builtInParameter) {
            if(element is null) {
                throw new ArgumentNullException(nameof(element));
            }

            object? value = element.GetParam(builtInParameter).AsObject();
            return value == null ? default : (T) value;
        }

        /// <summary>
        ///     Gets the parameter value of the element.
        /// </summary>
        /// <param name="builtInParameter">The built-in parameter.</param>
        /// <returns>The parameter value.</returns>
        public object? GetParamValue(BuiltInParameter builtInParameter) {
            if(element is null) {
                throw new ArgumentNullException(nameof(element));
            }

            return element.GetParam(builtInParameter).AsObject();
        }

        /// <summary>
        ///     Gets the parameter of the element.
        /// </summary>
        /// <param name="builtInParameter">The built-in parameter.</param>
        /// <returns>The parameter.</returns>
        public Parameter GetParam(BuiltInParameter builtInParameter) {
            if(element is null) {
                throw new ArgumentNullException(nameof(element));
            }

            Parameter parameter = element.get_Parameter(builtInParameter);
            if(parameter is null) {
                throw new ArgumentException(
                    $"The parameter '{builtInParameter}' does not exist on the element with Id: {element.Id}.",
                    nameof(builtInParameter));
            }

            return parameter;
        }
    }
}