using Autodesk.Revit.DB;

namespace RevitPlugin.Library;

/// <summary>
///     Provides extension methods for Revit <see cref="Parameter"/> objects.
/// </summary>
public static class ParamExtension {
    /// <param name="parameter">The parameter.</param>
    extension(Parameter parameter) {
        /// <summary>
        ///     Gets the value of the parameter.
        /// </summary>
        /// <returns>The value of the parameter, or <see langword="null" /> if it has no value.</returns>
        public object? AsObject() {
            if(parameter is null) {
                throw new ArgumentNullException(nameof(parameter));
            }

            if(parameter.HasValue) {
                StorageType storageType = parameter.StorageType;
                switch(storageType) {
                    case StorageType.Integer:
                        return parameter.AsInteger();
                    case StorageType.Double:
                        return parameter.AsDouble();
                    case StorageType.String: {
                        string value = parameter.AsString();
                        return string.IsNullOrWhiteSpace(value) ? null : value;
                    }
                    case StorageType.ElementId:
                        return parameter.AsElementId();
                    case StorageType.None:
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }

            return null;
        }

        /// <summary>
        ///     Removes the value of the parameter by setting it to its default value.
        /// </summary>
        public void RemoveValue() {
            if(parameter is null) {
                throw new ArgumentNullException(nameof(parameter));
            }

            if(parameter.HasValue) {
                StorageType storageType = parameter.StorageType;
                switch(storageType) {
                    case StorageType.Integer:
                        parameter.Set((int) 0);
                        break;
                    case StorageType.Double:
                        parameter.Set((double) 0);
                        break;
                    case StorageType.String:
                        parameter.Set((string?) null);
                        break;
                    case StorageType.ElementId:
                        parameter.Set((ElementId?) null);
                        break;
                    case StorageType.None:
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }
        }

        /// <summary>
        ///     Sets the value of the parameter to the value of another parameter.
        /// </summary>
        /// <param name="rightParameter">The parameter whose value to assign.</param>
        public void Set(Parameter rightParameter) {
            if(parameter is null) {
                throw new ArgumentNullException(nameof(parameter));
            }

            if(rightParameter is null) {
                throw new ArgumentNullException(nameof(rightParameter));
            }

            if(parameter.StorageType != StorageType.String &&
               parameter.StorageType != rightParameter.StorageType) {
                throw new ArgumentException(
                    "The storage type of the source parameter does not match the target parameter.",
                    nameof(rightParameter));
            }

            StorageType storageType = rightParameter.StorageType;
            switch(storageType) {
                case StorageType.Integer:
                    parameter.Set(rightParameter.AsInteger());
                    break;
                case StorageType.Double:
                    parameter.Set(rightParameter.AsDouble());
                    break;
                case StorageType.String:
                    parameter.Set(rightParameter.AsObject()?.ToString());
                    break;
                case StorageType.ElementId:
                    parameter.Set(rightParameter.AsElementId());
                    break;
                case StorageType.None:
                default:
                    parameter.Set((string?) null);
                    break;
            }
        }
    }
}