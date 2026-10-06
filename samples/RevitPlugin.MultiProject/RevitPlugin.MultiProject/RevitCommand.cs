using System;
using System.Linq;
using System.Reflection;

using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;

using RevitPlugin.MultiProject.Views;

namespace RevitPlugin.MultiProject;

[Transaction(TransactionMode.Manual)]
[Regeneration(RegenerationOption.Manual)]
public sealed partial class RevitCommand : IExternalCommand {
    public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements) {
        MainWindow window = new() {
            Greeting = $"Hello Revit {RevitVersion}!" +
                       $"{Environment.NewLine}" +
                       $"OrGreater: {GetGreaterVersions()}"
        };

        window.ShowDialog();
        return Result.Succeeded;
    }

    private string GetGreaterVersions() {
        IEnumerable<object?> values = typeof(RevitCommand)
            .GetProperties(BindingFlags.Instance | BindingFlags.Public)
            .Where(item => item.Name.StartsWith("RevitVersionOrGreater"))
            .Select(item => item.GetValue(this));

        return string.Join(";", values);
    }
}