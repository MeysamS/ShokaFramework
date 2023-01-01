using System;
using System.Linq;
using Nuke.Common;
using Nuke.Common.CI;
using Nuke.Common.Execution;
using Nuke.Common.Git;
using Nuke.Common.IO;
using Nuke.Common.ProjectModel;
using Nuke.Common.Tooling;
using Nuke.Common.Tools.DotNet;
using Nuke.Common.Tools.GitVersion;
using Nuke.Common.Utilities.Collections;
using static Nuke.Common.EnvironmentInfo;
using static Nuke.Common.IO.FileSystemTasks;
using static Nuke.Common.IO.PathConstruction;
using static Nuke.Common.Tools.DotNet.DotNetTasks;

class Build : NukeBuild
{
    /// Support plugins are available for:
    ///   - JetBrains ReSharper        https://nuke.build/resharper
    ///   - JetBrains Rider            https://nuke.build/rider
    ///   - Microsoft VisualStudio     https://nuke.build/visualstudio
    ///   - Microsoft VSCode           https://nuke.build/vscode

    public static int Main() => Execute<Build>(x => x.Compile);

    [Parameter("Configuration to build - Default is 'Debug' (local) or 'Release' (server)")]
    readonly Configuration Configuration = IsLocalBuild ? Configuration.Debug : Configuration.Release;

    [Solution] readonly Solution solution;
    [GitRepository] readonly GitRepository repository;
    [GitVersion] readonly GitVersion gitVersion;

    [Parameter] string NugetApiUrl = "";
    [Parameter] string NugetApiKey;

    AbsolutePath SourceDirectory => RootDirectory / "src";
    AbsolutePath TestsDirectory => RootDirectory / "tests";
    AbsolutePath ArtifactsDirectory => RootDirectory / "artifacts";
    AbsolutePath NugetDirectory => ArtifactsDirectory / "nuget";



    Target Clean => _ => _
        .Before(Restore)
        .Executes(() =>
        {
            SourceDirectory.GlobDirectories("**/bin", "**/obj").ForEach(DeleteDirectory);
            TestsDirectory.GlobDirectories("**/bin", "**/obj").ForEach(DeleteDirectory);
            EnsureCleanDirectory(ArtifactsDirectory);
        });

    Target Restore => _ => _
        .Executes(() =>
        {
            DotNetRestore(_ => _.SetProjectFile(solution));
        });

    Target Compile => _ => _
        .DependsOn(Restore)
        .Executes(() =>
        {
            DotNetBuild(_ => _
            .SetProjectFile(solution)
            .SetAuthors("SADAD.Securities")
            .SetConfiguration(Configuration)
            .SetAssemblyVersion(gitVersion.AssemblySemVer)
            .SetFileVersion(gitVersion.AssemblySemFileVer)
            .SetInformationalVersion(gitVersion.InformationalVersion)
            .EnableNoRestore());
        });

    Target Pack => _ => _
    .Executes(() =>
    {
        int commitNum = 0;
        string NuGetVersionCustom = gitVersion.NuGetVersionV2;

        if (Int32.TryParse(gitVersion.CommitsSinceVersionSource, out commitNum))
            NuGetVersionCustom = commitNum > 0 ? NuGetVersionCustom + $"{commitNum}" : NuGetVersionCustom;

        DotNetPack(s => s
           .SetProject(solution.GetProject("Shoka.Core"))
           .SetConfiguration(Configuration)
           .EnableNoBuild()
           .EnableNoRestore()
           .SetVersion(NuGetVersionCustom)
           .SetDescription("Core library Shoka framework")
           .SetPackageTags("Core lib Description")
           .SetNoDependencies(true)
           .SetOutputDirectory(ArtifactsDirectory / "nuget"));

        DotNetPack(s => s
            .SetProject(solution.GetProject("Shoka.Domain"))
            .SetConfiguration(Configuration)
            .EnableNoBuild()
            .EnableNoRestore()
            .SetVersion(NuGetVersionCustom)
            .SetDescription("Domain library Shoka framework")
            .SetPackageTags("Domain lib Description")
            .SetNoDependencies(true)
            .SetOutputDirectory(ArtifactsDirectory / "nuget"));
    });
}
