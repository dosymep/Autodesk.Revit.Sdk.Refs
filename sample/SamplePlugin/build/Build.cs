using System.Collections.Generic;
using System.Linq;

using Nuke.Common;
using Nuke.Common.IO;
using Nuke.Common.Tooling;
using Nuke.Common.Tools.DotNet;
using Nuke.Components;

using static Nuke.Common.Tools.DotNet.DotNetTasks;

class Build : NukeBuild {
    /// Support plugins are available for:
    /// - JetBrains ReSharper        https://nuke.build/resharper
    /// - JetBrains Rider            https://nuke.build/rider
    /// - Microsoft VisualStudio     https://nuke.build/visualstudio
    /// - Microsoft VSCode           https://nuke.build/vscode
    public static int Main() => Execute<Build>(x => x.Compile);

    [Parameter("Configuration to build - Default is 'Debug' (local) or 'Release' (server)")]
    readonly Configuration Configuration = IsLocalBuild ? Configuration.Debug : Configuration.Release;

    /// <summary>
    /// The name of the project to be compiled.
    /// </summary>
    string ProjectName => "SamplePlugin";

    /// <summary>
    /// Output directory.
    /// </summary>
    AbsolutePath OutputDirectory => RootDirectory / "bin";

    /// <summary>
    /// Project directory.
    /// </summary>
    AbsolutePath ProjectDirectory => RootDirectory / ProjectName;

    /// <summary>
    /// Cleans <see cref="Build.OutputDirectory"/> and build and obj folders in project.
    /// </summary>
    Target Clean => _ => _
        .Executes(() => {
            OutputDirectory.CreateOrCleanDirectory();
            ProjectDirectory.GlobDirectories("**/bin", "**/obj")
                .Where(item => item != RootDirectory / "build" / "bin")
                .Where(item => item != RootDirectory / "build" / "obj")
                .DeleteDirectories();
        });

    /// <summary>
    /// Compile project with all revit versions.
    /// </summary>
    Target Compile => _ => _
        .DependsOn(Clean)
        .Executes(() => {
            DotNetBuild(s => s
                .EnableForce()
                .DisableNoRestore()
                .SetProjectFile(ProjectName)
                .SetConfiguration(Configuration)
                .When(IsServerBuild, _ => _
                    .EnableContinuousIntegrationBuild())
                .SetProcessArgumentConfigurator(a => a.Add("--restore"))
                .CombineWith(RevitVersion.GetRevitVersions(), (settings, version) => {
                    return settings
                        .SetOutputDirectory(OutputDirectory / version)
                        .SetProperty("RevitVersion", (int) version)
                        .SetProperty("TargetFramework", version.TargetFramework);
                }));
        });
}