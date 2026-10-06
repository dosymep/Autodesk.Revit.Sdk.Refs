using System.Linq;

using Nuke.Common;
using Nuke.Common.IO;
using Nuke.Common.ProjectModel;
using Nuke.Common.Tooling;
using Nuke.Common.Tools.DotNet;
using Nuke.Common.Utilities;
using Nuke.Components;

using static Nuke.Common.Tools.DotNet.DotNetTasks;

class Build : NukeBuild, IHazSolution {
    /// Support plugins are available for:
    /// - JetBrains ReSharper        https://nuke.build/resharper
    /// - JetBrains Rider            https://nuke.build/rider
    /// - Microsoft Visual Studio    https://nuke.build/visualstudio
    /// - Microsoft VSCode           https://nuke.build/vscode
    public static int Main() => Execute<Build>(x => x.Compile);

    [Parameter("Configuration to build - Default is 'Debug' (local) or 'Release' (server)")]
    readonly Configuration Configuration = IsLocalBuild ? Configuration.Debug : Configuration.Release;

    /// <summary>
    /// Output directory.
    /// </summary>
    AbsolutePath Output => RootDirectory / "bin";

    /// <summary>
    /// Build Revit versions.
    /// </summary>
    [Parameter("Build Revit versions.", List = true)]
    readonly string[] RevitVersions = [];

    /// <summary>
    /// Cleans <see cref="Output"/> and build and obj folders in the project.
    /// </summary>
    Target Clean => _ => _
        .Executes(() => {
            Output.CreateOrCleanDirectory();
            RootDirectory.GlobDirectories("**/bin", "**/obj")
                .Where(item => item != RootDirectory / "build" / "bin")
                .Where(item => item != RootDirectory / "build" / "obj")
                .DeleteDirectories();
        });

    Target Restore => _ => _
        .DependsOn(Clean)
        .Executes(() => {
            DotNetRestore(s => s
                .SetProjectFile(((IHazSolution) this).Solution));
        });

    /// <summary>
    /// Compile the project with all revit versions.
    /// </summary>
    Target Compile => _ => _
        .DependsOn(Restore)
        .Executes(() => {
            DotNetBuild(s => s
                .SetConfiguration(Configuration)
                .SetProjectFile(((IHazSolution) this)
                    .Solution.GetProject("RevitPlugin.SingleAssembly"))
                .When(IsServerBuild, _ => _
                    .EnableContinuousIntegrationBuild())
                .CombineWith(RevitVersions, (settings, version) => settings
                    .SetOutputDirectory(Output / version)
                    .SetProperty("RevitVersion", version)));
        });
}