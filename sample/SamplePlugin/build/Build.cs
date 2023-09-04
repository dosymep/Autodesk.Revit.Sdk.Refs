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

    string ProjectName => "SamplePlugin";
    AbsolutePath OutputDirectory => RootDirectory / "bin";
    AbsolutePath ProjectDirectory => RootDirectory / ProjectName;

    Target Clean => _ => _
        .Executes(() => {
            OutputDirectory.CreateOrCleanDirectory();
            ProjectDirectory.GlobDirectories("**/bin", "**/obj")
                .Where(item => item != RootDirectory / "build" / "bin")
                .Where(item => item != RootDirectory / "build" / "obj")
                .DeleteDirectories();
        });

    Target Compile => _ => _
        .DependsOn(Clean)
        .Executes(() => {
            DotNetBuild(s => s
                .DisableNoRestore()
                .SetProjectFile(ProjectName)
                .SetConfiguration(Configuration)
                //.SetOutputDirectory(OutputDirectory)
                .When(IsServerBuild, _ => _
                    .EnableContinuousIntegrationBuild())
                .CombineWith(RevitVersion.GetRevitVersions(), (settings, version) => {
                    return settings
                        .SetOutputDirectory(OutputDirectory / version)
                        .SetProperty("RevitVersion", (int) version);
                    //.SetProperty("AssemblyName", $"TestSdk_{version}");
                }));
        });
}