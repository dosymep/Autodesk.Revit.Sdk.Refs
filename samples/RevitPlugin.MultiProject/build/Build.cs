using System.Linq;

using Nuke.Common;
using Nuke.Common.IO;
using Nuke.Common.ProjectModel;
using Nuke.Common.Tooling;
using Nuke.Common.Tools.DotNet;
using Nuke.Common.Utilities;
using Nuke.Components;

using static Nuke.Common.Tools.DotNet.DotNetTasks;

class Build : NukeBuild {
    /// Support plugins are available for:
    /// - JetBrains ReSharper        https://nuke.build/resharper
    /// - JetBrains Rider            https://nuke.build/rider
    /// - Microsoft Visual Studio    https://nuke.build/visualstudio
    /// - Microsoft VSCode           https://nuke.build/vscode
    public static int Main() => Execute<Build>(x => x.Compile);

    [Solution] public readonly Solution Solution;

    [Parameter("Configuration to build - Default is 'Debug' (local) or 'Release' (server)")]
    readonly Configuration Configuration = IsLocalBuild ? Configuration.Debug : Configuration.Release;

    /// <summary>
    /// Output directory.
    /// </summary>
    AbsolutePath Output => RootDirectory / "bin";

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
                .SetProjectFile(Solution));
        });

    /// <summary>
    /// Compile the project with all revit versions.
    /// </summary>
    Target Compile => _ => _
        .DependsOn(Restore)
        .Executes(() => {
            var revitProjects = Solution.AllProjects
                .Where(project => project.Name.StartsWith("RevitPlugin.MultiProject"));

            DotNetBuild(s => s
                .SetConfiguration(Configuration)
                .When(IsServerBuild, _ => _
                    .EnableContinuousIntegrationBuild())
                .CombineWith(revitProjects, (settings, revitProject) => settings
                    .SetProjectFile(revitProject)
                    .SetOutputDirectory(Output)));
        });
}