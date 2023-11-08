using System.Collections.Generic;
using System.Linq;

using dosymep.Nuke.RevitVersions;

using Nuke.Common;
using Nuke.Common.IO;
using Nuke.Common.Tooling;
using Nuke.Common.Tools.DotNet;
using Nuke.Components;

using static Nuke.Common.Tools.DotNet.DotNetTasks;

class Build : NukeBuild, IHazSolution {
    /// Support plugins are available for:
    /// - JetBrains ReSharper        https://nuke.build/resharper
    /// - JetBrains Rider            https://nuke.build/rider
    /// - Microsoft VisualStudio     https://nuke.build/visualstudio
    /// - Microsoft VSCode           https://nuke.build/vscode
    public static int Main() => Execute<Build>(x => x.Compile);

    [Parameter("Configuration to build - Default is 'Debug' (local) or 'Release' (server)")]
    readonly Configuration Configuration = IsLocalBuild ? Configuration.Debug : Configuration.Release;

    /// <summary>
    /// Output directory.
    /// </summary>
    AbsolutePath Output => RootDirectory / "bin";
    
    /// <summary>
    /// Min Revit version.
    /// </summary>
    [Parameter("Min Revit version.")]
    readonly RevitVersion MinVersion = RevitVersion.Rv2020;

    /// <summary>
    /// Max Revit version.
    /// </summary>
    [Parameter("Max Revit version.")]
    readonly RevitVersion MaxVersion = RevitVersion.Rv2024;
    
    [Parameter("Build Revit versions.")] readonly RevitVersion[] RevitVersions = new RevitVersion[0];

    IEnumerable<RevitVersion> BuildRevitVersions;

    protected override void OnBuildInitialized() {
        base.OnBuildInitialized();
        BuildRevitVersions = RevitVersions.Length > 0
            ? RevitVersions
            : RevitVersion.GetRevitVersions(MinVersion, MaxVersion);
    }

    /// <summary>
    /// Cleans <see cref="Output"/> and build and obj folders in project.
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
    /// Compile project with all revit versions.
    /// </summary>
    Target Compile => _ => _
        .DependsOn(Restore)
        .Executes(() => {
            DotNetBuild(s => s
                .EnableForce()
                .DisableNoRestore()
                .SetConfiguration(Configuration)
                .SetProjectFile(((IHazSolution) this).Solution)
                .When(IsServerBuild, _ => _
                    .EnableContinuousIntegrationBuild())
                .CombineWith(BuildRevitVersions, (settings, version) => {
                    return settings
                        .SetOutputDirectory(Output / version)
                        .SetProperty("RevitVersion", (int) version);
                }));
        });
}