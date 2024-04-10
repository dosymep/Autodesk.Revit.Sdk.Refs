using System;
using System.Linq;
using System.Reflection;

using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;

using SamplePlugin.Views;

namespace SamplePlugin {
    [Transaction(TransactionMode.Manual)]
    [Regeneration(RegenerationOption.Manual)]
    public sealed partial class RevitCommand : IExternalCommand {
        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements) {
            var window = new MainWindow();
            window.Greeting = $"Hello Revit {RevitVersion}!" +
                              $"{Environment.NewLine}" +
                              $"OrGreater: {GetGreaterVersions()}";
            
            window.ShowDialog();
            return Result.Succeeded;
        }

        private string GetGreaterVersions() {
            var values = typeof(RevitCommand)
                .GetProperties(BindingFlags.Instance | BindingFlags.Public)
                .Where(item => item.Name.StartsWith("RevitVersionOrGreater"))
                .Select(item => item.GetValue(this));

            return string.Join(";", values);
        }
    }
}